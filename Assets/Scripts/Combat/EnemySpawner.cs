using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnerDimensions = new Vector2(24, 24);

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float timeBetweenSpawns = 1f;

    [SerializeField]
    private float minDistanceFromPlayer = 2f;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private void SpawnEnemy()
    {
        float randomX = Random.Range(spawnerDimensions.x, -spawnerDimensions.x);
        float randomY = Random.Range(spawnerDimensions.y, -spawnerDimensions.y);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return;
        }

        Vector2 playerPosition = player.transform.position;

        if (Vector2.Distance(playerPosition, new Vector2(randomX, randomY)) < minDistanceFromPlayer)
        {
            SpawnEnemy();
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = new Vector2(randomX, randomY);
    }

    private IEnumerator Spawner()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(timeBetweenSpawns);
        StartCoroutine(Spawner());
    }
}
