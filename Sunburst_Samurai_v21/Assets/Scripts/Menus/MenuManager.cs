using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Display
{
    public class MenuManager : MonoBehaviour
    {
        HUD hud;

        // Start is called before the first frame update
        void Start()
        {
            hud = GetComponent<HUD>();
        }

        // Function for opening up this menu
        public void OpenMenu(GameObject theMenu)
        {
            theMenu.SetActive(true);
        }

        // Function for closing this menu
        public void CloseMenu(GameObject theMenu)
        {
            theMenu.SetActive(false);
        }

        // Function for closing every menu
        public void CloseAllMenus()
        {
            CloseMenu(hud.pauseMenu);
            CloseMenu(hud.inventoryMenu);
        }

        // Function that reports if any menu is open
        public bool AnyMenuOpen()
        {
            if (hud.GetComponent<HUD>().pauseOpen || hud.GetComponent<StoreMenu>().storeMenuOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


