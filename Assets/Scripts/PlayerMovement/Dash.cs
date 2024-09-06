using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Dash : MovementExtension
{
    #region Settings
    [Header("Settings")]
    [SerializeField]
    private float dashSpeed = 4f;

    [SerializeField]
    private float dashRange = 5f;

    [SerializeField]
    private float dashEndThreshold = 0.3f;

    [SerializeField]
    private float dashRangeMultiplier = 1f;

    [SerializeField]
    private int maxDashCharges = 4;

    [SerializeField]
    private int dashChargesAfterKill = 2;

    [SerializeField]
    private float dashDamageMultiplier = 0.5f;

    [SerializeField]
    private float dashCooldown = 5f;

    #endregion

    private int currentDashCharges;

    #region Vectors
    private Vector2 dashDestination = Vector2.zero;
    private Vector2 dashDirection = Vector2.zero;
    private Vector2 dashStartingVelocity = Vector2.zero;
    #endregion

    void Start()
    {
        currentDashCharges = maxDashCharges;

        playerMovementManager.dashStart += DashStarted;
        playerMovementManager.wallHit += DashEnded;

        StartCoroutine(DashCooldown());
    }

    public void AddDashCharges(int charges)
    {
        if (currentDashCharges + charges > maxDashCharges)
        {
            currentDashCharges = maxDashCharges;
        }
        else
        {
            currentDashCharges += charges;
        }
    }

    public int GetDashChargesAfterKill()
    {
        return dashChargesAfterKill;
    }

    public float GetDashDamageMultiplier()
    {
        return dashDamageMultiplier;
    }

    public int GetCurrentDashCharges()
    {
        return currentDashCharges;
    }

    private void FixedUpdate()
    {
        HandleDash();
    }

    private void DashStarted()
    {
        if (
            playerMovementManager.currentPlayerState == PlayerState.Dashing
            || currentDashCharges <= 0
        )
        {
            return;
        }

        if (playerMovementManager.currentPlayerState == PlayerState.Stunned)
        {
            return;
        }

        currentDashCharges--;
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

        if (playerMovementManager.currentPlayerState == PlayerState.Stunned)
        {
            return;
        }

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

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        AddDashCharges(1);

        StartCoroutine(DashCooldown());
    }
}
