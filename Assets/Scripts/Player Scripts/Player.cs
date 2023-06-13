using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;
    Animator animator;
    public bool isDead = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0 && !isDead)
        {
            animator.SetTrigger("die");
            isDead = true;
            Destroy(gameObject, 2f);
        }
        else
        {
            Debug.Log("arrow damage" + HP);
            animator.SetTrigger("damage");
        }
    }
}
