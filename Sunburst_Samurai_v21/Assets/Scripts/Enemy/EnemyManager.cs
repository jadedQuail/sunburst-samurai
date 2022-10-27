using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Control;
using RPG.Combat;

public class EnemyManager : MonoBehaviour
{
    // DESCRIPTION:
    // A script for all the functions that are necessary
    // for managing enemies en masse

    private List<GameObject> enemies = new List<GameObject>();

    private void Update()
    {
        FindEnemies();
        //DescribeEnemies();
    }

    // Function that finds all the objects tagged "Enemy" in the scene
    public void FindEnemies()
    {
        // Add new enemies
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    // Function that describes the enemies found in the scene (for debugging purposes)
    public void DescribeEnemies()
    {
        Debug.Log(enemies.Count);
    }

    // Function that stops the movement of all enemies
    // (Make sure the enemies are found before calling this)
    public void StopEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<EnemyMover>().StopEnemy();
        }
    }

    // Function that enables the pause switch on every enemy
    public void EnablePause()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<AIController>().pauseEnabled = true;
        }
    }

    // Function that disables the pause switch on every enemy
    public void DisablePause()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<AIController>().pauseEnabled = false;
        }
    }

    // Function that freezes the animation on every enemy
    public void FreezeEnemyAnimation()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<Animator>().enabled = false;
        }
    }

    // Function that unfreezes the animation on every enemy
    public void UnfreezeEnemyAnimation()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<Animator>().enabled = true;
        }
    }

    // Function that removes a dead enemy (called by an enemy when it dies)
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
