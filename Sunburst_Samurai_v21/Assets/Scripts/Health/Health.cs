using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Display;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 20f;
        [SerializeField] float maxHealth = 20f;

        [SerializeField] float shieldPoints = 0f;
        [SerializeField] float maxShield = 100f;
        [SerializeField] bool isPlayer;

        [SerializeField] float shieldRechargeInterval = 1f;
        [SerializeField] float rechargeTime = 5f;

        float rechargeCountdown;

        float shieldTimeElapsed = 0f;
        float hitTimeCounter = 0f;

        int shieldDownCount = 0;

        bool isDead = false;

        GameObject hud;

        // This bool records whether or not the player is in a damaged state
        // If damaged, the player's shield cannot recharge
        bool playerDamaged;

        // Allows other classes to check for death
        public bool IsDead()
        {
            return isDead;
        }

        private void Start()
        {
            hud = GameObject.FindWithTag("HUD");
        }

        private void Update()
        {
            if (isPlayer)
            {
                RechargeShield();
                SetCountdown();
            }
        }

        // Entity takes damage
        public void TakeDamage(float damage)
        {
            if (isPlayer)
            {
                // Report that the player has been hit, reset the timer for when
                // the player can recharge shields again
                playerDamaged = true;

                hitTimeCounter = rechargeTime;

                //Debug.Log("Hit time = " + hitTimeCounter.ToString());

                // Shield points go first, then health
                shieldPoints = Mathf.Max(shieldPoints - damage, 0);

                // When shield points are gone, allow the player to be hit
                if (shieldPoints == 0)
                {
                    shieldDownCount += 1;
                    GetComponent<PlayerEffects>().StartShieldBurst();
                }

                // The shieldDownCount variable makes it so that the enemy has to
                // first take down the player's shield and then they can start
                // damaging the player's health points
                if (shieldDownCount >= 2)
                {
                    healthPoints = Mathf.Max(healthPoints - damage, 0);
                }
            }
            else
            {
                // Instantiate damage text on enemy
                if(!IsDead())
                {
                    GetComponent<AIController>().SetDamageText(damage);
                    Instantiate(GetComponent<AIController>().floatingPointsParent, GetComponent<AIController>().pointsLocation.transform.position, Quaternion.identity);
                }

                // Take health from enemies
                healthPoints = Mathf.Max(healthPoints - damage, 0);
            }

            // If out of health, then die
            if (healthPoints == 0)
            {
                // If an enemy just died, pay the player the enemy's worth
                if (!isPlayer)
                {
                    GameObject.FindWithTag("Player").GetComponent<Money>().AddMoney(GetComponent<AIController>().GetMoneyAmount());

                    GetComponent<AIController>().SetCashText(GetComponent<AIController>().GetMoneyAmount());
                    if(!IsDead())
                    {
                        StartCoroutine(InstantiateCashPopup());
                    }
                    
                }

                // Die
                Die();
            }
        }

        // Function that recharges the player's shield
        private void RechargeShield()
        {
            if (isPlayer && playerDamaged == false)
            {
                if (shieldTimeElapsed > shieldRechargeInterval)
                {
                    if (shieldPoints < maxShield)
                    {
                        shieldPoints += 5f;
                    }

                    shieldTimeElapsed = 0f;
                }

                shieldTimeElapsed += Time.deltaTime;
            }
        }

        // Function that creates a countdown for when the player's shields begin to recharge
        // If the player gets hit, the countdown gets restarted to 5 seconds
        private void SetCountdown()
        {
            // Countdown
            if (hitTimeCounter > 0)
            {
                if (!hud.GetComponent<MenuManager>().AnyMenuOpen())
                {
                        hitTimeCounter -= Time.deltaTime;
                }        
            }

            // Countdown complete; recharge shields
            if (hitTimeCounter <= 0)
            {
                TurnOffHitStatus();
            }
        }

        private void TurnOffHitStatus()
        {
            playerDamaged = false;
        }

        // Entity dies
        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponentInChildren<Animator>().SetTrigger("die");
        }

        // Allows other scripts to access healthPoints and maxHealth
        public (float, float) GetHealth()
        {
            return (healthPoints, maxHealth);
        }

        // Allows other scripts to access shieldPoints and maxShield
        public (float, float) GetShield()
        {
            return (shieldPoints, maxShield);
        }

        private IEnumerator InstantiateCashPopup()
        {
            yield return new WaitForSeconds(0.5f);

            Instantiate(GetComponent<AIController>().cashAmountParent, GetComponent<AIController>().pointsLocation.transform.position, Quaternion.identity);
        }
    }
}
