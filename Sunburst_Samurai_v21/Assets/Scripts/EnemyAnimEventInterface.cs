using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Control;

namespace RPG.Core
{
    public class EnemyAnimEventInterface : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          //
        //    The scripts that control the enemy are attached to a parent object,   //
        //    whereas the animator is attached to a child object.                   //
        //    So, this script is used to register animation events and call         //
        //    commands scripts in the parent object.                                //
        //                                                                          //
        //////////////////////////////////////////////////////////////////////////////

        // Function that is called when the enemy hits the player with a swing
        void Hit()
        {
            // Only damage the player if the player was still within a 2 foot range (*** Play with this number later! ***)
            if (GetComponentInParent<AIController>().StillInAttackRange())
            {
                GetComponentInParent<EnemyFighter>().DamagePlayer();
            }
        }

        void SwingStart()
        {
            // Do nothing, for now
        }

        void SwingFinish()
        {
            // Do nothing, for now
        }
    }
}

