using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float swingRange = 1.5f;
        [SerializeField] float swingDamage = 1f;

        [SerializeField] public float spinDamage = 2f;

        [SerializeField] Transform handTransform = null;
    
        public GameObject currentWeapon = null;
        public GameObject linkedWeapon = null;

        Health target;

        // Another object to make sure that the right target is being attacked
        Health targetAtAttackStart;

        private void Update()
        {
            // Check for mid-animation changes
            TargetSwitchCheck();

            // If there is no target, we're not in combat, so we shouldn't do any of this.
            if (target == null) return;

            // If the target's dead, we're done
            if (target.IsDead()) return;

            // If there is a target and we're not in range, move towards the target
            if (target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else // Otherwise, stop moving and start attacking
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        // Triggers an attacking animation
        private void AttackBehavior()
        {
            // Look at the enemy before attacking (no rotation on y-axis)
            Vector3 targetPostition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(targetPostition);

            // This will trigger the Hit() event, which does damage
            GetComponentInChildren<Animator>().SetBool("attack", true);
            targetAtAttackStart = target;
            
        }

        // Animation Event, called from child script
        public void HitCall()
        {
            //If target is switched, alternate cannot take damage
            if (targetAtAttackStart == target)
            {
                targetAtAttackStart.TakeDamage(swingDamage);
            }
        }

        // Checks whether the player is within a designated range of the enemy or not
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < swingRange;
        }

        // Tests to see if there is a target and they are alive; if so, then we can attack
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        // Begins the attack by making it so the target is not null
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        // Player exits combat (by making it so there is no target)
        public void Cancel()
        {
            GetComponentInChildren<Animator>().SetBool("attack", false);
            target = null;
        }

        // Checks to see if the player has switched targets suddenly and cancels
        // the attacking animation
        public void TargetSwitchCheck()
        {
            if (targetAtAttackStart != target)
            {
                GetComponentInChildren<Animator>().SetBool("attack", false);
            }
        }

        // Method for equipping a weapon to the player's hand
        public void EquipWeapon(GameObject weaponPrefab, float weaponDamage)
        {
            // Destroy the currently equipped weapon if there is one.
            if (GetComponentInChildren<DestroyEquipped>() != null)
            {
                GetComponentInChildren<DestroyEquipped>().DestroyHeldWeapon();
            }

            // Set current weapon and instantiate (and grab the particle system)
            currentWeapon = Instantiate(weaponPrefab, handTransform);
            linkedWeapon = weaponPrefab;

            // Set the new damage
            swingDamage = weaponDamage;
            spinDamage = weaponDamage * 1.2f;
        }

        // Method for unequipping a weapon (selling it, etc.)
        public void UnequipWeapon()
        {
            GetComponentInChildren<DestroyEquipped>().DestroyHeldWeapon();

            // Reset the player's damage
            swingDamage = 1f;
            spinDamage = 1f;
        }

        // Function for getting swing damage
        public float GetSwingDamage()
        {
            return swingDamage;
        }
    }
}
