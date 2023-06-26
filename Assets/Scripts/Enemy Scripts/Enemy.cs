using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
    Quaternion rotation;
    public Slider healthBar;
    public GameObject healthBarCanvas;
    public Animator animator;
    public bool isDead = false;
    public GameObject arrowObject;
    public Transform arrowPoint;

    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private PlayerStats playerStats;

    GameObject gameManager;
    GameManagerScript gameManagerScript;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        rotation = healthBarCanvas.transform.rotation;
    }
    private void Start()
    {
        HP = enemyStats.currentHealth;
        healthBar.value = HP;
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
            playerStats.UpdatePlayerStatsOnEnemyKill(1);
            animator.SetTrigger("die");
            isDead = true;
            StartCoroutine(waitForEnemyDie());
            gameManagerScript = gameManager.GetComponent<GameManagerScript>();
            gameManagerScript.checkforLevelObjective();
            gameObject.SetActive(false);
            HP = 100;
            ObjectPoolingManager.ReturnObjectToPool(gameObject);
            //Destroy(gameObject, 2f);

        }
        else
        {   
            animator.SetTrigger("damage");
        }

        IEnumerator waitForEnemyDie()
        {
            yield return new WaitForSeconds(2);
        }

    }
}
