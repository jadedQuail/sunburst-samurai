using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Combat;

namespace RPG.Core
{
    public class AnimEventInterface : MonoBehaviour
    {

        //////////////////////////////////////////////////////////////////////////////
        //                                                                          //
        //    The scripts that control the enemy are attached to a parent object,   //
        //    whereas the animator is attached to a child object.                   //
        //    So, this script is used to register animation events and call         //
        //    commands scripts in the parent object.                                //
        //                                                                          //
        //////////////////////////////////////////////////////////////////////////////

        void Hit()
        {
            GetComponentInParent<Fighter>().HitCall();
        }

        void SwingStart()
        {
            GetComponentInChildren<EquippedWeapon>().StartWeaponTrail();
        }

        void SwingFinish()
        {
            GetComponentInChildren<EquippedWeapon>().StopWeaponTrail();
            GetComponentInParent<Fighter>().Cancel();
        }

        void SpinFinish()
        {
            GetComponent<Animator>().SetBool("spin", false);
            // Turn off the swinging animation
            GetComponentInChildren<EquippedWeapon>().StopWeaponTrail();
        }

        void FirstSpin()
        {
            foreach (Health spinTarget in GetComponentInParent<PlayerController>().spinTargets)
            {
                spinTarget.TakeDamage(GetComponentInParent<Fighter>().spinDamage);
            }
        }

        void SecondSpin()
        {
            foreach (Health spinTarget in GetComponentInParent<PlayerController>().spinTargets)
            {
                spinTarget.TakeDamage(GetComponentInParent<Fighter>().spinDamage);
            }
        }
    }
}
