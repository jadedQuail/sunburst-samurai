using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        public List<Health> spinTargets = new List<Health>();
        public bool playerCanMove = true;
        public bool playerCanSpin = true;
        public bool playerCanSpeedBoost = true;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetComponentInChildren<Animator>().SetBool("isShooting", true);
                Debug.Log("Zing!");
            }

            // Get rid of dead spin targets.
            RemoveDeadSpinTargets();

            // If the player is dead, freeze his movement completely
            if (GetComponent<Health>().IsDead()) return;

            // No movement, fighting, or spinning can take place if the player cannot move
            if (!playerCanMove) return;

            // If interacting with combat succeeds, then we don't move on to movement
            if (playerCanSpin)
            {
                if (InteractWithSpin()) return;
            }
            
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        // See if we're engaged with an enemy; returns true if we are, false otherwise
        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) { continue; } // We only execute the remaining code if we can attack the target

                // This is a regular attack
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;

            }
            return false;
        }

        // See if we've chosen to move to a spot; returns true if we are, false otherwise
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithSpin()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Spinning!");

                // Cancel fighting activities
                GetComponent<Fighter>().Cancel();

                // Spin!
                GetComponentInChildren<Animator>().SetBool("spin", true);

                // Start the effect on the weapon trail
                if (GetComponentInChildren<EquippedWeapon>() != null)
                {
                    GetComponentInChildren<EquippedWeapon>().StartWeaponTrail();
                }

                // Start the reload visual
                GameObject.FindWithTag("HUD").GetComponent<MoveBar>().StartReloadVisual("Q");

                return true;
            }
            return false;
        }

        // Returns the coordinates of the location clicked with the mouse
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public void AddTargetForSpin(Health combatTarget)
        {
            if (!spinTargets.Contains(combatTarget))
            {
                spinTargets.Add(combatTarget);
            }
        }

        // Removes targets that are no longer valid (dead, far-away)
        private void RemoveDeadSpinTargets()
        {
            // For-loop instead of for-each b/c of a Collections error
            for (int i = 0; i < spinTargets.Count; i++)
            {
                if (spinTargets[i].IsDead() || spinTargets[i].GetComponent<PlayerDetection>().GetRange() > 2.5f)
                {
                    spinTargets.Remove(spinTargets[i]);
                }
            }
        }
    }
}
