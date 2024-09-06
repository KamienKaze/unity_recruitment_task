using System.Collections;
using UnityEngine;

public class PlayerShooting : MovementExtension
{
    [Header("Settings")]
    #region
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Sprite bulletSprite;

    [SerializeField]
    private float bulletSpreadRange = 5f;

    [SerializeField]
    private float bulletSpeed = 20f;

    [SerializeField]
    private float shootingCooldown = 0.2f;

    [SerializeField]
    private int bulletDamage = 1;

    [SerializeField]
    private float maxBulletDistance = 30f;
    #endregion

    private bool isShooting = false;
    private bool canShoot = true;

    private void FixedUpdate()
    {
        isShooting = playerMovementManager.isShooting;
        Shoot();
    }

    private void Shoot()
    {
        if (!isShooting || !canShoot)
        {
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = currentPlayerPosition;

        bullet.transform.right = currentCursorDirection;

        bullet.transform.eulerAngles = new Vector3(
            0,
            0,
            bullet.transform.eulerAngles.z + GetBulletOffset()
        );

        Projectile bulletProjectile = bullet.GetComponent<Projectile>();
        bulletProjectile.SetProjectileSpeed(bulletSpeed);
        bulletProjectile.SetProjectileDamage(bulletDamage);
        bulletProjectile.SetMaxProjectileDistance(maxBulletDistance);
        bulletProjectile.SetProjectileSprite(bulletSprite);

        bulletProjectile.StartProjectile();

        StartCoroutine(ShootingCooldown());
    }

    private IEnumerator ShootingCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }

    private float GetBulletOffset()
    {
        return Random.Range(-bulletSpreadRange, bulletSpreadRange);
    }
}
