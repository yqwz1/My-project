using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintSystem : MonoBehaviour
{
    public static HintSystem Instance;

    private List<LetterTile> currentTiles = new();
    private LetterTile currentHintTile = null;
    private List<string> possibleWords = new(); // Store all possible words that can be formed

    [SerializeField] private Color hintColor = Color.yellow;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Button hintButton;

    [Header("Hint Settings")]
    [SerializeField] private int totalHints = 3;
    private int currentHints;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("[HintSystem] Duplicate instance destroyed");
            Destroy(gameObject);
        }

        currentHints = totalHints;
        UpdateHintUI();
    }

    public void ResetHints(List<LetterTile> newTiles)
    {
        currentTiles = newTiles;
        ClearCurrentHint();
        // Find all possible words when new tiles are set
        FindAllPossibleWords();
        Debug.Log("[HintSystem] Hints reset with new grid");
    }

    private void FindAllPossibleWords()
    {
        possibleWords.Clear();
        
        // Get all available letters
        string availableLetters = string.Join("", currentTiles.Select(t => t.letterTileData.letter));
        Debug.Log($"Available letters: {availableLetters}");

        // Create a dictionary to count available letters
        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach (char c in availableLetters.ToUpper())
        {
            if (letterCount.ContainsKey(c))
                letterCount[c]++;
            else
                letterCount[c] = 1;
        }

        // Check each word in the dictionary
        foreach (string word in DictionaryManager.Instance.GetAllWords())
        {
            if (CanFormWord(word, new Dictionary<char, int>(letterCount)))
            {
                possibleWords.Add(word.ToUpper());
            }
        }

        Debug.Log($"Found {possibleWords.Count} possible words: {string.Join(", ", possibleWords)}");
    }

    private bool CanFormWord(string word, Dictionary<char, int> availableLetters)
    {
        foreach (char c in word.ToUpper())
        {
            if (!availableLetters.ContainsKey(c) || availableLetters[c] == 0)
                return false;
            availableLetters[c]--;
        }
        return true;
    }

    public void ShowHint()
    {
        Debug.Log("[HintSystem] ShowHint called");

        if (currentHints <= 0)
        {
            Debug.Log("[HintSystem] No more hints remaining!");
            return;
        }

        ClearCurrentHint();

        string currentInput = WordBuilder.Instance.GetCurrentWord();
        Debug.Log($"[HintSystem] Current input: {currentInput}");

        // Find valid words that start with the current input
        var validWords = possibleWords
            .Where(w => w.StartsWith(currentInput, System.StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (validWords.Count == 0)
        {
            Debug.Log("[HintSystem] No valid words found starting with current input.");
            return;
        }

        // Choose the shortest valid word as the hint
        string hintWord = validWords.OrderBy(w => w.Length).First();
        
        if (currentInput.Length < hintWord.Length)
        {
            char nextLetter = hintWord[currentInput.Length];
            Debug.Log($"[HintSystem] Next letter in hint word '{hintWord}': {nextLetter}");

            // Find an unused tile with the next letter
            currentHintTile = currentTiles.FirstOrDefault(t => 
                !t.isSelected && 
                char.ToUpperInvariant(t.letterTileData.letter[0]) == nextLetter);

            if (currentHintTile != null)
            {
                currentHintTile.SetColor(hintColor);
                Debug.Log($"[HintSystem] Highlighted tile: {currentHintTile.letterTileData.letter}");
                
                LetterTileUI tileUI = currentHintTile.GetComponent<LetterTileUI>();
                if (tileUI != null)
                {
                    tileUI.OnClick();
                }
                else
                {
                    Debug.LogError("[HintSystem] LetterTileUI not found on tile!");
                }

                currentHints--;
                UpdateHintUI();
            }
            else
            {
                Debug.Log($"[HintSystem] No available tile for letter: {nextLetter}");
            }
        }
        else
        {
            Debug.Log("[HintSystem] Word is complete.");
        }
    }

    private void ClearCurrentHint()
    {
        if (currentHintTile != null)
        {
            currentHintTile.SetColor(Color.white);
            currentHintTile = null;
        }
    }

    private void UpdateHintUI()
    {
        hintText.text = currentHints.ToString();
        hintButton.interactable = currentHints > 0;
        Color buttonColor = hintButton.image.color;
        buttonColor.a = currentHints > 0 ? 1f : 0.35f;
        hintButton.image.color = buttonColor;
    }
}