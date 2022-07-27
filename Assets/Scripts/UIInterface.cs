using UnityEngine;
using UnityEngine.UI;

public class UIInterface : MonoBehaviour
{
    private Text manaText;
    private void Start() 
    {
        manaText = transform.Find("Mana").GetComponent<Text>();
    }
    public void Mana(int value)
    {
        manaText.text = $"{value}/5";
    }
}
