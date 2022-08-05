using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStats : MonoBehaviour
{
    [System.Serializable]
    private class Translate
    {
        public string name;
        public string rus;
    }
    private enum Character
    {
        tower,
        rock,
        enemy
    }
    private Transform cameraTransform;
    private Transform currentCharacter;
    private Dictionary<string, string> translates;

    // статы персонажа
    private new Text name;
    private Text health;
    private Text attack;
    private Text range;
    private Text attackSpeed;
    private Text armor;

    // возможные улучшения вышки
    private Transform actions;
    private TextMeshProUGUI levelText;

    // эффекты вышки
    private Transform magic;
    private Transform poison;

    // Костыль, не видит raycast'ом UI, за ним находится щит, чтобы тыкать и оно не закрывалось
    [SerializeField] Transform shield;

    private void Start()
    {
        name = transform.Find("Name").Find("Value").GetComponent<Text>();
        health = transform.Find("Health").Find("Value").GetComponent<Text>();
        attack = transform.Find("Stats").Find("Attack").GetComponent<Text>();
        range = transform.Find("Stats").Find("Range").GetComponent<Text>();
        attackSpeed = transform.Find("Stats").Find("AttackSpeed").GetComponent<Text>();
        armor = transform.Find("Stats").Find("Armor").GetComponent<Text>();
        magic = transform.Find("Effects").Find("Magic");
        poison = transform.Find("Effects").Find("Poison");

        actions = transform.Find("Actions").Find("Values");
        levelText = actions.Find("A1").Find("Level").GetComponent<TextMeshProUGUI>();

        cameraTransform = transform.Find("Image").Find("Camera");

        translates = new Dictionary<string, string>();
        var text = Resources.Load<TextAsset>("towersName");
        Translate[] items = JsonHelper.FromJson<Translate>(text.text);
        foreach (var item in items)
            translates.Add(item.name, item.rus);

        Hide();
    }

    private void Update()
    {
        cameraTransform.position = new Vector3(currentCharacter.position.x, currentCharacter.position.y + 1f, currentCharacter.position.z - 1f);
    }

    public void Show(Transform character)
    {
        switch (character.gameObject.layer)
        {
            case 11:
                SetStats(null, Character.rock);
                break;
            case 10:
                Tower tower = character.parent.GetComponent<Tower>();
                SetStats(tower, Character.tower);
                SetActions(tower);
                break;
            case 9:
                Hide();
                return;
            default:
                return;
        }

        currentCharacter = character;
        gameObject.SetActive(true);
        shield.gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
        shield.gameObject.SetActive(false);
    }

    private void SetActions(Tower tower)
    {
        if (tower == null)
            return;

        for (int i = 0; i < tower.Upgrades.Length; i++)
        {
            if (tower.Level != 0)
            {
                levelText.text = tower.Level.ToString();
                levelText.gameObject.SetActive(true);
            }

            Transform action = actions.Find($"A{i + 1}");
            action.gameObject.SetActive(true);
            action.GetComponent<RawImage>().texture = tower.Upgrades[i];
            action.GetComponent<Button>().onClick.RemoveAllListeners();
            action.GetComponent<Button>().onClick.AddListener(
                () => { tower.UpgradeNumber = i; Hide(); ActionsVisibility(false); });
        }

    }

    public void ActionsVisibility(bool value)
    {
        actions.gameObject.SetActive(value);
    }
    private void SetStats(ICharacter character, Character type)
    {
        string tName = "";
        int tHealth = 0;
        int tAttack = 0;
        float tAttackSpeed = 0;
        int tRange = 0;
        int tArmor = 0;
        int tPoison = 0;
        int tMagic = 0;

        switch (type)
        {
            case Character.tower:
            Tower tower = character as Tower;
            tName = translates[tower.Name].ToString() + " " + tower.Level.ToString();
            tAttack = tower.Attack;
            tAttackSpeed = tower.AttackSpeed;
            tRange = tower.Range;
            tPoison = tower.Poison;
            tMagic = tower.Magic;
            break;
            case Character.rock:
            tName = "Камень";
            break;
            case Character.enemy:
            break;
        }

        name.text = tName; 

        if (tHealth == 0)
            health.text = "Неуязвимость";
        else
            health.text = tHealth.ToString();

        if (tAttack == 0)
            attack.gameObject.SetActive(false);
        else
            attack.text = $"Атака: {tAttack.ToString()}";

        if (tRange == 0)
            range.gameObject.SetActive(false);
        else
            range.text = $"Радиус атаки: {tAttack.ToString()}";

        if (tAttackSpeed == 0)
            attackSpeed.gameObject.SetActive(false);
        else
            attackSpeed.text = $"Cкорость атаки: {tAttack.ToString()}";

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
        if (tMagic == 0)
            magic.gameObject.SetActive(false);
        else
        {
            magic.Find("Value").GetComponent<Text>().text = tPoison.ToString();
            magic.gameObject.SetActive(true);
        }
    }
}
