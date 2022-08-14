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
    private enum Effects
    {
        magic,
        poison
    }
    private Transform cameraTransform;
    private Transform currentCharacter;
    private Dictionary<string, string> translates;

    // статы персонажей
    private TextMeshProUGUI[] fieldsEnemy;
    private TextMeshProUGUI[] fieldsTower;
    private string[] fieldsTextEnemy;
    private string[] fieldsTextTower;

    // возможные улучшения вышки
    private Transform actions;
    private TextMeshProUGUI levelText;

    // эффекты вышки
    private Transform effects;
    private Transform magic;
    private Transform poison;

    // Костыль, не видит raycast'ом UI, за ним находится щит, чтобы тыкать и оно не закрывалось
    [SerializeField] Transform shield;

    private void Start()
    {
        fieldsTextEnemy = new string[2] { "Защита: ", "Скорость: " };
        fieldsTextTower = new string[3] { "Атака: ", "Cкорость атаки: ", "Радиус атаки: " };

        Transform stats = transform.Find("Stats");
        fieldsEnemy = new TextMeshProUGUI[4];
        fieldsEnemy[0] = transform.Find("Name").Find("Value").GetComponent<TextMeshProUGUI>();
        fieldsEnemy[1] = transform.Find("Health").Find("Value").GetComponent<TextMeshProUGUI>();
        fieldsEnemy[2] = stats.Find("Enemy").Find("Speed").GetComponent<TextMeshProUGUI>();
        fieldsEnemy[3] = stats.Find("Enemy").Find("Armor").GetComponent<TextMeshProUGUI>();
        fieldsTower = new TextMeshProUGUI[4];
        fieldsTower[0] = transform.Find("Name").Find("Value").GetComponent<TextMeshProUGUI>();
        fieldsTower[1] = stats.Find("Tower").Find("Attack").GetComponent<TextMeshProUGUI>();
        fieldsTower[2] = stats.Find("Tower").Find("AttackSpeed").GetComponent<TextMeshProUGUI>();
        fieldsTower[3] = stats.Find("Tower").Find("Range").GetComponent<TextMeshProUGUI>();

        effects = transform.Find("Effects").Find("Values");
        magic = effects.Find("Magic");
        poison = effects.Find("Poison");

        actions = transform.Find("Actions").Find("Values");
        levelText = actions.Find("A1").Find("Level").GetComponent<TextMeshProUGUI>();

        cameraTransform = transform.Find("Image").Find("Camera");

        translates = new Dictionary<string, string>();
        var text = Resources.Load<TextAsset>("towersName");
        Translate[] items = JsonHelper.FromJson<Translate>(text.text);
        foreach (var item in items)
            translates.Add(item.name, item.rus);

        Visibility(false);
    }

    private void Update()
    {
        cameraTransform.position = new Vector3(currentCharacter.position.x, currentCharacter.position.y + 1f, currentCharacter.position.z - 1f);
    }

    public void Show(Transform character)
    {
        switch (character.gameObject.layer)
        {
            case 12:
                PickRock();
                break;
            case 11:
                Enemy enemy = character.GetComponent<Enemy>();
                PickEnemy(enemy);
                PickTower(null);
                break;
            case 10:
                Tower tower = character.parent.GetComponent<Tower>();
                PickTower(tower);
                PickEnemy(null);
                SetActions(tower);
                break;
            case 9:
                Visibility(false);
                return;
            default:
                return;
        }
        currentCharacter = character;
        Visibility(true);
    }
    private void Visibility(bool state)
    {
        gameObject.SetActive(state);
        shield.gameObject.SetActive(state);
    }

    private void SetActions(Tower tower)
    {
        if (tower.upgrades == null)
        {
            for (int i = 1; i <= 3; i++)
                actions.Find($"A{i}").gameObject.SetActive(false);
            return;
        }
            
        for (int i = 0; i < 3; i++)
        {
            if (tower.upgrades[i] == null)
                break;
            if (tower.Level != 0)
            {
                levelText.text = tower.Level.ToString();
                levelText.gameObject.SetActive(true);
            }

            Transform action = actions.Find($"A{i + 1}");
            action.gameObject.SetActive(true);
            action.GetComponent<RawImage>().texture = tower.upgrades[i];
            action.GetComponent<Button>().onClick.RemoveAllListeners();
            action.GetComponent<Button>().onClick.AddListener(() => TowerPicked(tower, action.name));
        }

    }

    private void TowerPicked(Tower tower, string name)
    {
        int slotNum = int.Parse(name.Substring(1)) - 1;
        tower.upgradesNum[0] = tower.upgradesNum[slotNum];
        tower.upgraded?.Invoke(tower);
        Visibility(false);
    }

    public void ActionsVisibility(bool value)
    {
        actions.gameObject.SetActive(value);
    }

    private void PickTower(Tower tower)
    {
        if (tower == null)
        {
            fieldsTower[1].transform.parent.gameObject.SetActive(false);
            ActionsVisibility(false);
            return;
        }

        string name = translates[tower.Name] + " ";
        if (tower.Level != 0)
            name += tower.Level.ToString();

        fieldsTower[0].SetText(name);
        fieldsTower[1].SetText(fieldsTextTower[0] + tower.Attack.ToString());
        fieldsEnemy[1].SetText("Неуязвимость");
        fieldsTower[2].SetText(fieldsTextTower[1] + tower.AttackSpeed.ToString());
        fieldsTower[3].SetText(fieldsTextTower[2] + tower.Range.ToString());

        // включить отображение характеристик
        fieldsTower[1].transform.parent.gameObject.SetActive(true);

        SetEffect(Effects.magic, tower.Magic);
        SetEffect(Effects.poison, tower.Poison);
        effects.gameObject.SetActive(true);
    }

    private void PickEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            fieldsEnemy[2].transform.parent.gameObject.SetActive(false);
            return;
        }

        fieldsEnemy[0].SetText(enemy.Name);
        fieldsEnemy[1].SetText(enemy.CurrentHealth.ToString() + "/" + enemy.Health.ToString());
        fieldsEnemy[2].SetText(fieldsTextEnemy[0] + enemy.Armor.ToString());
        fieldsEnemy[3].SetText(fieldsTextEnemy[1] + enemy.Speed.ToString());
        // включить отображение характеристик
        fieldsEnemy[2].transform.parent.gameObject.SetActive(true);
    }

    private void PickRock()
    {
        fieldsEnemy[0].SetText("Камень");
        fieldsEnemy[1].SetText("Неуязвимость");
        PickEnemy(null);
        PickTower(null);
    }

    private void SetEffect(Effects effect, int value)
    {
        switch (effect)
        {
            case Effects.magic:
                if (value == 0)
                {
                    magic.gameObject.SetActive(false);
                    break;
                }
                magic.Find("Value").GetComponent<Text>().text = value.ToString();
                magic.gameObject.SetActive(true);
                break;
            case Effects.poison:
                if (value == 0)
                {
                    poison.gameObject.SetActive(false);
                    break;
                }
                poison.Find("Value").GetComponent<Text>().text = value.ToString();
                poison.gameObject.SetActive(true);
                break;
            default:
                return;
        }
    }
}
