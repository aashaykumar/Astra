using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] private PlayerStats targetstats;

    private void LateUpdate()
    {
        checkForOutsideBoundary();
    }

    private void checkForOutsideBoundary()
    {
        pos = transform.position;
        if (pos.z > 12f || pos.x > 12f || pos.x < -12f || pos.z < -12f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "Player")
        {
            Debug.Log("Enemy arrow");
            targetstats.TakeDamage(20);
            Destroy(gameObject);
        }
        else if(other.tag == "SafeArea")
        {
            Destroy(gameObject);
        }
    }
}
