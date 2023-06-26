using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] private PlayerStats stats;
    public int damage = 50;

    private void LateUpdate()
    {
        checkForOutsideBoundary();
    }

    private void Awake()
    {
        
    }
    private void checkForOutsideBoundary()
    {
       pos = transform.position;

        if (pos.z > 12f || pos.x > 12f || pos.x < -12f || pos.z < -12f)
        {

            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(transform.GetComponent<Rigidbody>());
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (other.tag == "Enemy")
        {
            Debug.Log("Player arrow");
            other.GetComponent<Enemy>().TakeDamage(damage);
            stats.UpdateEnemyHitCount();
            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            //Destroy(gameObject);
        }
    }
}
