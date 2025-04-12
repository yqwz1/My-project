using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance;

    private HashSet<string> wordSet = new();

    private void Awake()
    {
        Instance = this;

        TextAsset dictionaryFile = Resources.Load<TextAsset>("dictionary");
        string[] words = dictionaryFile.text.Split('\n');

        foreach (var word in words)
        {
            wordSet.Add(word.Trim().ToUpper());
        }
    }

    public bool IsValidWord(string word)
    {
        return wordSet.Contains(word.ToUpper());
    }

    public HashSet<string> GetAllWords()
    {
        return wordSet;
    }
}
