using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static Transform bullet;
    private Vector3 targetPosition;
    
    public static void Create(Vector3 spawnPosition, Vector3 targetPosition)
    {
        bullet = Resources.Load<Transform>("Prefabs/Bullet");
        Transform bulletTransform = Instantiate(bullet, spawnPosition, Quaternion.identity);
        bulletTransform.GetComponent<Bullet>().Setup(targetPosition);
    }

    private void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        Debug.Log(targetPosition);
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float speed = 1f;

        transform.position += moveDir * speed * Time.deltaTime;

        float destrouDistanse = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) <= destrouDistanse)
            Destroy(gameObject);
    }
}
