using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    Vector3 pos;

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
        if (other.tag == "Enemy")
        {
            Debug.Log("Player arrow");
            other.GetComponent<Enemy>().TakeDamage(120);
            Destroy(gameObject);
        }
    }
}
