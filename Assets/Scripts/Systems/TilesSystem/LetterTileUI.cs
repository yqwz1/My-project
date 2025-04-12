using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterTileUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image background;

    private LetterTile tile;

    public void Init(LetterTile tileLogic)
    {
        Debug.Log($"[LetterTileUI] Initializing UI for {tileLogic.letterTileData.letter}");

        if (letterText == null) Debug.LogError("[LetterTileUI] LetterText reference missing!");
        if (valueText == null) Debug.LogError("[LetterTileUI] ValueText reference missing!");
        if (background == null) Debug.LogError("[LetterTileUI] Background reference missing!");

        tile = tileLogic;

        letterText.text = tile.letterTileData.letter;
        valueText.text = tile.letterTileData.value.ToString();
    }

    public void OnClick()
    {
        if (tile == null)
        {
            Debug.LogError("[LetterTileUI] Tile reference missing!");
            return;
        }

        Debug.Log($"[LetterTileUI] Click detected on {tile.letterTileData.letter}");
        tile.ToggleSelect();
    }

    public void UpdateVisuals()
    {
        if (background == null)
        {
            Debug.LogError("[LetterTileUI] Background reference missing in UpdateVisuals!");
            return;
        }

        
    }

    public void SetColor(Color color)
    {
        if (background == null)
        {
            Debug.LogError("[LetterTileUI] Background reference missing in SetColor!");
            return;
        }

        background.color = color;
    }
}
