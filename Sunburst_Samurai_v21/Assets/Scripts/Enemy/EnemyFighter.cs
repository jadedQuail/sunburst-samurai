using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyFighter : MonoBehaviour
    {
        GameObject player;

        [SerializeField] float enemyDamage = 5f;
        [SerializeField] bool hasWeapon = false;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            // Calculate the weapon damage
            CalculateDamageWithWeapon();
        }

        // Function that alters the damage if there's a sword attached
        private void CalculateDamageWithWeapon()
        {
            // No sword attached; enemyDamage stays as it is
            if (!hasWeapon) return;

            // Grab the weapon's damage and make it the enemy's new damage
            enemyDamage = GetComponentInChildren<EnemyWeapon>().GetWeaponDamage();
        }

        // Function that damages the player
        public void DamagePlayer()
        {
            player.GetComponent<Health>().TakeDamage(enemyDamage);
        }
    }
}


