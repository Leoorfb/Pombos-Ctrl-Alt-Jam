using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEnemy
{
    public GameObject enemyPrefab;
    public int spawnChance;
    public int difficultyLevel;
}

[CreateAssetMenu]
public class WaveData : ScriptableObject
{
    public WaveEnemy[] waveEnemyList;
    public int waveDifficulty;
    public float spawnRate;

    List<GameObject> CurrentWave = new List<GameObject>();

    public List<GameObject> GetWave()
    {
        CurrentWave.Clear();
        int currentDifficulty = 0;
        while (currentDifficulty < waveDifficulty)
        {
            WaveEnemy waveEnemy = GetRandomWaveEnemy();
            currentDifficulty += waveEnemy.difficultyLevel;
            CurrentWave.Add(waveEnemy.enemyPrefab);
        }
        return CurrentWave;
    }

    WaveEnemy GetRandomWaveEnemy()
    {
        return waveEnemyList[UnityEngine.Random.Range(0,waveEnemyList.Length)];
    }
}
