using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;
    public bool isDead = false;
    public GameObject arrowObject;
    public Transform arrowPoint;
    public GameObject resultScreen;
   /* public GameObject bowObject;
    public Transform bowPoint;
*/
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0 && !isDead)
        {
            animator.SetTrigger("die");
            isDead = true;
            Destroy(gameObject, 2f);
            SceneManager.LoadScene("ScreenTransitions");
            resultScreen.SetActive(true);

        }
        else
        {
            Debug.Log("arrow damage" + HP);
            animator.SetTrigger("damage");
        }
    }
}
