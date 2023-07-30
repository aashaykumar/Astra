using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private PlayerStats targetstats;
    [SerializeField] private EnemyStats bossstats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetstats.TakeDamage(bossstats.attackDamage);
        }
    }
}
