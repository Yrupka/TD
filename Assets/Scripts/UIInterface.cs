using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInterface : MonoBehaviour
{
    private Button start;
    private Text manaText;
    [SerializeField] ObjectStats objectStats;

    public static Action onStart;

    private void Start() 
    {
        start = transform.Find("Start").GetComponent<Button>();
        start.onClick.AddListener(() => onStart?.Invoke());
        manaText = transform.Find("Mana").GetComponent<Text>();
    }

    public void Mana(int value)
    {
        manaText.text = $"{value}/5";
        if (value > 0)
        {
            objectStats.ActionsVisibility(false);
            start.gameObject.SetActive(false);
        }
            
        else
        {
            objectStats.ActionsVisibility(true);
            start.gameObject.SetActive(true);
        }
            
    }
}
