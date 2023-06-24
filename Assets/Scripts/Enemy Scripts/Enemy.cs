using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class Enemy : MonoBehaviour
{
    private int HP = 100;

    Quaternion rotation;
    public Slider healthBar;
    public GameObject healthBarCanvas;
    public Animator animator;
    public bool isDead = false;
    public GameObject arrowObject;
    public Transform arrowPoint;

    [SerializeField] private PlayerStats stats;

    GameObject gameManager;
    GameManagerScript gameManagerScript;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        rotation = healthBarCanvas.transform.rotation;
    }

    private void Update()
    {
        healthBar.value = HP;
    }

    private void LateUpdate()
    {
        healthBarCanvas.transform.rotation = rotation;
    }

    public void FireArrow()
    {
        GameObject arrow = Instantiate(arrowObject, arrowPoint.position, transform.rotation);
        arrow.GetComponent<Rigidbody>().AddForce(transform.forward * 25f, ForceMode.Impulse);
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0 && !isDead)
        {
            stats.UpdatePlayerStatsOnEnemyKill(1);
            animator.SetTrigger("die");
            isDead = true;
            gameManagerScript = gameManager.GetComponent<GameManagerScript>();
            gameManagerScript.checkforLevelObjective();
            Destroy(gameObject, 2f);

        }
        else
        {   
            animator.SetTrigger("damage");
        }

    }
}
