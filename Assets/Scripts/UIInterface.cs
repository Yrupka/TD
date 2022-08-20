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
    private TextMeshProUGUI waveText;
    private TextMeshProUGUI timeText;

    public Action onStart;

    private void Update()
    {
        timeText.SetText(Time.realtimeSinceStartup.ToString("F2"));
    }

    public void Init(int mana, int health)
    {
        start = transform.Find("Start").GetComponent<Button>();
        start.onClick.AddListener(() => onStart?.Invoke());
        manaText = transform.Find("Mana").GetComponent<TextMeshProUGUI>();
        healthText = transform.Find("Health").GetComponent<TextMeshProUGUI>();
        enemyText = transform.Find("Enemy").GetComponent<TextMeshProUGUI>();
        waveText = transform.Find("Wave").GetComponent<TextMeshProUGUI>();
        timeText = transform.Find("Time").GetComponent<TextMeshProUGUI>();

        manaText.SetText($"{mana}/{mana}");
        healthText.SetText(health.ToString());
        enemyText.SetText("0");
        waveText.SetText("0");
        timeText.SetText("0");
    }

    public void Mana(int value, int maxValue)
    {
        manaText.text = $"{value}/{maxValue}";
    }

    public void TowersBuilded(bool value)
    {
        start.gameObject.SetActive(value);
    }
}
