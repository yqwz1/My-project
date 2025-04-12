using System.Collections.Generic;
using System.Linq; // Import System.Linq for LINQ methods like Select
using UnityEngine;

public class WordBuilder : MonoBehaviour
{
    public static WordBuilder Instance;

    public Transform wordArea;

    private List<LetterTile> selectedTiles = new();
   [SerializeField] private EnemyStats currentTarget;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCurrentTarget(EnemyStats enemy)
    {
        currentTarget = enemy;
    }

    public void AddTile(LetterTile tile)
    {
        if (!selectedTiles.Contains(tile))
        {
            selectedTiles.Add(tile);
            tile.MoveTo(wordArea);
            UpdateUI();
        }
    }

    public void RemoveTile(LetterTile tile)
    {
        if (selectedTiles.Contains(tile))
        {
            selectedTiles.Remove(tile);
            tile.ResetTile();
            UpdateUI();
        }
    }

    public void SubmitWord()
    {
        string word = GetCurrentWord();

        if (!IsValidWord(word))
        {
            Debug.Log("[WordBuilder] Invalid word.");
            return;
        }

        if (currentTarget == null)
        {
            Debug.LogWarning("[WordBuilder] No target enemy assigned!");
            return;
        }

        int damage = CalculateScore();
        CombatManager.Instance.SubmitPlayerWord(damage);

        // Destroy selected letters (used in this turn)
        foreach (var tile in selectedTiles)
        {
            Destroy(tile.gameObject);
        }

        selectedTiles.Clear();
        UpdateUI();

        // Replace entire letter grid
        LetterGridSpawner.Instance.ClearGrid();
        LetterGridSpawner.Instance.SpawnGrid();

        // Proceed to enemy turn
        
    }

    public void StartNewTurn()
    {
        ResetWord();
    }

    private void ResetWord()
    {
        foreach (var tile in new List<LetterTile>(selectedTiles))
        {
            tile.ResetTile();
        }

        selectedTiles.Clear();
        UpdateUI();
    }

    public string GetCurrentWord() =>
        string.Join("", selectedTiles.Select(t => t.letterTileData.letter));

    private int CalculateScore()
    {
        int baseScore = 0;
        foreach (var tile in selectedTiles)
        {
            baseScore += tile.letterTileData.value;
        }

        int wordLength = selectedTiles.Count;

        // 🔥 Bonus: Add multiplier for long words
        float lengthMultiplier = 1f;

        if (wordLength >= 4 && wordLength < 6) lengthMultiplier = 1.25f;
        else if (wordLength >= 6 && wordLength < 8) lengthMultiplier = 1.5f;
        else if (wordLength >= 8) lengthMultiplier = 2f;

        int finalScore = Mathf.RoundToInt(baseScore * lengthMultiplier);

        Debug.Log($"[WordBuilder] Word Length: {wordLength}, Base Score: {baseScore}, Final Score: {finalScore}");

        return finalScore;
    }

    private bool IsValidWord(string word)
    {
        return DictionaryManager.Instance != null && DictionaryManager.Instance.IsValidWord(word);
    }

    private void UpdateUI()
    {
        string word = GetCurrentWord();
        int score = CalculateScore();
        bool isValid = IsValidWord(word);

        if (WordUI.Instance != null)
        {
            WordUI.Instance.ShowSubmitButton(isValid);
            WordUI.Instance.UpdateWord(word, score, isValid);
        }
    }
}