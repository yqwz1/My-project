using UnityEngine;

public class LetterTile : MonoBehaviour
{
    public LetterTileData letterTileData;
    public bool isSelected;

    private Transform originalParent;
    private LetterTileUI tileUI;

    private void Awake()
    {
        tileUI = GetComponent<LetterTileUI>();
        Debug.Log($"[LetterTile] Awake called on {gameObject.name}");
    }
    
    public void SetTileUI(LetterTileUI ui)
    {
        tileUI = ui;
    }
    
    public void SetColor(Color color)
    {
        if (tileUI == null)
        {
            Debug.LogError("[LetterTile] tileUI reference is missing in SetColor!");
            return;
        }
        tileUI.SetColor(color);
    }

    public void Init(LetterTileData data, Transform parent)
    {
        Debug.Log($"[LetterTile] Init called with letter: {data.letter}");
        letterTileData = data;
        originalParent = parent;
        
        if (parent == null)
        {
            Debug.LogError("[LetterTile] Parent is null!");
            return;
        }

        transform.SetParent(parent);
        transform.localScale = Vector3.one;

        if (tileUI == null)
        {
            Debug.LogError("[LetterTile] tileUI reference is missing!");
            return;
        }

        tileUI.Init(this);
    }

    public void ToggleSelect()
    {
        Debug.Log($"[LetterTile] ToggleSelect called on {letterTileData.letter}");
        isSelected = !isSelected;

        if (WordBuilder.Instance == null)
        {
            Debug.LogError("[LetterTile] WordBuilder instance is missing!");
            return;
        }

        if (isSelected)
        {
            Debug.Log($"[LetterTile] Adding tile {letterTileData.letter}");
            WordBuilder.Instance.AddTile(this);
        }
        else
        {
            Debug.Log($"[LetterTile] Removing tile {letterTileData.letter}");
            WordBuilder.Instance.RemoveTile(this); 
            SetColor(Color.white);
        }

        if (tileUI == null)
        {
            Debug.LogError("[LetterTile] tileUI reference is missing in ToggleSelect!");
            return;
        }

        tileUI.UpdateVisuals();
    }

    public void MoveTo(Transform newParent)
    {
        Debug.Log($"[LetterTile] Moving {letterTileData.letter} to parent: {newParent.name}");
        if (newParent == null)
        {
            Debug.LogError("[LetterTile] New parent is null!");
            return;
        }
        
        transform.SetParent(newParent);
        transform.localScale = Vector3.one;
    }

    public void ResetTile()
    {
        Debug.Log($"[LetterTile] Resetting {letterTileData.letter}");
        isSelected = false;
        
        if (originalParent == null)
        {
            Debug.LogError("[LetterTile] Original parent is null!");
            return;
        }
        
        MoveTo(originalParent);
        
        if (tileUI == null)
        {
            Debug.LogError("[LetterTile] tileUI reference is missing in ResetTile!");
            return;
        }
        
        tileUI.UpdateVisuals();
    }
}