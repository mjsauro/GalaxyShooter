using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private float sideBoundary = 8f;
    [SerializeField] private float upperBoundary = 7f;
    [SerializeField] private GameObject[] powerUps;
    private bool StopSpawning { get; set; } = false;
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        while (StopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);
            Vector3 positionToSpawn = new Vector3(Random.Range(-sideBoundary, sideBoundary), upperBoundary, 0);
            int powerUpIndex = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[powerUpIndex], positionToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (StopSpawning == false)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-sideBoundary, sideBoundary), upperBoundary, 0);
            var newEnemy = Instantiate(enemyPrefab, positionToSpawn, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void OnPlayerDeath()
    {
        StopSpawning = true;
    }
}
