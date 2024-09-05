using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public enum PlayerState
{
    Basic,
    Dashing,
    Sliding,
    Stunned,
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

    [SerializeField]
    private float minStunVelocity = 10f;

    [SerializeField]
    private float stunLength = 2f;

    private Vector2 cursorDirection;
    private Vector2 moveInput;

    public Action dashStart;
    public Action slideStart;
    public Action slideEnd;
    public Action colliderHit;

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
        if (currentPlayerState == PlayerState.Stunned)
        {
            return;
        }

        transform.up = cursorDirection;
    }

    private void MovePlayer()
    {
        if (currentPlayerState != PlayerState.Basic)
        {
            return;
        }

        if (currentPlayerState == PlayerState.Stunned)
        {
            return;
        }

        playerRigidbody.velocity = moveInput * runSpeed;
    }

    private void ApplyStun()
    {
        currentPlayerState = PlayerState.Stunned;
        playerRigidbody.velocity = Vector2.zero;
        StartCoroutine(StunTimer());
    }

    private IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(stunLength);
        currentPlayerState = PlayerState.Basic;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetCurrentVelocity().magnitude >= minStunVelocity)
        {
            ApplyStun();
        }
        else
        {
            colliderHit.Invoke();
            SetPlayerVelocity(Vector2.zero);
        }
    }
}
