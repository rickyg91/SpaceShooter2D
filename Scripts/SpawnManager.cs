using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;

    private bool _stopSpawning = false;
    private int _enemyTotal = 0;
    
    public void StartSpawning(float enemySpawn, float powerSpawn)
    {
        StartCoroutine(SpawnEnemyRoutine(enemySpawn));
        StartCoroutine(SpawnPowerUpRoutine(powerSpawn));
    }
    
    IEnumerator SpawnEnemyRoutine(float spawnTime)
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            if (_enemyTotal < 25) //Maximum enemies allowed
            {
                _enemyTotal += 1;

                var newPosition = new Vector3(Random.Range(-8f, 8f), 6.8f, 0);

                GameObject newEnemy = Instantiate(_enemyPrefab, newPosition, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

                yield return new WaitForSeconds(spawnTime);
            }
            else
            {
                yield return new WaitForSeconds(3.0f);
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine(float spawnTime)
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            var newPosition = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            int randomPower = Random.Range(0, 3);

            Instantiate(_powerUps[randomPower], newPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnTime);

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void OnEnemyDeath()
    {
        _enemyTotal -= 1;
    }
}
