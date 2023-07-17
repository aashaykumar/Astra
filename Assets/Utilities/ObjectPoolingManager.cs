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

    public enum poolType
    {
        PlayerArrow,
        Goblin,
        EnemyArrow,
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
            Debug.Log("object set Inactive");
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

            case poolType.none:
                return null;

            default: 
                return null;
        }
    }
}

public class PoolObjectInfo
{
    public string LookupString;
    public List<GameObject> Inactiveobjects = new List<GameObject>();

}
