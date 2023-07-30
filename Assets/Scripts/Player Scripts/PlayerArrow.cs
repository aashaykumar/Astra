using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Arrow arrowStats;

    public int damage = 50;

    private void LateUpdate()
    {
        checkForOutsideBoundary();
    }

    private void Awake()
    {
        if (arrowStats.currentArrowEffect != Arrow.ArrowEffectType.Normal)
        {
            string vfxString = arrowStats.GetTypeString(arrowStats.currentArrowEffect);
            GameObject vfx = transform.Find(vfxString).gameObject;
            vfx.SetActive(true);
            //arrowStats.GetVFX(arrowStats.currentArrowEffect).transform.SetParent(gameObject.transform);
        }
        
    }
    private void checkForOutsideBoundary()
    {
       pos = transform.position;

        if (pos.z > 12f || pos.x > 12f || pos.x < -12f || pos.z < -12f)
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "Enemy")
        {
            //Debug.Log("Player arrow");
            other.GetComponent<Enemy>().TakeDamage(damage);
            stats.UpdateEnemyHitCount();
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            //Destroy(gameObject);
        }
    }
}
