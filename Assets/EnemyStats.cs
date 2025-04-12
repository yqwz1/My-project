using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    
    public int currentHP;
    
    public EnemyData enemyData;

    private void Start()
    {
        currentHP = enemyData.maxHealth;
        EnemyUI.Instance.UpdateHP(currentHP, enemyData.maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        Debug.Log($"{name} took {damage} damage. Remaining HP: {currentHP}");
        EnemyUI.Instance.UpdateHP(currentHP, enemyData.maxHealth);
        
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    
    public void Attack(PlayerStats player)
    {
        player.TakeDamage(enemyData.Damage);
        Debug.Log($"[Enemy] Attacked player for {enemyData.Damage}.");
    }
    public bool IsDead() => currentHP <= 0;

    public void Die()
    {

        // Optional: disable collider and movement
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Destroy after animation time
        Destroy(gameObject, 1.5f);
    }
}
