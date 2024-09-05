using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D bulletRigidbody;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float maxProjectileDistance;

    private Vector2 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (Vector2.Distance(startingPosition, transform.position) > maxProjectileDistance)
        {
            Destroy(gameObject);
        }
    }

    public void StartBullet()
    {
        bulletRigidbody.velocity = transform.right * projectileSpeed;
    }

    public void StopBullet()
    {
        bulletRigidbody.velocity = Vector2.zero;
    }
}
