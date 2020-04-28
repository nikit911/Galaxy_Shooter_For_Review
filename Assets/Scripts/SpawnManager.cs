using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    // Start is called before the first frame update

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        while (_stopSpawning == false)
        {
           
            GameObject newEnemy = Instantiate(_enemy);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);

        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawnPowerup = new Vector3(Random.Range(-8f,8f), 7, 0);
            int randomPowerUP = Random.Range(0,3);
            GameObject speedPowerUp = Instantiate(_powerUps[randomPowerUP], posToSpawnPowerup, Quaternion.identity);
            //GameObject powerup = Instantiate(_tripleShotPowerup, posToSpawnPowerup, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }
}
