using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private float currentVelocity;

    [SerializeField]
    private bool colorIndicators = false;

    // UI Objects
    #region
    [SerializeField]
    private TMP_Text velocityMeter;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    #endregion

    // Input Variables
    #region
    private Vector2 mousePositionRelativeToPlayer;
    private Vector2 moveInput;
    private Action chargeButtonClicked;
    private Action dashStart;
    private Action dashEnd;
    #endregion

    // Movement Settings
    #region
    private float maxVelocity = 20f;

    [SerializeField]
    private float runSpeed = 5f;

    [SerializeField]
    private float chargeSpeed = 4f;

    [SerializeField]
    private float chargeRange = 5f;

    [SerializeField]
    private float chargeThreshold = 0.3f;

    [SerializeField]
    private float chargeRangeMultiplier = 1f;

    [SerializeField]
    private float dashVelocityDecreaseSpeed = 1f;

    [SerializeField]
    private float dashEndVelocity = 0.5f;
    #endregion

    // Charge Vectors
    #region
    private Vector2 chargeDestination = Vector2.zero;
    private Vector2 chargeDirection = Vector2.zero;
    private Vector2 chargeStartingVelocity = Vector2.zero;
    #endregion

    [SerializeField]
    private bool isCharging = false;

    [SerializeField]
    private bool isDashing = false;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        // Assign Observers
        #region
        chargeButtonClicked += ChargeStarted;
        dashStart += DashStarted;
        dashEnd += DashEnded;
        #endregion

        if (colorIndicators)
        {
            spriteRenderer.color = Color.blue;
        }
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
        HandleDash();
        HandleCharge();
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

        if (Input.GetButtonDown("Fire2"))
        {
            chargeButtonClicked.Invoke();
        }

        if (Input.GetButtonDown("Jump"))
        {
            dashStart.Invoke();
        }

        if (Input.GetButtonUp("Jump"))
        {
            dashEnd.Invoke();
        }

        moveInput.Normalize();
    }

    private void MovePlayer()
    {
        if (isCharging || isDashing)
        {
            return;
        }

        playerRigidbody.velocity = moveInput * runSpeed;
        //Debug.Log(moveInput * runSpeed);
    }

    private void HandleCharge()
    {
        if (!isCharging)
        {
            return;
        }

        if (Vector2.Distance(transform.position, chargeDestination) < chargeThreshold)
        {
            ChargeEnded();
        }

        playerRigidbody.velocity = chargeStartingVelocity + (chargeDirection * chargeSpeed);
    }

    private void ChargeStarted()
    {
        if (isCharging)
        {
            return;
        }

        chargeStartingVelocity =
            mousePositionRelativeToPlayer.normalized * playerRigidbody.velocity.magnitude;
        chargeDirection = mousePositionRelativeToPlayer.normalized;
        chargeDestination =
            (
                chargeDirection
                * (chargeRange + (playerRigidbody.velocity.magnitude * chargeRangeMultiplier))
            ) + new Vector2(transform.position.x, transform.position.y);
        isCharging = true;
        isDashing = false;
        if (colorIndicators)
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void ChargeEnded()
    {
        chargeDestination = Vector2.zero;
        chargeDirection = Vector2.zero;
        isCharging = false;
        if (colorIndicators)
        {
            spriteRenderer.color = Color.blue;
        }
    }

    private void DashStarted()
    {
        isDashing = true;
        ChargeEnded();
        if (colorIndicators)
        {
            spriteRenderer.color = Color.green;
        }
    }

    private void DashEnded()
    {
        playerRigidbody.velocity = Vector2.zero;
        isDashing = false;
        if (colorIndicators)
        {
            spriteRenderer.color = Color.blue;
        }
    }

    private void HandleDash()
    {
        if (!isDashing)
        {
            return;
        }

        playerRigidbody.velocity = new Vector2(
            Mathf.Lerp(playerRigidbody.velocity.x, 0, Time.deltaTime * dashVelocityDecreaseSpeed),
            Mathf.Lerp(playerRigidbody.velocity.y, 0, Time.deltaTime * dashVelocityDecreaseSpeed)
        );

        if (currentVelocity < dashEndVelocity)
        {
            DashEnded();
        }
    }

    private void DrawDebugLines()
    {
        if (chargeDestination != Vector2.zero)
        {
            Debug.DrawLine(transform.position, chargeDestination, Color.red);
        }
    }
}
