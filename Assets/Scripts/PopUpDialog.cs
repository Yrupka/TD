using TMPro;
using UnityEngine;

public class PopUpDialog : MonoBehaviour
{
    public static void Create(Vector3 pos, string info)
    {
        prefab = Resources.Load<Transform>("Prefabs/Dialog");
        Transform dialog = Instantiate(prefab);
        dialog.position = pos + Vector3.up * 1.5f;
        dialog.GetComponent<PopUpDialog>().Setup(info);

        Destroy(dialog.gameObject, 2f);
    }
    
    private static Transform prefab;


    private void Setup(string info)
    {
        transform.Find("Text").GetComponent<TextMeshPro>().SetText(info);
    }
}
