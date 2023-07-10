using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyCost> enemies = new List<EnemyCost>();
    public int currentWave;
    private int waveValue;
    public int waveDuration;

    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    private int gamelevel;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    private void Awake()
    {
        gamelevel = SceneManager.GetActiveScene().buildIndex;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
            if ( gamelevel % 3 == 0 )
            {
                GenerateBossEnemy();
            }
        GenerateWave();
    }

    public void GenerateWave()
    {
        waveValue = currentWave * gamelevel;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count;

        waveTimer = 60f;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while(waveValue > 0)
        {
            int randEnemyId = UnityEngine.Random.Range(0, enemies.Count - 1);
            int randEnemyCost = enemies[randEnemyId].cost;

            if(waveValue - randEnemyCost >= 0) 
            {
               generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
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
                Debug.Log("enemiesToSpawn" + enemiesToSpawn.Count);
                ObjectPoolingManager.spawnObject(enemiesToSpawn[0], getSpawnPosition(), transform.rotation, ObjectPoolingManager.poolType.Goblin);
                //Instantiate(enemiesToSpawn[0], getSpawnPosition(), Quaternion.EulerAngles(0f, 0f, 0f));
                enemiesToSpawn[0].GetComponent<Enemy>().isDead = false;
                enemiesToSpawn.RemoveAt(0);
                spawnTimer = spawnInterval;
            }
            else
            {
                spawnTimer = spawnInterval;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
        }

        if (waveTimer <= 0)
        {
            waveTimer = 60f;
            spawnTimer = spawnInterval;
            GenerateWave();
        }
        else
        {
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    private void GenerateBossEnemy()
    {
        Instantiate(enemies[2].enemyPrefab, getSpawnPosition(), Quaternion.EulerAngles(0f, 0f, 0f));
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
