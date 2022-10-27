using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyWeapon : MonoBehaviour
    {
        [SerializeField] float weaponDamage;

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
    }
}

