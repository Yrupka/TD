using UnityEngine;
using UnityEngine.UI;

public class ObjectStats : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 followPosition;

    // статы персонажа
    private new Text name;
    private Text health;
    private Text attack;
    private Text armor;

    // возможные улучшения вышки
    private Transform chooseTower;
    private Transform updateTower;

    // эффекты вышки
    private Transform magic;
    private Transform poison;

    private void Start()
    {
        name = transform.Find("Name").Find("Value").GetComponent<Text>();
        health = transform.Find("Health").Find("Value").GetComponent<Text>();
        attack = transform.Find("Stats").Find("Attack").GetComponent<Text>();
        armor = transform.Find("Stats").Find("Armor").GetComponent<Text>();
        magic = transform.Find("Effects").Find("Magic");
        poison = transform.Find("Effects").Find("Poison");

        cameraTransform = transform.Find("Image").Find("Camera");
        Hide();
    }

    private void Update()
    {
        cameraTransform.position = new Vector3(followPosition.x, followPosition.y + 1f, followPosition.z - 1f);
    }

    public void Show(Transform character)
    {
        if (character == null)
        {
            Hide();
            return;
        }

        string tName = "";
        int tHealth = 0;
        int tArmor = 0;
        int tAttack = 0;
        int tPoison = 0;
        bool tMagic = false;

        switch (character.parent.name)
        {
            case "Tower(Clone)":
                character.parent.GetComponent<Tower>().GetStats(
                    out tName, out tAttack, out tPoison, out tMagic);
                break;
            default:
                break;
        }
        SetStats(tName, tHealth, tArmor, tAttack, tPoison, tMagic);

        gameObject.SetActive(true);
        followPosition = character.position;
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetStats(string tName, int tHealth, int tArmor, int tAttack, int tPoison, bool tMagic)
    {
        name.text = tName;

        if (tHealth == 0)
            health.text = "Неуязвима";
        else
            health.text = tHealth.ToString();

        if (tAttack == 0)
            attack.gameObject.SetActive(false);
        else
            attack.text = $"Атака: {tAttack.ToString()}";

        if (tArmor == 0)
            armor.gameObject.SetActive(false);
        else
            armor.text = $"Защита: {tArmor.ToString()}";

        if (tPoison == 0)
            poison.gameObject.SetActive(false);
        else
        {
            poison.Find("Value").GetComponent<Text>().text = tPoison.ToString();
            poison.gameObject.SetActive(true);
        }
    }
}
