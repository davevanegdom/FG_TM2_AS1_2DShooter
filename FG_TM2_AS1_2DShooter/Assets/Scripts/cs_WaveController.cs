using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cs_WaveController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public LayerMask spawnLayer;
    private int waveIndex;
    private int enemiesToSpawn;
    private int waveTime;
    [SerializeField] private float timeInterval;

    void SpawnEnemies(int newWaveIndex, int enemiesCount, int newWaveTime)
    {
        waveIndex = newWaveIndex;
        enemiesToSpawn = enemiesCount;
        waveTime = newWaveTime;
        timeInterval = waveTime / enemiesToSpawn;
        spawnEnemy();
        StartCoroutine(WaveTimer(waveTime));

        Debug.Log("Wave " + waveIndex + ": spawn " + enemiesCount + " enemies over the course of the next " + newWaveTime + " seconds");
    }

    void spawnEnemy()
    {
        //Spawn player at the given point
        Instantiate(enemyPrefab, enemySpawnPosition(), Quaternion.identity);
        timeInterval = waveTime / enemiesToSpawn;
        StartCoroutine(spawnTimer(timeInterval));
    }

    //Get a point on an edge collider
    Vector2 enemySpawnPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(0, -0.2f), new Vector2((UnityEngine.Random.Range(-2f, 2f)), (UnityEngine.Random.Range(-2f, 2f))), 100f, spawnLayer);

        if(hit.collider == null)
        {
            Debug.Log("No collider was found");
        }
        return hit.point;
    }

    public IEnumerator spawnTimer(float timeInterval)
    {
        float time = timeInterval;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        //spawn Enemy
        spawnEnemy();
    }

    public IEnumerator WaveTimer(float waveTimer)
    {
        float time = waveTimer;

        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Wave Ended");
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        cs_WaveManager.spawnWave += SpawnEnemies;
    }

    private void OnDisable()
    {
        cs_WaveManager.spawnWave -= SpawnEnemies;
    }

}
