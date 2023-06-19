using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public Slider healthBar;
    Animator animator;
    public bool isDead = false;

    GameObject gameManager;
    GameManagerScript gameManagerScript;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        healthBar.value = HP;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0 && !isDead)
        {
            animator.SetTrigger("die");
            isDead = true;
            Destroy(gameObject, 2f);
            gameManagerScript = gameManager.GetComponent<GameManagerScript>();
            gameManagerScript.Gamelose(); 
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }
}
