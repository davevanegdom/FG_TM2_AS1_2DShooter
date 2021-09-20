using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cs_WaveManager : MonoBehaviour
{
    

    public int waveIndex;
    public static event Action<int> setWaveIndex;

    public List<Wave> waves;
    public static event Action<int, int, int> spawnWave;


    private void WaveStart(int currentWave)
    {
        if(waveIndex + 1 < waves.Count)
        {
            waveIndex = currentWave;
            spawnWave?.Invoke(waveIndex, waves[waveIndex].numberOfEnemies.amountOfEnemies, waves[waveIndex].targetTime.waveTime);

            SetWaveIndex(waveIndex + 1);
        }
        else
        {
            Debug.Log("Last Wave has been reached");
        }
    }

    public void SetWaveIndex(int displayIndex)
    {
        setWaveIndex?.Invoke(displayIndex);
    }

    private void WaveProgress(int currentWave)
    {

    }

    #region Wave Setup
    [System.Serializable]
    public class Wave
    {
        public int waveIndex;
        public enemyCount numberOfEnemies;
        public targetTime targetTime;
    }

    [System.Serializable]
    public class targetTime
    {
        [Range(0, 300)] public int waveTime;
    }

    [System.Serializable]
    public class enemyCount
    {
        public int amountOfEnemies;
    }
    #endregion

    #region Subscribing/Unsubscribing
    private void OnEnable()
    {
        cs_Enemy.onEnemyKilled += WaveStart;
    }

    private void OnDisable()
    {
        cs_Enemy.onEnemyKilled -= WaveStart;
    }
    #endregion 
}
