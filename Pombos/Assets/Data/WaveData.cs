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
    public float waveDuration = 45;
    public float spawnRate;
    public int waveEnemiesTotalSpawnChance = 0;

    List<GameObject> CurrentWave = new List<GameObject>();

    private void OnEnable()
    {
        waveEnemiesTotalSpawnChance = 0;
        foreach (WaveEnemy waveEnemy in waveEnemyList)
        {
            waveEnemiesTotalSpawnChance += waveEnemy.spawnChance;
        }
    }

    public List<GameObject> GetWave()
    {
        CurrentWave.Clear();
        int currentDifficulty = 0;
        while (currentDifficulty < waveDifficulty)
        {
            WaveEnemy waveEnemy = GetRandomWaveEnemy();
            if (waveEnemy == null) continue;

            currentDifficulty += waveEnemy.difficultyLevel;
            CurrentWave.Add(waveEnemy.enemyPrefab);
        }
        return CurrentWave;
    }

    WaveEnemy GetRandomWaveEnemy()
    {
        int index = UnityEngine.Random.Range(0, waveEnemiesTotalSpawnChance+1);
        int currentIndex = 0;

        //Debug.Log("Wave " + waveDifficulty);
        //Debug.Log("Index " + index);
        foreach(WaveEnemy waveEnemy in waveEnemyList)
        {
            currentIndex += waveEnemy.spawnChance;
            if (currentIndex >= index)
            {
                //Debug.Log("Index Spawnado" + currentIndex);

                return waveEnemy;
            }
        }
        return null;
    }
}
