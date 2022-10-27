using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Combat;

public class PlayerDetection : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Detects whether or not the enemy is in a valid position to be hit by a spin
        // Passes self on to player as the target for the spin attack
        if (GetRange() <= 2.5 && player.GetComponentInChildren<Animator>().GetBool("spin"))
        {
            player.GetComponent<PlayerController>().AddTargetForSpin(GetComponent<Health>());
        }
    }

    // Finds distance between the enemy and the player
    public float GetRange()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
}
