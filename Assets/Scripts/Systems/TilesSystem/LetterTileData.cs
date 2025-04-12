using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LetterTileData", menuName = "ScriptableObjects/LetterTileData", order = 1)]
public class LetterTileData : ScriptableObject
{
    public string letter;
    public int value;
}