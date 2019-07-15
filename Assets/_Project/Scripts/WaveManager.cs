using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int CurrentWave = 0;
    public List<WaveData> Waves;
    public Action AllWavesFinished;
    private EnemyManager _enemyMan;

    // Start is called before the first frame update
    void Awake()
    {
        _enemyMan = FindObjectOfType<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnNextWave();
        }
    }


    public void SpawnNextWave()
    {
        _enemyMan.AllEnemiesDefeated = FinishWave;

        foreach (var item in Waves[CurrentWave].Enemies)
        {
            _enemyMan.SpawnEnemies(1, item);
        } 
        

        CurrentWave++;
    }

    public void FinishWave()
    {
        _enemyMan.AllEnemiesDefeated = null;
        Debug.Log("Wave " + CurrentWave + " finished!");

        if (Waves.Count <= CurrentWave)
            SpawnNextWave();
        else
        {
            AllWavesFinished?.Invoke();
        }
    }
}

[System.Serializable]
public class WaveData
{

    public List<GameObject> Enemies;

}
