using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;

public class PlayerBattleTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasTriggered) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("[Battle] Enemy touched! Starting combat...");

            hasTriggered = true;

            EnemyStats enemy = collision.gameObject.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                CombatManager.Instance.StartCombat(enemy);
            }
            else
            {
                Debug.LogError("[Battle] EnemyStats not found on enemy!");
            }

            // Optional: Disable movement or control
            GetComponent<PlayerController>().enabled = false;
        }
    }
}
