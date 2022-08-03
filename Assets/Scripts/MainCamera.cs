using UnityEngine;
using System;

public class MainCamera : MonoBehaviour
{
    float edgeOffset = 20f;
    float moveStep = 40f;
    float zoomStep = 40f;
    Vector3 cameraPosition = new Vector3(0, 10, 0);

    public static Action<Transform> selected;

    void Update()
    {
        this.transform.position = cameraPosition;
        MouseCameraMove();
        MouseCameraScroll();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
            {
                selected?.Invoke(raycastHit.transform);
            }
        }
    }

    void MouseCameraMove()
    {
        // право
        if (Input.mousePosition.x > Screen.width - edgeOffset)
            cameraPosition.x += moveStep * Time.deltaTime;
        // лево
        if (Input.mousePosition.x < edgeOffset)
            cameraPosition.x -= moveStep * Time.deltaTime;
        //вверх
        if (Input.mousePosition.y > Screen.height - edgeOffset)
            cameraPosition.z += moveStep * Time.deltaTime;
        //вниз
        if (Input.mousePosition.y < edgeOffset)
            cameraPosition.z -= moveStep * Time.deltaTime;
    }

    void MouseCameraScroll()
    {
        if (Input.mouseScrollDelta.y > 0)
            cameraPosition.y -= zoomStep * Time.deltaTime;
        if (Input.mouseScrollDelta.y < 0)
            cameraPosition.y += zoomStep * Time.deltaTime;
        
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, 1f, 15f);
        moveStep = cameraPosition.y;
    }
}
