using RPG.Combat;
using RPG.Control;
using RPG.Display;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    public bool storeMenuOpen = false;

    GameObject hud;
    GameObject player;

    bool isBuying = false;
    bool isSelling = false;

    // This is the object "on deck" to be sold
    // The player has selected it, and we are awaiting confirmation
    GameObject temporarySellItem;
    GameObject temporaryBuyItem;

    int storeSlots = 10;

    // Dialogue options, to be enabled and disabled
    [SerializeField] GameObject longDialogue;
    [SerializeField] GameObject shortDialogue;
    [SerializeField] GameObject thankYouDialogue;

    [SerializeField] Text longDialogueText;
    [SerializeField] Text shortDialogueText;
    [SerializeField] Text thankYouDialogueText;
    
    [SerializeField] List<Image> sellItemImages = new List<Image>();
    [SerializeField] List<Text> sellItemTexts = new List<Text>();
    [SerializeField] List<Text> sellItemAmounts = new List<Text>();

    List<GameObject> shopInventory = new List<GameObject>();

    [SerializeField] Sprite gray;

    [SerializeField] Image sellCheckImage;
    [SerializeField] Image buyCheckImage;

    private void Start()
    {
        hud = GameObject.FindWithTag("HUD");
        player = GameObject.FindWithTag("Player");
    }

    // Function that resets the thank you text
    // (Many different buttons will have to call this)
    private void ResetThankYouText()
    {
        thankYouDialogueText.text = "Thank you! Feel free to look around some more.";
    }

    // Function that opens the store menu and "freezes" the world
    public void FullOpenStore()
    {
        hud.GetComponent<MenuManager>().OpenMenu(hud.GetComponent<HUD>().storeMenu);
        hud.GetComponent<HUD>().StopWorld();
        hud.GetComponent<HUD>().canPause = false;
        hud.GetComponent<StoreMenu>().storeMenuOpen = true;
    }

    // Function that closes the store menu and "resumes" the world
    public void FullCloseStore()
    {
        hud.GetComponent<MenuManager>().CloseMenu(hud.GetComponent<HUD>().storeMenu);
        hud.GetComponent<HUD>().ResumeWorld();
        hud.GetComponent<HUD>().canPause = true;
        hud.GetComponent<StoreMenu>().storeMenuOpen = false;

        // Reset the dialogue boxes
        longDialogueText.text = "Hello and welcome to my store! I hope that you find what you're looking for.";
        ResetThankYouText();

        longDialogue.SetActive(true);
        shortDialogue.SetActive(false);
        thankYouDialogue.SetActive(false);

        // Reset the checkmarks
        sellCheckImage.gameObject.SetActive(false);
        buyCheckImage.gameObject.SetActive(false);

        // Reset the buying and selling booleans
        isBuying = false;
        isSelling = false;
    }

    // Function that takes the player's inventory and shows it on the store menu during a "sell" session
    // This is what is run when the "Sell" button is selected
    public void PutInventoryOnStoreMenu()
    {
        // Indicate that selling mode is activated
        isSelling = true;
        isBuying = false;

        // Activate the sell checkmark
        buyCheckImage.gameObject.SetActive(false);
        sellCheckImage.gameObject.SetActive(true);

        // Counter
        int i = 0;

        foreach (GameObject item in player.GetComponent<Inventory>().inventory)
        {
            sellItemImages[i].sprite = item.GetComponent<EquippedWeapon>().GetWeaponSprite();
            sellItemTexts[i].text = item.GetComponent<EquippedWeapon>().GetWeaponName();

            sellItemAmounts[i].text = "$" + item.GetComponent<EquippedWeapon>().GetWeaponSellAmount().ToString();
            sellItemAmounts[i].gameObject.SetActive(true);

            i++;
        }

        for (int j = i; j < storeSlots; j++)
        {
            sellItemTexts[j].text = "EMPTY";
            sellItemImages[j].sprite = gray;
            sellItemAmounts[j].gameObject.SetActive(false);
        }
    }

    // Function that shows the vendor's inventory
    // This is what is run when the "Buy" button is selected
    public void ShowVendorInventory()
    {
        // Indicate that buying mode is active
        isBuying = true;
        isSelling = false;

        // Activate the buy checkmark
        buyCheckImage.gameObject.SetActive(true);
        sellCheckImage.gameObject.SetActive(false);

        // Counter
        int i = 0;

        foreach (GameObject item in shopInventory)
        {
            sellItemImages[i].sprite = item.GetComponent<EquippedWeapon>().GetWeaponSprite();
            sellItemTexts[i].text = item.GetComponent<EquippedWeapon>().GetWeaponName();

            sellItemAmounts[i].text = "$" + item.GetComponent<EquippedWeapon>().GetWeaponBuyAmount().ToString();
            sellItemAmounts[i].gameObject.SetActive(true);

            i++;
        }

        for (int j = i; j < storeSlots; j++)
        {
            sellItemTexts[j].text = "EMPTY";
            sellItemImages[j].sprite = gray;
            sellItemAmounts[j].gameObject.SetActive(false);
        }
    }

    // Function that sells the item (first triggering the dialogue)
    public void TriggerSellDialogue(int itemNumber)
    {
        // Reset the thank you text
        ResetThankYouText();

        // Cache the player's inventory
        List<GameObject> playerInventory = player.GetComponent<Inventory>().inventory;

        // Trigger the vendor's dialogue, activate buttons
        longDialogue.SetActive(false);
        thankYouDialogue.SetActive(false);

        if (isSelling)
        {
            if (itemNumber >= playerInventory.Count) return;

            temporarySellItem = playerInventory[itemNumber];

            shortDialogueText.text = "I can buy that item for $" +
                playerInventory[itemNumber].GetComponent<EquippedWeapon>().GetWeaponSellAmount().ToString() +
                ". Would you like to sell it?";

            shortDialogue.SetActive(true);
        }

        if (isBuying)
        {
            if (itemNumber >= shopInventory.Count) return;

            temporaryBuyItem = shopInventory[itemNumber];

            shortDialogueText.text = "I can sell that item for $" +
                shopInventory[itemNumber].GetComponent<EquippedWeapon>().GetWeaponBuyAmount().ToString() +
                ". Would you like to buy it?";

            shortDialogue.SetActive(true);
        }
    }

    // Function that sells or buys the item
    public void TakeAction()
    {
        if (isSelling)
        {
            // Remove the item
            player.GetComponent<Inventory>().RemoveItem(temporarySellItem);

            // Update the store menu
            PutInventoryOnStoreMenu();

            // If the weapon we sold is the one we're holding, then remove it from
            // the player's hands
            if (temporarySellItem == player.GetComponent<Fighter>().linkedWeapon)
            {
                player.GetComponent<Fighter>().UnequipWeapon();
                hud.GetComponent<InventoryMenu>().FindEquippedWeaponPicture(gray);

                hud.GetComponent<InventoryMenu>().DeactivateCheckMarks();
            }

            // Give the item's value to the player
            player.GetComponent<Money>().AddMoney(temporarySellItem.GetComponent<EquippedWeapon>().GetWeaponSellAmount());
        }

        if (isBuying)
        {
            // Player can afford the item
            if(player.GetComponent<Money>().GetMoney() >= temporaryBuyItem.GetComponent<EquippedWeapon>().GetWeaponBuyAmount())
            {
                // Add the item to the player's inventory, remove it from store inventory
                player.GetComponent<Inventory>().AppendItem(temporaryBuyItem);
                shopInventory.Remove(temporaryBuyItem);

                // Update the store menu
                ShowVendorInventory();

                // Take the item's value from the player
                player.GetComponent<Money>().RemoveMoney(temporaryBuyItem.GetComponent<EquippedWeapon>().GetWeaponBuyAmount());
            }
            else // Player cannot afford the weapon
            {
                thankYouDialogueText.text = "I'm sorry, but you cannot afford that item.";
            }

        }

        // Trigger the thank-you dialogue
        shortDialogue.SetActive(false);
        thankYouDialogue.SetActive(true);
    }

    // Function that brings the player back when they say "No" on the buy/sell confirmation dialogue
    public void DenyAction()
    {
        // Go back to the long dialogue
        shortDialogue.SetActive(false);

        longDialogueText.text = "No problem! Feel free to look around some more.";
        longDialogue.SetActive(true);
    }

    // Function that empties the entire vendor menu
    public void EmptyVendorMenu()
    {
        for (int i = 0; i < sellItemTexts.Count; i++)
        {
            sellItemTexts[i].text = "EMPTY";
            sellItemImages[i].sprite = gray;
            sellItemAmounts[i].gameObject.SetActive(false);
        }
    }

    // Function that sets the shop's inventory (grabbed from a vendor)
    public void SetShopInventory(List<GameObject> vendorInventory)
    {
        shopInventory = vendorInventory;
    }
}
