using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Display;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] GameObject equippedWeapon;
        [SerializeField] float weaponDamage;
        [SerializeField] float pickupRange = 5.0f;

        GameObject player;
        GameObject hud;

        private bool mouseHovering = false;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            hud = GameObject.FindWithTag("HUD");
        }

        private void Update()
        {
            // Turn off the item text if any menu is open
            if (hud.GetComponent<MenuManager>().AnyMenuOpen())
            {
                GetComponentInChildren<ItemText>().TextOff();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                // If no weapon is equipped, equip the first one
                // And, activate the checkmark
                if(player.GetComponent<Fighter>().currentWeapon == null)
                {
                    player.GetComponent<Fighter>().EquipWeapon(equippedWeapon, weaponDamage);
                    GameObject.FindWithTag("HUD").GetComponent<InventoryMenu>().ActivateCheckMark(0);
                    GameObject.FindWithTag("HUD").GetComponent<InventoryMenu>().FindEquippedWeaponPicture(equippedWeapon.GetComponent<EquippedWeapon>().GetWeaponSprite());
                }
                
                player.GetComponent<Inventory>().AppendItem(equippedWeapon);

                Destroy(gameObject);
                GetComponentInChildren<ItemText>().TextOff();
            }
        }

        // Shows the item's text when hovered over
        private void OnMouseOver()
        {
            if (GetIsInPickupRange() && !hud.GetComponent<MenuManager>().AnyMenuOpen())
            {
                GetComponentInChildren<ItemText>().TextOn();
            }
            mouseHovering = true;
        }

        // Hides the item's text when not hovered over
        private void OnMouseExit()
        {
            GetComponentInChildren<ItemText>().TextOff();
            mouseHovering = false;
        }

        // Checks if the weapon and player are in range
        private bool GetIsInPickupRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < pickupRange;
        }
    }
}

