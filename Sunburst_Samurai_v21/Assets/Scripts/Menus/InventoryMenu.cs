using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;
using RPG.Combat;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] Image equippedWeaponImage;

    [SerializeField] List<Image> itemImages = new List<Image>();
    [SerializeField] List<Text> itemTexts = new List<Text>();
    [SerializeField] List<Image> checkImages = new List<Image>();

    [SerializeField] Sprite gray;

    int inventorySlots = 10;

    GameObject player;
    List<GameObject> inventory;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        //Disable all checks
        foreach(Image check in checkImages)
        {
            check.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FillOutInventory();
    }

    // Function that fills out the inventory menu with the items in the player's inventory
    private void FillOutInventory()
    {
        int i = 0;
        foreach (GameObject item in player.GetComponent<Inventory>().inventory)
        {
            itemTexts[i].text = item.GetComponent<EquippedWeapon>().GetWeaponName();
            itemImages[i].sprite = item.GetComponent<EquippedWeapon>().GetWeaponSprite();

            i = i + 1;
        }

        for (int j = i; j < inventorySlots; j++)
        {
            itemTexts[j].text = "EMPTY";
            itemImages[j].sprite = gray;
        }
    }

    // Function that registers a weapon button being clicked, and switches to that weapon
    public void SelectItem(int itemNumber)
    {
        // Cache the player's inventory
        List<GameObject> playerInventory = player.GetComponent<Inventory>().inventory;

        // Equip the correct weapon from the inventory when the player clicks it
        player.GetComponent<Fighter>().EquipWeapon(playerInventory[itemNumber], playerInventory[itemNumber].GetComponent<EquippedWeapon>().GetWeaponDamage());

        // Add the correct image to the damage box in the bottom left corner
        FindEquippedWeaponPicture(playerInventory[itemNumber].GetComponent<EquippedWeapon>().GetWeaponSprite());

        // Activate the correct checkmark
        ActivateCheckMark(itemNumber);
    }

    // Function that activates the correct checkmark
    public void ActivateCheckMark(int checkNumber)
    {
        int i = 0;
        foreach (Image check in checkImages)
        {
            if (checkNumber == i)
            {
                check.enabled = true;
            }
            else
            {
                check.enabled = false;
            }
            i += 1;
        }
    }

    // Function that deactivates all check marks (in the event that the held weapon is sold)
    public void DeactivateCheckMarks()
    {
        foreach (Image check in checkImages)
        {
            check.enabled = false;
        }
    }

    // Function that puts a picture of the equipped weapon in the damage box in the bottom left corner
    public void FindEquippedWeaponPicture(Sprite picture)
    {
        equippedWeaponImage.sprite = picture;
    }
}
