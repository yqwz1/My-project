using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int maxHP = 100;
    public int currentHP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("[PlayerStats] Duplicate instance destroyed");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        PlayerUI.Instance.UpdateHP(currentHP, maxHP);
        if(currentHP <= 0)
        {
            Die();
        }
    }

    public bool IsDead() => currentHP <= 0;
    
    public void Die()
    {
        // Play death animation
        GetComponent<Animator>().Play("Player-Death");

        // Optional: disable collider and movement
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Destroy after animation time
        Destroy(gameObject, 1.5f);
    }
}