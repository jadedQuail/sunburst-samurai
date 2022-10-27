using RPG.Display;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    // Boolean for allowing the player to talk to the vendor (in range)
    bool canTalkToVendor = false;

    [SerializeField] List<GameObject> vendorInventory = new List<GameObject>();

    private void Update()
    {
        if (canTalkToVendor)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Open the Store Menu and give the vendor's inventory to the shop interface
                // (Clear the store so it is empty on opening)
                GameObject.FindWithTag("HUD").GetComponent<StoreMenu>().EmptyVendorMenu();

                GameObject.FindWithTag("HUD").GetComponent<StoreMenu>().FullOpenStore();
                GameObject.FindWithTag("HUD").GetComponent<StoreMenu>().SetShopInventory(vendorInventory);
            }
        }
    }

    // Function that detects whether or not the player is in range of the vendor.
    // Turns on the "F Key" image, allows the player to interact
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //GetComponentInChildren<ItemText>().TextOn();
            GetComponentInChildren<ItemText>().ImageOn();
            canTalkToVendor = true;
        }
    }

    // Function that detects if the player has exited the range of the vendor.
    // Turns off the "F Key" image, shuts off the player's ability to interact
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //GetComponentInChildren<ItemText>().TextOff();
            GetComponentInChildren<ItemText>().ImageOff();
            canTalkToVendor = false;
        }
    }

    // Function that gets the vendor's inventory
}
