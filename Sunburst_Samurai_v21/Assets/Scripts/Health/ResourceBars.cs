using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class ResourceBars : MonoBehaviour
    {
        // Player reference
        GameObject player;

        // HEALTH BAR //

        // Floats for health bar length calculation
        private float maxHealth;
        private float currentHealth;
        private float healthRatio;

        // Front and back of health bar
        public Image healthForeground;
        public Image healthBackground;

        // Health texts on the health bar
        public Text currentHealthText;
        public Text maxHealthText;

        // SHIELD BAR //

        private float maxShield;
        private float currentShield;
        private float shieldRatio;

        // Front and back of shield bar
        public Image shieldForeground;
        public Image shieldBackground;

        // Shield texts on the shield bar
        public Text currentShieldText;
        public Text maxShieldText;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            // Set the length of the health bar every frame
            SetHealthBar();
            SetHealthTexts();

            SetShieldBar();
            SetShieldTexts();
        }

        // Makes the health bar reflect the % of health of the entity
        // Health bar disappears after the entity dies
        private void SetHealthBar()
        {
            (currentHealth, maxHealth) = player.GetComponent<Health>().GetHealth();

            healthRatio = (float) currentHealth / (float) maxHealth;

            healthForeground.fillAmount = healthRatio;

            if (healthRatio <= 0)
            {
                healthForeground.enabled = false;
            }
        }

        // This function is repetitive, but it works (fix it later)
        private void SetShieldBar()
        {
            (currentShield, maxShield) = player.GetComponent<Health>().GetShield();

            shieldRatio = (float)currentShield / (float)maxShield;

            shieldForeground.fillAmount = shieldRatio;

            if (shieldRatio <= 0)
            {
                shieldForeground.enabled = false;
            }

            if (shieldRatio > 0)
            {
                shieldForeground.enabled = true;
            }
        }

        // Function that updates the health text
        // Assumes that SetHealthBar is called previously, and defines the floats currentHealth and maxHealth
        private void SetHealthTexts()
        {
            currentHealthText.text = currentHealth.ToString();
            maxHealthText.text = maxHealth.ToString();
        }

        // Also repetitive, also should be fixed
        private void SetShieldTexts()
        {
            currentShieldText.text = currentShield.ToString();
            maxShieldText.text = maxShield.ToString();
        }
    }
}
