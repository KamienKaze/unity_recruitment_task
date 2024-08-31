using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    private Transform shootingPoint;

    [SerializeField]
    private GameObject bulletPrefab;

    // Input Variables
    #region
    private float horizontal;
    private float vertical;
    private Vector3 mousePosition;
    #endregion

    [SerializeField]
    private float playerHealth = 5.0f;

    [SerializeField]
    private float runSpeedMultiplier = 5.0f;

    [SerializeField]
    private float scaleMultiplier = 0.5f;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GatherInput();
        RotatePlayer();

        DebugHealth();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GatherInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        mousePosition = Input.mousePosition;
    }

    private void RotatePlayer()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = direction;
    }

    private void MovePlayer()
    {
        playerRigidbody.velocity =
            new Vector2(horizontal, vertical).normalized * runSpeedMultiplier;
    }

    private void HandleHealthChanges() { }

    public void DebugHealth()
    {
        if (Input.GetKeyDown("+"))
        {
            Debug.Log("+");
        }
        if (Input.GetKeyDown("-"))
        {
            Debug.Log("-");
        }
    }
}
