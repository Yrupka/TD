using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 followPosition;

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
        gameObject.SetActive(true);
        followPosition = character.position;
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
