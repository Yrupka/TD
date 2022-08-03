using UnityEngine;
using UnityEngine.UI;

public class ObjectStats : MonoBehaviour
{
    private Transform cameraTransform;
    private Transform currentCharacter;

    // статы персонажа
    private new Text name;
    private Text health;
    private Text attack;
    private Text range;
    private Text attackSpeed;
    private Text armor;

    // возможные улучшения вышки
    private Transform actions;

    // эффекты вышки
    private Transform magic;
    private Transform poison;

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

        cameraTransform = transform.Find("Image").Find("Camera");
        Hide();
    }

    private void Update()
    {
        cameraTransform.position = new Vector3(currentCharacter.position.x, currentCharacter.position.y + 1f, currentCharacter.position.z - 1f);
    }

    public void Show(Transform character)
    {
        string tName = "";
        int tHealth = 0;
        int tArmor = 0;
        int tAttack = 0;
        int tRange = 0;
        float tAttackSpeed = 0f;
        int tPoison = 0;
        bool tMagic = false;

        switch (character.gameObject.layer)
        {
            case 11:
                tName = "Камень";
                break;
            case 10:
                character.parent.GetComponent<Tower>().GetStats(
                    out tName, out tAttack, out tRange, out tAttackSpeed, out tPoison, out tMagic);
                SetActions(character.parent.GetComponent<Tower>());
                break;
            case 9:
                Hide();
                return;
            default:
                return;
        }

        SetStats(tName, tHealth, tArmor, tAttack, tRange, tAttackSpeed, tPoison, tMagic);

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
    private void SetStats(string tName, int tHealth, int tArmor, int tAttack, int tRange, float tAttackSpeed, int tPoison, bool tMagic)
    {
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
    }
}
