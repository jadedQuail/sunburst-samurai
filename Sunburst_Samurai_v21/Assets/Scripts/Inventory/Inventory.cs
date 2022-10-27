using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class Inventory : MonoBehaviour
    {
        public List<GameObject> inventory = new List<GameObject>();

        // Function that appends an item to the inventory
        public void AppendItem(GameObject item)
        {
            inventory.Add(item);
        }

        // Function that removes an item from the inventory
        public void RemoveItem(GameObject item)
        {
            inventory.Remove(item);
        }

        // Function that returns a string of the item name, so it can be written to the HUD
        public string WriteItemToHUD()
        {
            if (inventory.Count == 0)
            {
                return "Null";
            }
            else
            {
                return inventory[0].GetComponent<EquippedWeapon>().GetWeaponName();
            }
        }
    }
}

