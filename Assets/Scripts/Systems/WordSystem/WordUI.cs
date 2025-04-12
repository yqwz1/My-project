using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordUI : MonoBehaviour
{
    public static WordUI Instance;

    [SerializeField] private TextMeshProUGUI wordText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hintTextUI; // 🔥 Added this line!
    [SerializeField] private Button submitButton;

    private void Awake()
    {
        Instance = this;
        ShowSubmitButton(false);
    }

    public void UpdateWord(string word, int score, bool isValid)
    {
        wordText.text = word;
        scoreText.text = "Score: " + score;
        wordText.color = isValid ? Color.green : Color.red;
    }
    
    public void ShowSubmitButton(bool show)
    {
        submitButton.interactable = show;
        Color buttonColor = submitButton.image.color;
        buttonColor.a = show ? 1f : 0.35f;
        submitButton.image.color = buttonColor;
    }

    public void UpdateHint(string hintText, int hintsLeft)
    {
        hintTextUI.text = $"Hint: {hintText}  ({hintsLeft} left)";
    }
}