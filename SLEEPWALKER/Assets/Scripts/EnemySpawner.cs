using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 15f;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SpawnEnemies());

    }

    private IEnumerator SpawnEnemies()
    {

        while (true)
        {
            if (player != null)
            {
                // Spawn an enemy
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                // Wait for the specified spawn interval
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                yield break;
            }
        }
    }
}
