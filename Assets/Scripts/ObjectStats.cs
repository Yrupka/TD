using UnityEngine;
using UnityEngine.UI;

public class ObjectStats : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 followPosition;

    // статы персонажа
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

        switch (character.parent.name)
        {
            case "Tower(Clone)":
                Debug.Log("tower");
                break;
            case "Rock(Clone)":
                Debug.Log("rock");
                break;
            default:
                break;
        }


        gameObject.SetActive(true);
        followPosition = character.position;
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
