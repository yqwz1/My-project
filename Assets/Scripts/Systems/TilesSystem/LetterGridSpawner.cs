using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetterGridSpawner : MonoBehaviour
{
 public static LetterGridSpawner Instance;

    [Header("Grid Settings")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private LetterTileData[] availableLetters;

    [Header("Vowel Settings")]
    [SerializeField] [Range(0, 1)] private float vowelSpawnProbability = 0.6f;
    [SerializeField] private string vowels = "AEIOU";

    [SerializeField] private int columns = 5;
    [SerializeField] private int rows = 2;

    private List<LetterTileData> vowelLetters = new();
    private List<LetterTileData> consonantLetters = new();
    public List<LetterTile> spawnedTiles = new();

    private int GridSize => columns * rows;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("[LetterGridSpawner] Duplicate instance destroyed");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CategorizeLetters();
        SpawnGrid();
    }

    void CategorizeLetters()
    {
        foreach (LetterTileData data in availableLetters)
        {
            if (IsVowel(data))
                vowelLetters.Add(data);
            else
                consonantLetters.Add(data);
        }
    }

    bool IsVowel(LetterTileData data)
    {
        return vowels.Contains(data.letter.ToUpper());
    }

    public void SpawnGrid()
    {
        ClearGrid();
        spawnedTiles.Clear();

        List<char> lettersToSpawn = GenerateSmartLetterSet();

        for (int i = 0; i < lettersToSpawn.Count; i++)
        {
            GameObject tileGO = Instantiate(tilePrefab, gridParent);

            LetterTile logic = tileGO.GetComponent<LetterTile>();
            LetterTileUI ui = tileGO.GetComponent<LetterTileUI>();

            if (logic == null || ui == null)
            {
                Debug.LogError("[LetterGridSpawner] Tile prefab is missing components!");
                continue;
            }

            logic.SetTileUI(ui);

            string letterStr = lettersToSpawn[i].ToString();
            LetterTileData data = availableLetters.FirstOrDefault(d => d.letter == letterStr);

            if (data == null)
            {
                Debug.LogError($"[LetterGridSpawner] Could not find data for letter '{letterStr}'");
                continue;
            }

            logic.Init(data, gridParent);
            spawnedTiles.Add(logic);
        }

        HintSystem.Instance.ResetHints(spawnedTiles);
    }

    private List<char> GenerateSmartLetterSet()
    {
        HashSet<string> allWords = DictionaryManager.Instance.GetAllWords();
        int maxTries = 100;

        for (int attempt = 0; attempt < maxTries; attempt++)
        {
            string selectedWord = allWords
                .Where(w => w.Length >= 4 && w.Length <= GridSize)
                .OrderBy(w => Random.value)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(selectedWord)) continue;

            List<char> result = new(selectedWord.ToUpper().ToCharArray());

            // Fill the rest randomly
            while (result.Count < GridSize)
            {
                var letterPool = availableLetters[Random.Range(0, availableLetters.Length)];
                result.Add(letterPool.letter[0]);
            }

            // Shuffle result so the word isn't in order
            result = result.OrderBy(_ => Random.value).ToList();

            return result;
        }

        Debug.LogWarning("[LetterGridSpawner] Could not find a valid word to embed in grid.");
        return GeneratePureRandomLetters(); // fallback
    }

    private List<char> GeneratePureRandomLetters()
    {
        List<char> randomLetters = new();

        for (int i = 0; i < GridSize; i++)
        {
            LetterTileData data = GetWeightedRandomLetter();
            randomLetters.Add(data.letter[0]);
        }

        return randomLetters;
    }

    LetterTileData GetWeightedRandomLetter()
    {
        bool useVowel = vowelLetters.Count > 0 &&
                        (consonantLetters.Count == 0 || Random.value < vowelSpawnProbability);

        return useVowel
            ? vowelLetters[Random.Range(0, vowelLetters.Count)]
            : consonantLetters[Random.Range(0, consonantLetters.Count)];
    }

    public void ClearGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }
}