using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public enum PlayerState
{
    Basic,
    Dashing,
    Sliding,
}

public class PlayerMovementManager : MonoBehaviour
{
    public PlayerState currentPlayerState = PlayerState.Basic;
    private Rigidbody2D playerRigidbody;

    [Header("Settings")]
    [SerializeField]
    private float maxVelocityMagnitude = 20f;

    [SerializeField]
    private float runSpeed = 5f;

    private Vector2 cursorDirection;
    private Vector2 moveInput;

    public Action dashStart;
    public Action slideStart;
    public Action slideEnd;

    public bool isShooting;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
        RotatePlayer();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        cursorDirection = Input.mousePosition;
        cursorDirection = Camera.main.ScreenToWorldPoint(cursorDirection);
        cursorDirection = new Vector2(
            cursorDirection.x - transform.position.x,
            cursorDirection.y - transform.position.y
        );
        cursorDirection.Normalize();

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        isShooting = Input.GetButton("Fire1");

        if (Input.GetButtonDown("Fire2"))
        {
            dashStart.Invoke();
        }

        if (Input.GetButtonDown("Jump"))
        {
            slideStart.Invoke();
        }

        if (Input.GetButtonUp("Jump"))
        {
            slideEnd.Invoke();
        }
    }

    private void RotatePlayer()
    {
        transform.up = cursorDirection;
    }

    private void MovePlayer()
    {
        if (currentPlayerState != PlayerState.Basic)
        {
            return;
        }

        playerRigidbody.velocity = moveInput * runSpeed;
    }

    public Vector2 GetCurrentPosition()
    {
        return transform.position;
    }

    public Vector2 GetCurrentVelocity()
    {
        return playerRigidbody.velocity;
    }

    public Vector2 GetCurrentCursorDirection()
    {
        return cursorDirection;
    }

    public void SetPlayerVelocity(Vector2 newVelocity)
    {
        if (newVelocity.magnitude > maxVelocityMagnitude)
        {
            playerRigidbody.velocity = newVelocity.normalized * maxVelocityMagnitude;
            return;
        }

        playerRigidbody.velocity = newVelocity;
    }
}
