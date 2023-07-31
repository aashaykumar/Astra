using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    public Slider healthBar;
    public bool isDead = false;

    Animator animator;
    GameObject gameManager;
    GameManagerScript gameManagerScript;

    public void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        healthBar.value = stats.currentHealth;
    }

    void OnEnable()
    {
        stats.HealthChanged += CheckIfAlive;
    }
    void OnDisable()
    {
        stats.HealthChanged -= CheckIfAlive;
    }

    private void CheckIfAlive(int newHealthValue)
    {
       // Debug.Log("IN CheckIFAlive" + stats.currentHealth);
        if (newHealthValue <= 0 && !isDead)
        {
            animator.SetTrigger("isDead");
            isDead = true;
            gameManagerScript.Gamelose();
            Destroy(gameObject, 2f);
        }
        else
        {
            //Debug.Log("HealthAfterDamage"+ stats.currentHealth);
            healthBar.value = stats.currentHealth;
            //animator.SetTrigger("isHit");
        }
    }
}
