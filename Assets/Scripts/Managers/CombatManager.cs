using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
     public static CombatManager Instance;

    [Header("Combatants")]
    [SerializeField] private PlayerStats player;
    [SerializeField] private float enemyTurnDelay = 1.5f;

    [Header("Combat UI")]
    [SerializeField] private GameObject combatUI;

    private EnemyStats currentEnemy;
    private bool playerTurn = false;
    private bool combatActive = false;
   [SerializeField] private PlayerController playerController;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("[CombatManager] Duplicate instance destroyed");
            Destroy(gameObject);
        }
    }

    public void StartCombat(EnemyStats enemy)
    {
        if (combatActive) return;

        this.player = player;
        currentEnemy = enemy;
        
        
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        enemy.GetComponent<Animator>().enabled = false; 
        //enemy.GetComponent<Collider2D>().enabled = false;
        enemy.GetComponent<AnimationController>().enabled = false;
        if (enemyStats != null)
        {
            WordBuilder.Instance.SetCurrentTarget(enemyStats);
        }
        else
        {
            Debug.LogError("[CombatManager] Enemy doesn't have EnemyStats component!");
            return;
        }

        combatActive = true;
        ShowCombatUI();
        StartPlayerTurn();

    }

    public void SubmitPlayerWord(int damage)
    {
        
        if (!combatActive || !playerTurn) return;
        // Apply damage to the enemy
        currentEnemy.TakeDamage(damage);
        //EnemyUI.Instance.UpdateHP(currentEnemy.currentHP, currentEnemy.enemyData.maxHealth);
        if (currentEnemy.IsDead())
        {
            Debug.Log("[Combat] Enemy defeated!");
            EndCombat();
            return;
        }
        
        playerTurn = false;

        // Prevent overlapping enemy turns by checking if it's already invoked
        if (!IsInvoking(nameof(EnemyTurn))) 
        {
            Invoke(nameof(EnemyTurn), enemyTurnDelay);
        }
    }

    public void EnemyTurn()
    {
        if (!combatActive || player == null || currentEnemy == null) return;

        currentEnemy.Attack(player);


        if (PlayerUI.Instance != null)
        {
            PlayerUI.Instance.UpdateHP(player.currentHP, player.maxHP);
        }

        if (player.IsDead())
        {
            Debug.Log("[Combat] Player defeated!");
            EndCombat();
            return;
        }

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log("[Combat] Player's turn started.");
        playerTurn = true;
        WordBuilder.Instance.StartNewTurn();
    }

    private void EndCombat()
    {
        Debug.Log("[Combat] Combat Ended.");
        combatActive = false;
        playerTurn = false;

        if (currentEnemy != null && currentEnemy.IsDead())
        {
            currentEnemy.Die(); // Play animation and destroy
        }

        WordBuilder.Instance.SetCurrentTarget(null);
           LetterGridSpawner.Instance.ClearGrid(); // clear the old grid first
           LetterGridSpawner.Instance.SpawnGrid(); // spawn new tiles properly
        HideCombatUI();

        // Re-enable player controls
        if (playerController != null)
        {
            playerController.controlEnabled = true;
            // Reset any movement-related states if needed
            playerController.move = Vector2.zero; // You might need to make 'move' public
        }
        
        // Clear references
        currentEnemy = null;
        player = null;
        
    }

    private void ShowCombatUI()
    {
        if (combatUI != null)
        {
            combatUI.SetActive(true);
            Debug.Log("[CombatManager] Combat UI shown.");
        }
    }

    private void HideCombatUI()
    {
        if (combatUI != null)
        {
            combatUI.SetActive(false);
            Debug.Log("[CombatManager] Combat UI hidden.");
        }
    }
}