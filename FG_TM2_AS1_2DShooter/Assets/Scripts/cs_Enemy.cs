using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cs_Enemy : MonoBehaviour
{
    public static event Action<int> onEnemyKilled;

    [HideInInspector]
    public int waveIndex;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            EnemyKilled();
        }
    }

    public class Enemy
    {
        
    }

    private void EnemyKilled()
    {
        onEnemyKilled?.Invoke(waveIndex);
    }

    private void SetWaveIndex(int newWaveIndex)
    {
        waveIndex = newWaveIndex;
    }

    private void OnEnable()
    {
        cs_WaveManager.setWaveIndex += SetWaveIndex;
    }

    private void OnDisable()
    {
        cs_WaveManager.setWaveIndex -= SetWaveIndex;
    }
}
