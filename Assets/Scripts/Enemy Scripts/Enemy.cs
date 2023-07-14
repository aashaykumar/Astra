using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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
    public EnemyType enemyType;
    public EnemyAttackType enemyAttackType;

    [SerializeField] public EnemyStats enemyStats;
    [SerializeField] private PlayerStats playerStats;

    GameObject gameManager;
    GameManagerScript gameManagerScript;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
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
        GameObject obj = ObjectPoolingManager.spawnObject(arrowObject, arrowPoint.position, transform.rotation, ObjectPoolingManager.poolType.EnemyArrow);
        //GameObject arrow = Instantiate(arrowObject, arrowPoint.position, transform.rotation);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * 15f, ForceMode.VelocityChange);
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0 && !isDead)
        {
            playerStats.UpdatePlayerStatsOnEnemyKill(1);
            isDead = true;
            animator.SetTrigger("die");

            //animator.applyRootMotion = false;
            StartCoroutine(waitForEnemyDie());
            if (gameManagerScript.GetCurrentLevel() % 3 == 0 && enemyType == EnemyType.Boss)
            {
                gameManagerScript.checkforLevelObjective();
            }
            else if (gameManagerScript.GetCurrentLevel() % 3 != 0 && enemyType == EnemyType.Swarm)
                gameManagerScript.checkforLevelObjective();
        }
        else
        {
            animator.SetTrigger("damage");
        }

        IEnumerator waitForEnemyDie()
        {
            yield return new WaitForSeconds(2);
            if (enemyAttackType == EnemyAttackType.Range)
                UpdateToObjectPool();
            else
                Destroy(gameObject);
        }

    }

    private void UpdateToObjectPool()
    {
        this.gameObject.SetActive(false);
        HP = 100;
        isDead = false;
        ObjectPoolingManager.ReturnObjectToPool(gameObject);
    }

    public enum EnemyAttackType
    {
        Range,
        Meelee
    }

    public enum EnemyType
    {
        Swarm,
        Boss
    }
}
