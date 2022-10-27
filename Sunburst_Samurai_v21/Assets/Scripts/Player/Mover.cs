using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Control;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        [SerializeField] float speedBoostTime;
        [SerializeField] ParticleSystem playerTrail;

        float speedBoostCounter;

        public Animator theAnimator;

        private bool spedUp = false;

        NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            speedBoostCounter = speedBoostTime;
        }

        void Update()
        {
            SetSpeedTime();

            if (spedUp)
            {
                ActivateSpeedBoost();
            }
            else
            {
                DeactivateSpeedBoost();
            }

            // Detects whether the player has stopped moving, and then changes from "Running" to "Idle" animation (and lets attack animation have priority)
            if ((Vector3.Distance(transform.position, agent.destination) <= 1f || agent.isStopped) && !GetComponentInChildren<Animator>().GetBool("attack"))
            {
                StopRunningAnimation();
            }

            if (Input.GetKeyDown(KeyCode.W) && GetComponent<PlayerController>().playerCanSpeedBoost)
            {
                // Speed boost will be activated here as well
                GameObject.FindWithTag("HUD").GetComponent<MoveBar>().StartReloadVisual("W");
                GetComponent<PlayerController>().playerCanSpeedBoost = false;
                playerTrail.Play();
                spedUp = true;
            }
        }

        // Cancels combat and changes to movement, player moves to a destination away from enemy
        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        // Moves the player to a given destination
        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;

            // Transition to running animation
            StartRunningAnimation();
        }

        // Start the "Running" animation
        public void StartRunningAnimation()
        {
            theAnimator.SetBool("isMoving", true);
        }

        // Stop the "Running" animation, switch to combat
        public void StopRunningAnimation()
        {
            theAnimator.SetBool("isMoving", false);
        }

        // Stops the NavMeshAgent
        public void Cancel()
        {
            agent.isStopped = true;
        }

        // Freeze the animation (for pauses)
        public void FreezeAnimation()
        {
            GetComponentInChildren<Animator>().enabled = false;
        }

        // Unfreeze the animation
        public void UnfreezeAnimation()
        {
            GetComponentInChildren<Animator>().enabled = true;
        }

        // Return the NavMeshAgent's destination, so that it can be restored after a pause
        public Vector3 GetDestination()
        {
            return agent.destination;
        }

        // Set the player's destination manually and move there (for pauses)
        // Player will not move if he was not moving before the pause
        public void SetDestinationAndMove(Vector3 destination, bool playerWasMoving)
        {
            if (playerWasMoving)
            {
                MoveTo(destination);
            }
        }

        // Function that says whether or not the player is currently moving
        public bool GetIsPlayerMoving()
        {
            if (agent.velocity == Vector3.zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Function that gives the player a speed boost
        public void ActivateSpeedBoost()
        {
            agent.speed = 20f;
        }

        // Function that deactivates the player's speed boost
        public void DeactivateSpeedBoost()
        {
            agent.speed = 10f;
            playerTrail.Stop();
        }

        // Function that keeps the speed boost going for a few seconds
        public void SetSpeedTime()
        {
            if (spedUp)
            {
                speedBoostCounter -= Time.deltaTime;

                if (speedBoostCounter <= 0f)
                {
                    spedUp = false;
                    speedBoostCounter = speedBoostTime;
                }
            }
        }
    }
}
