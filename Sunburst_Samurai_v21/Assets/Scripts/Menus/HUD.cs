using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Combat;
using RPG.Control;
using RPG.Movement;

namespace RPG.Display
{
    public class HUD : MonoBehaviour
    {
        GameObject player;
        GameObject enemyManager;

        [SerializeField] Text damageNumberText;

        public GameObject pauseMenu;
        public GameObject inventoryMenu;
        public GameObject storeMenu;

        // A boolean that dictates whether or not pausing is permitted
        public bool canPause = true;

        public bool pauseOpen = false;

        private bool movingAtPause;

        float playerHealthPoints;
        float playerMaxHealth;

        private Vector3 storedDestination;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            enemyManager = GameObject.FindWithTag("EnemyManager");
        }

        private void Update()
        {
            UpdateDamageText();
            CheckForPause();
        }

        // Function that updates the damage text
        private void UpdateDamageText()
        {
            damageNumberText.text = player.GetComponent<Fighter>().GetSwingDamage().ToString();
        }

        // Method that checks to see if the player has paused the game (or unpaused it), and carries out necessary actions
        public void CheckForPause()
        {
            if (canPause)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && pauseOpen == false)
                {
                    FullOpenPause();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && pauseOpen == true)
                {
                    FullClosePause();
                }
            }
        }

        // Function that opens the pause menu and takes care of all necessary actions for the player and enemies
        public void FullOpenPause()
        {
            // Open the pause menu
            GetComponent<MenuManager>().OpenMenu(pauseMenu);
            StopWorld();

        }

        // Function that closes all menus and takes care of all necessary actions for the player and enemies
        public void FullClosePause()
        {
            // Close all menus (pause and all)
            GetComponent<MenuManager>().CloseAllMenus();
            ResumeWorld();
        }

        // Function that freezes all enemies and "pauses" the world for when menus are activated;
        public void StopWorld()
        {
            pauseOpen = true;

            // Save the destination of the player so that way when the pause menu
            // is closed, they can continue to travel to that location!
            storedDestination = player.GetComponent<Mover>().GetDestination();

            // Save whether or not the player was moving at the pause
            if (player.GetComponent<Mover>().GetIsPlayerMoving())
            {
                movingAtPause = true;
            }
            else
            {
                movingAtPause = false;
            }

            // Freeze the player, stop movement, refuse to let the player move
            player.GetComponent<Mover>().FreezeAnimation();
            player.GetComponent<Mover>().Cancel();
            player.GetComponent<PlayerController>().playerCanMove = false;

            // Freeze all the enemies (find, pause, stop, freeze)
            enemyManager.GetComponent<EnemyManager>().FindEnemies();
            enemyManager.GetComponent<EnemyManager>().EnablePause();
            enemyManager.GetComponent<EnemyManager>().StopEnemies();
            enemyManager.GetComponent<EnemyManager>().FreezeEnemyAnimation();
        }

        // Function that unfreezes all enemies and "resumes" the world for when menus are deactivated;
        public void ResumeWorld()
        {
            pauseOpen = false;

            // Unfreeze the player, allow them to move again, give them back their current destination
            player.GetComponent<Mover>().UnfreezeAnimation();
            player.GetComponent<PlayerController>().playerCanMove = true;
            player.GetComponent<Mover>().SetDestinationAndMove(storedDestination, movingAtPause);

            // Unfreeze all enemies
            enemyManager.GetComponent<EnemyManager>().DisablePause();
            enemyManager.GetComponent<EnemyManager>().UnfreezeEnemyAnimation();
        }
    }
}

