using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] private PlayerStats targetstats;
    [SerializeField] private EnemyStats  enemystats;
    int damage;

    private void Awake()
    {
        damage = enemystats.attackDamage;
    }
    private void LateUpdate()
    {
        checkForOutsideBoundary();
    }

    private void checkForOutsideBoundary()
    {
        pos = transform.position;
        if (pos.z > 12f || pos.x > 12f || pos.x < -12f || pos.z < -12f || pos.y < -12f)
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ResetArrow();
            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && targetstats.currentHealth > 0)
        {
            //Debug.Log("Enemy arrow" + targetstats.name);
            targetstats.TakeDamage(damage);
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ResetArrow();
            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            
            //Destroy(gameObject);
        }
        /*if(other.tag == "SafeArea")
        {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            //Destroy(gameObject);
        }*/
    }

    void ResetArrow()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
