using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyCost> enemies = new List<EnemyCost>();
    public int currentWave;
    private int waveValue;

    public int waveDuration;

    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
    }

    public void GenerateWave()
    {
        waveValue = currentWave * 1;
        GenerateEnemies();

        spawnInterval = waveDuration/enemiesToSpawn.Count;

        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while(waveValue > 0)
        {
            int randEnemyCost = enemies[0].cost;

            if(waveValue - randEnemyCost >= 0) 
            {
                generatedEnemies.Add(enemies[0].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if(waveValue <= 0)
            {
                break;
            } 
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
        
    }

    void FixedUpdate()
    {
        if (spawnTimer <= 0) 
        {
            //spawn an enemy
            if (enemiesToSpawn.Count > 0)
                {
                    Instantiate(enemiesToSpawn[0], getSpawnPosition(), Quaternion.identity);
                    enemiesToSpawn.RemoveAt(0);
                    spawnTimer = spawnInterval;
                }
            else { waveTimer = 0; }
            }
        else
        { 
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    Vector3 getSpawnPosition() { 
        float _x = 0f;
        float _y = 0f;
        float _z = 4f;
        Vector3 newPos = new Vector3(_x, _y, _z);
        return newPos;
    }
}

[System.Serializable]
public class EnemyCost
{
    public int cost;
    public GameObject enemyPrefab;
}
