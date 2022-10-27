using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class EnemyMover : MonoBehaviour
    {
        GameObject player;
        NavMeshAgent enemyAgent;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            enemyAgent = GetComponent<NavMeshAgent>();
        }

        // Function that moves the enemy to the player
        public void MoveToPlayer()
        {
            enemyAgent.destination = player.transform.position;
            enemyAgent.isStopped = false;
        }

        // Function that stops the enemy
        public void StopEnemy()
        {
            enemyAgent.isStopped = true;
        }
    }
}

