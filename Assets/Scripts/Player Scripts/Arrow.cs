using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "Enemy")
        {
            Debug.Log("arrow");
            other.GetComponent<Enemy>().TakeDamage(20);
            Destroy(gameObject);
        }
    }
}
