using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private PlayerStats targetstats;
    [SerializeField] private EnemyStats bossstats;
    private Animator animator;
    private AnimatorStateInfo enemyAnim;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        enemyAnim = animator.GetCurrentAnimatorStateInfo(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator != null)
        {
            enemyAnim = animator.GetCurrentAnimatorStateInfo(0);
            if (other.tag == "Player" && enemyAnim.IsName("AttackState"))
            {
                targetstats.TakeDamage(bossstats.attackDamage);
            }
        }
    }
}
