using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    private GameObject bulletPrefab;
    private float currentVelocity;

    // UI Objects
    #region
    private TMP_Text velocityMeter;
    #endregion

    // Input Variables
    #region
    private Vector2 mousePositionRelativeToPlayer;
    private Vector2 moveInput;
    private Action dashButtonClicked;
    private Action slideStart;
    private Action slideEnd;
    #endregion

    // Movement Settings
    #region
    //private float maxVelocity = 20f;

    [Header("Run Settings")]
    [SerializeField]
    private float runSpeed = 5f;

    [Header("Dash Settings")]
    [SerializeField]
    private float dashSpeed = 4f;

    [SerializeField]
    private float dashRange = 5f;

    [SerializeField]
    private float dashThreshold = 0.3f;

    [SerializeField]
    private float dashRangeMultiplier = 1f;

    [Header("Slide Settings")]
    [SerializeField]
    private float slideVelocityDecreaseSpeed = 1f;

    [SerializeField]
    private float slideEndVelocity = 0.5f;

    [Header("Shooting Settings")]
    [SerializeField]
    private float shootingCooldown = 0.2f;

    [SerializeField]
    private float bulletSpreadRange = 5f;
    #endregion

    // Dash Vectors
    #region
    private Vector2 dashDestination = Vector2.zero;
    private Vector2 dashDirection = Vector2.zero;
    private Vector2 dashStartingVelocity = Vector2.zero;
    #endregion

    private bool canShoot = true;

    [Header("Player State Info")]
    [SerializeField]
    private bool isCharging = false;

    [SerializeField]
    private bool isSliding = false;

    [SerializeField]
    private bool isShooting = false;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        velocityMeter = GameObject.Find("VelocityMeter").GetComponent<TMP_Text>();

        // Assign Observers
        #region
        dashButtonClicked += DashStarted;
        slideStart += SlideStarted;
        slideEnd += SlideEnded;
        #endregion
    }

    void Update()
    {
        GetMousePosition();
        GetMoveInput();

        RotatePlayer();

        UpdateUserInterface();
        DrawDebugLines();
    }

    private void FixedUpdate()
    {
        currentVelocity = playerRigidbody.velocity.magnitude;
        MovePlayer();
        HandleSlide();
        HandleDash();
        Shoot();
    }

    private void UpdateUserInterface()
    {
        velocityMeter.text = "Velocity: " + Mathf.Round(currentVelocity * 100f) / 100f;
    }

    private void GetMousePosition()
    {
        mousePositionRelativeToPlayer = Input.mousePosition;

        mousePositionRelativeToPlayer = Camera.main.ScreenToWorldPoint(
            mousePositionRelativeToPlayer
        );

        mousePositionRelativeToPlayer = new Vector2(
            mousePositionRelativeToPlayer.x - transform.position.x,
            mousePositionRelativeToPlayer.y - transform.position.y
        );
    }

    private void RotatePlayer()
    {
        transform.up = mousePositionRelativeToPlayer;
    }

    private void GetMoveInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        isShooting = Input.GetButton("Fire1");

        if (Input.GetButtonDown("Fire2"))
        {
            dashButtonClicked.Invoke();
        }

        if (Input.GetButtonDown("Jump"))
        {
            slideStart.Invoke();
        }

        if (Input.GetButtonUp("Jump"))
        {
            slideEnd.Invoke();
        }

        moveInput.Normalize();
    }

    private void MovePlayer()
    {
        if (isCharging || isSliding)
        {
            return;
        }

        playerRigidbody.velocity = moveInput * runSpeed;
    }

    private void HandleDash()
    {
        if (!isCharging)
        {
            return;
        }

        if (Vector2.Distance(transform.position, dashDestination) < dashThreshold)
        {
            DashEnded();
        }

        playerRigidbody.velocity = dashStartingVelocity + (dashDirection * dashSpeed);
    }

    private void DashStarted()
    {
        if (isCharging)
        {
            return;
        }

        dashStartingVelocity =
            mousePositionRelativeToPlayer.normalized * playerRigidbody.velocity.magnitude;
        dashDirection = mousePositionRelativeToPlayer.normalized;
        dashDestination =
            (
                dashDirection
                * (dashRange + (playerRigidbody.velocity.magnitude * dashRangeMultiplier))
            ) + new Vector2(transform.position.x, transform.position.y);
        isCharging = true;
        isSliding = false;
    }

    private void DashEnded()
    {
        dashDestination = Vector2.zero;
        dashDirection = Vector2.zero;
        isCharging = false;
    }

    private void HandleSlide()
    {
        if (!isSliding)
        {
            return;
        }

        playerRigidbody.velocity = new Vector2(
            Mathf.Lerp(playerRigidbody.velocity.x, 0, Time.deltaTime * slideVelocityDecreaseSpeed),
            Mathf.Lerp(playerRigidbody.velocity.y, 0, Time.deltaTime * slideVelocityDecreaseSpeed)
        );

        if (currentVelocity < slideEndVelocity)
        {
            SlideEnded();
        }
    }

    private void SlideStarted()
    {
        isSliding = true;
        DashEnded();
    }

    private void SlideEnded()
    {
        playerRigidbody.velocity = Vector2.zero;
        isSliding = false;
    }

    private void Shoot()
    {
        if (!isShooting || !canShoot)
        {
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = transform.position;
        bullet.transform.right = mousePositionRelativeToPlayer;

        Debug.Log(bullet.transform.eulerAngles.z);

        bullet.transform.eulerAngles = new Vector3(
            0,
            0,
            bullet.transform.eulerAngles.z + CalculateBulletDirection()
        );

        bullet.GetComponent<Projectile>().StartBullet();

        StartCoroutine(ShootingCooldown());
    }

    private float CalculateBulletDirection()
    {
        return UnityEngine.Random.Range(-bulletSpreadRange, bulletSpreadRange);
    }

    private IEnumerator ShootingCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }

    private void DrawDebugLines()
    {
        if (dashDestination != Vector2.zero)
        {
            Debug.DrawLine(transform.position, dashDestination, Color.red);
        }
    }
}
