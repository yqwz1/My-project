using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public static EnemyUI Instance;

    public Slider hpBar;
    public TextMeshProUGUI hpText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHP(int current, int max)
    {
        hpBar.maxValue = max;
        hpBar.value = current;
        hpText.text = $"{current} / {max}";
    }
}
