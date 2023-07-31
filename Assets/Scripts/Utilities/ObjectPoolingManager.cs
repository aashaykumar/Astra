using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static List<PoolObjectInfo> objectPools = new List<PoolObjectInfo>();

    private GameObject objectPoolHolder;

    private static GameObject enemyPoolHolder;
    private static GameObject playerArrowPoolHolder;
    private static GameObject enemyArrowPoolHolder;
    private static GameObject bossPoolHolder;

    public enum poolType
    {
        PlayerArrow,
        Goblin,
        EnemyArrow,
        Boss,
        none
    }
    public static poolType poolingType;

    private void Awake()
    {
        setupEmpty();
    }

    private void setupEmpty()
    {
        objectPoolHolder = new GameObject("Pooled objects");

        playerArrowPoolHolder = new GameObject("Player Arrow Pool");
        playerArrowPoolHolder.transform.SetParent(objectPoolHolder.transform);

        enemyArrowPoolHolder = new GameObject("Enemy Arrow Pool");
        enemyArrowPoolHolder.transform.SetParent(objectPoolHolder.transform);

        enemyPoolHolder = new GameObject("Enemy Pool");
        enemyPoolHolder.transform.SetParent(objectPoolHolder.transform); 
        
        bossPoolHolder = new GameObject("Boss Pool");
        bossPoolHolder.transform.SetParent(objectPoolHolder.transform);
    }

    public static GameObject spawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, poolType poolType = poolType.none)
    {
        PoolObjectInfo pool = objectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PoolObjectInfo() { LookupString = objectToSpawn.name};
            objectPools.Add(pool);
        }

        GameObject spawnableObj = pool.Inactiveobjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            GameObject parentObject = setParentObject(poolType);
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }

        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.Inactiveobjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        PoolObjectInfo pool = objectPools.Find(p => p.LookupString == goName);

        if (pool != null) 
        { 
            obj.SetActive(false);
            pool.Inactiveobjects.Add(obj);
        }
    }

    private static GameObject setParentObject(poolType poolType)
    {
        switch(poolType)
        {
            case poolType.PlayerArrow:
                return playerArrowPoolHolder;
            case poolType.Goblin:
                return enemyPoolHolder;
            case poolType.EnemyArrow:
                return enemyArrowPoolHolder;
            case poolType.Boss: 
                return bossPoolHolder;
            case poolType.none:
                return null;
            default: 
                return null;
        }
    }

    public static Vector3 GetNearestEnemy(Vector3 playerPos)
    {
        Vector3 nearestPos = Vector3.zero;
        float minDistance = 100000f;
        if (enemyPoolHolder != null)
        {
            int count = enemyPoolHolder.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform childObj = enemyPoolHolder.transform.GetChild(i);
                if (childObj != null && childObj.gameObject.activeSelf && !childObj.gameObject.GetComponent<Enemy>().isDead)
                {
                    if (Vector3.Distance(childObj.transform.position, playerPos) <= minDistance)
                    {
                        minDistance = Vector3.Distance(childObj.gameObject.transform.position, playerPos);
                        nearestPos = childObj.gameObject.transform.position;
                    }
                }
            }
        }

        if(bossPoolHolder != null) 
        {
            int count = bossPoolHolder.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform childObj = bossPoolHolder.transform.GetChild(i);
                if (childObj != null && childObj.gameObject.activeSelf)
                {
                    if (Vector3.Distance(childObj.transform.position, playerPos) <= minDistance)
                    {
                        minDistance = Vector3.Distance(childObj.gameObject.transform.position, playerPos);
                        nearestPos = childObj.gameObject.transform.position;
                    }
                }
            }
            return nearestPos; 
        }
        return nearestPos;
    }
}

public class PoolObjectInfo
{
    public string LookupString;
    public List<GameObject> Inactiveobjects = new List<GameObject>();

}
