using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Dash : MovementExtension
{
    // Editor Settings
    #region
    [Header("Settings")]
    [SerializeField]
    private float dashSpeed = 4f;

    [SerializeField]
    private float dashRange = 5f;

    [SerializeField]
    private float dashEndThreshold = 0.3f;

    [SerializeField]
    private float dashRangeMultiplier = 1f;
    #endregion

    // Vectors
    #region
    private Vector2 dashDestination = Vector2.zero;
    private Vector2 dashDirection = Vector2.zero;
    private Vector2 dashStartingVelocity = Vector2.zero;
    #endregion

    void Start()
    {
        playerMovementManager.dashStart += DashStarted;
    }

    private void FixedUpdate()
    {
        HandleDash();
    }

    private void DashStarted()
    {
        if (playerMovementManager.currentPlayerState == PlayerState.Dashing)
        {
            return;
        }

        dashStartingVelocity = currentCursorDirection * currentPlayerVelocity.magnitude;
        dashDirection = currentCursorDirection;
        dashDestination = (
            dashDirection * (dashRange + (currentPlayerVelocity.magnitude * dashRangeMultiplier))
            + currentPlayerPosition
        );

        playerMovementManager.currentPlayerState = PlayerState.Dashing;
    }

    private void DashEnded()
    {
        dashDestination = Vector2.zero;
        dashDirection = Vector2.zero;
        playerMovementManager.currentPlayerState = PlayerState.Basic;
    }

    private void HandleDash()
    {
        if (playerMovementManager.currentPlayerState != PlayerState.Dashing)
        {
            return;
        }

        if (Vector2.Distance(transform.position, dashDestination) < dashEndThreshold)
        {
            DashEnded();
        }

        playerMovementManager.SetPlayerVelocity(dashStartingVelocity + (dashDirection * dashSpeed));
    }
}
