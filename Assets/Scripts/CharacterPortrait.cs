using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPortrait : MonoBehaviour
{
    private Transform cameraTransform;
    private Transform character;

    private void Start()
    {
        cameraTransform = transform.Find("Image").Find("Camera");
        Hide();
    }
    
    private void Update()
    {
        cameraTransform.position = new Vector3(character.position.x, character.position.y, 10f);
    }

    public void Show(Transform transform)
    {
        if (transform == null)
        {
            Hide();
            return;
        }
        gameObject.SetActive(true);
        this.character = transform;
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
