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

    private void Awake()
    {

        gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        //playerMaxXpPerLevel = gameManagerScript.GetCurrentLevel() * playerMaxXpPerLevel;

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
        if (newHealthValue <= 0 && !isDead)
        {
            animator.SetTrigger("die");
            isDead = true;
            Destroy(gameObject, 2f);
            gameManagerScript.Gamelose();
        }
        else
        {
            healthBar.value = stats.currentHealth;
            animator.SetTrigger("isHit");
        }
    }
}
