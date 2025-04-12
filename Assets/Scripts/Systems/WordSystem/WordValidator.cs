using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordValidator : MonoBehaviour
{
    private HashSet<string> validWords;

    private void Awake() {
        LoadDictionary();
    }

    private void LoadDictionary() {
        TextAsset dictionaryText = Resources.Load<TextAsset>("dictionary");
        string[] words = dictionaryText.text.Split('\n');
        validWords = new HashSet<string>();

        foreach (var word in words) {
            string trimmedWord = word.Trim().ToUpper();
            // Only add words with length >= 2
            if (trimmedWord.Length >= 2) {
                validWords.Add(trimmedWord);
            }
        }
    }

    public bool IsValidWord(string word) {
        // Check if the word is at least 2 characters long and exists in the dictionary
        return word.Length >= 2 && validWords.Contains(word.ToUpper());
    }
}
