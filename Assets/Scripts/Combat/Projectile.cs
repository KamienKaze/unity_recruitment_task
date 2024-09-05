using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D projectileRigidbody;

    private float projectileSpeed;
    private float projectileDamage;
    private float maxProjectileDistance;

    private Vector2 projectileStartingPosition;

    private void Start()
    {
        projectileStartingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        ControlBulletDistance();
    }

    public void SetProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public void SetProjectileDamage(float projectileDamage)
    {
        this.projectileDamage = projectileDamage;
    }

    public void SetMaxProjectileDistance(float maxProjectileDistance)
    {
        this.maxProjectileDistance = maxProjectileDistance;
    }

    public void SetProjectileSprite(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void ControlBulletDistance()
    {
        if (
            Vector2.Distance(projectileStartingPosition, transform.position) > maxProjectileDistance
        )
        {
            Destroy(gameObject);
        }
    }

    public void StartProjectile()
    {
        projectileRigidbody.velocity = transform.right * projectileSpeed;
    }

    public void StopProjectile()
    {
        projectileRigidbody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            return;
        }

        Destroy(gameObject);
    }
}
