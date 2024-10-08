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

    #region Settings
    [Header("Settings")]
    [SerializeField]
    private int playerHealth = 5;

    private float maxVelocityMagnitude = 20f;

    [SerializeField]
    private float runSpeed = 5f;

    [SerializeField]
    private float minStunVelocity = 10f;

    [SerializeField]
    private float stunLength = 2f;
    #endregion

    private Vector2 cursorDirection;
    private Vector2 moveInput;

    #region Actions
    public Action dashStart;
    public Action wallHit;
    public Action slideStart;
    public Action slideEnd;
    #endregion

    public bool isShooting;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
        RotatePlayer();
        CheckForDeath();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        CheckForStun();
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

    #region Player Management
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

        playerRigidbody.velocity = moveInput.normalized * runSpeed;
    }

    private void CheckForStun()
    {
        if (GetCurrentVelocity().magnitude < minStunVelocity)
        {
            return;
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            GetComponent<CircleCollider2D>().radius,
            GetCurrentVelocity(),
            0.1f
        );

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                ApplyStun();
            }
        }
    }

    private void CheckForDeath()
    {
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
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
    #endregion

    #region Get Methods
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

    public float GetMaxVelocity()
    {
        return maxVelocityMagnitude;
    }

    public float GetMinStunVelocity()
    {
        return minStunVelocity;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
    #endregion

    #region Set Methods
    public void SetPlayerVelocity(Vector2 newVelocity)
    {
        if (newVelocity.magnitude > maxVelocityMagnitude)
        {
            playerRigidbody.velocity = newVelocity.normalized * maxVelocityMagnitude;
            return;
        }

        playerRigidbody.velocity = newVelocity;
    }

    public void UpdatePlayerHealth(int health)
    {
        playerHealth += health;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        SetPlayerVelocity(Vector2.zero);
        wallHit.Invoke();
    }
}
