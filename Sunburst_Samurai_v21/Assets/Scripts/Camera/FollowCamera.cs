using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;

        // Makes it so the camera follows the target (the player)
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
