using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public float maxHealth;
    public Image healthBar;
    public float health;

    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health/maxHealth, 0, 1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) // Check if the object is a bullet
        {
            TakeDamage();
            Destroy(collision.gameObject); // Destroy the bullet on impact
        }
    }

    void TakeDamage()
    {
        health--; // Reduce life by 1
        Debug.Log("Player hit! Lives remaining: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject); // Destroy player (You can replace this with a game over screen)
    }
}
