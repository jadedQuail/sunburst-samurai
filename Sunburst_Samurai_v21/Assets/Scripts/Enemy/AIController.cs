using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using TMPro;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float attackRange = 2f;
        [SerializeField] float bodyDespawnTime = 3f;
        [SerializeField] int moneyAmount;

        public GameObject floatingPointsParent;
        public GameObject cashAmountParent;
        public GameObject pointsLocation;

        public bool pauseEnabled = false;

        bool deathChecklistComplete = false;

        GameObject player;
        Animator enemyAnimator;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            enemyAnimator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            // Priority List:
            //   1. Check for death
            //   2. Check for chasing
            //   3. Check for stopping in attack range

            // If we're dead or the player is dead, don't move anymore
            if (GetComponent<Health>().IsDead() || player.GetComponent<Health>().IsDead())
            {
                // After all the after-death commands are run, prevent 
                if (!deathChecklistComplete)
                {
                    GetComponent<EnemyMover>().StopEnemy();
                    StopEnemyRunningAnimation();
                    StopEnemyAttackingAnimation();
                    StartCoroutine(RemoveBody());

                    deathChecklistComplete = true;
                }
                return;
            }

            // If we're in chase range of the player, move towards him; start running animation (also make sure pause is not enabled)
            if (InRange(chaseDistance) && !pauseEnabled)
            {
                GetComponent<EnemyMover>().MoveToPlayer();
                StartEnemyRunningAnimation();
                StopEnemyAttackingAnimation();
            }

            // If we're in attack range of the player, stop moving and start attacking; stop running animation
            if (InRange(attackRange))
            {
                GetComponent<EnemyMover>().StopEnemy();
                StopEnemyRunningAnimation();
                StopEnemyAttackingAnimation();

                // Begin to attack here
                StartEnemyAttackingAnimation();
            }

            // If we're not in range of the player in any shape or form, full stop
            if (NotInAnyRange())
            {
                GetComponent<EnemyMover>().StopEnemy();
                StopEnemyRunningAnimation();
                StopEnemyAttackingAnimation();
            }
        }

        // Function that checks if the enemy is within a given range of the player
        private bool InRange(float range)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < range;
        }

        // Function that starts the enemy's Running animation
        private void StartEnemyRunningAnimation()
        {
            enemyAnimator.SetBool("enemyMoving", true);
        }

        // Function that stops the enemy's Running animation
        private void StopEnemyRunningAnimation()
        {
            enemyAnimator.SetBool("enemyMoving", false);
        }

        // Function that starts the enemy's attacking animation
        private void StartEnemyAttackingAnimation()
        {
            enemyAnimator.SetBool("enemyAttacking", true);
        }

        // Function that stops the enemy's attacking animation
        private void StopEnemyAttackingAnimation()
        {
            enemyAnimator.SetBool("enemyAttacking", false);
        }

        // Function that returns true if the enemy is not within any applicable range of the player
        private bool NotInAnyRange()
        {
            return (!InRange(chaseDistance) && !InRange(attackRange));
        }

        // Function that checks that the player is still within attack range when hitting
        public bool StillInAttackRange()
        {
            return InRange(attackRange);
        }

        // Function that returns the enemy's value in money
        public int GetMoneyAmount()
        {
            return moneyAmount;
        }

        // Function that lets other scripts change the popup damage text
        public void SetDamageText(float damage)
        {
            floatingPointsParent.GetComponentInChildren<TextMeshPro>().SetText(damage.ToString());
        }

        // Function that lets other scripts change the cash popup text
        public void SetCashText(int cash)
        {
            cashAmountParent.GetComponentInChildren<TextMeshPro>().SetText("$" + cash.ToString());
        }

        // Function that gets rid of the enemy's body after a few seconds if they died
        IEnumerator RemoveBody()
        {
            // Wait for a few seconds
            yield return new WaitForSeconds(bodyDespawnTime);

            // Remove the enemy from the list of enemies
            GameObject.FindWithTag("EnemyManager").GetComponent<EnemyManager>().RemoveEnemy(transform.parent.gameObject);

            // Then destroy the parent object (and the child with it)
            Destroy(transform.parent.gameObject);
        }
    }
}

