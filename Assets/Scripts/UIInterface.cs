using UnityEngine;
using UnityEngine.UI;

public class UIInterface : MonoBehaviour
{
    private Text manaText;
    [SerializeField] ObjectStats objectStats;

    private void Start() 
    {
        manaText = transform.Find("Mana").GetComponent<Text>();
    }

    public void Mana(int value)
    {
        manaText.text = $"{value}/5";
        if (value > 0)
            objectStats.ActionsVisibility(false);
        else
            objectStats.ActionsVisibility(true);
    }
}
