using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInterface : MonoBehaviour
{
    private Button start;
    private TextMeshProUGUI manaText;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI enemyText;
    [SerializeField] ObjectStats objectStats;

    public static Action onStart;

    private void Start()
    {
        start = transform.Find("Start").GetComponent<Button>();
        start.onClick.AddListener(() => onStart?.Invoke());
        manaText = transform.Find("Mana").GetComponent<TextMeshProUGUI>();
        healthText = transform.Find("Health").GetComponent<TextMeshProUGUI>();
        enemyText = transform.Find("Enemy").GetComponent<TextMeshProUGUI>();
    }

    public void Mana(int value)
    {
        manaText.text = $"{value}/5";
        if (value > 0)
            NoMana(false);
        else
            NoMana(true);

    }
    private void NoMana(bool value)
    {
        objectStats.TowerComponents(value);
        start.gameObject.SetActive(value);
    }
}
