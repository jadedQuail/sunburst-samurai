using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    public GameObject botLeft;
    public GameObject botRight;
    public GameObject topLeft;
    public GameObject topRight;

    [SerializeField] int maxEnemyCount;

    int enemyCount = 0;

    int xPos;
    int zPos;

    float xMin;
    float xMax;
    float zMin;
    float zMax;

    private void Start()
    {
        xMin = botLeft.transform.position.x;
        xMax = botRight.transform.position.x;
        zMin = botLeft.transform.position.z;
        zMax = topLeft.transform.position.z;

        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < maxEnemyCount)
        {
            xPos = Random.Range((int)xMin, (int)xMax+1);
            zPos = Random.Range((int)zMin, (int)zMax+1);

            int index = Random.Range(0, enemies.Length);
            GameObject enemy = enemies[index];

            Instantiate(enemy, new Vector3(xPos, 0f, zPos), Quaternion.identity);

            yield return new WaitForSeconds(0.1f);

            enemyCount += 1;
        }
    }
}
