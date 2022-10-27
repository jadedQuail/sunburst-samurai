using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class DestroyEquipped : MonoBehaviour
    {
        // Destroys the held weapon, called by Fighter
        public void DestroyHeldWeapon()
        {
            Destroy(gameObject);
        }
    }
}

