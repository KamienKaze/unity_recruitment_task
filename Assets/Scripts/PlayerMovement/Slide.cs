using UnityEngine;

public class Slide : MovementExtension
{
    // Editor Settings
    #region
    [Header("Settings")]
    [SerializeField]
    private float slideVelocityDecreaseSpeed = 1f;

    [SerializeField]
    private float slideEndVelocity = 0.5f;
    #endregion

    void Start()
    {
        playerMovementManager.slideStart += SlideStarted;
        playerMovementManager.slideEnd += SlideEnded;
    }

    private void FixedUpdate()
    {
        HandleSlide();
    }

    private void SlideStarted()
    {
        playerMovementManager.currentPlayerState = PlayerState.Sliding;
    }

    private void SlideEnded()
    {
        if (playerMovementManager.currentPlayerState != PlayerState.Sliding)
        {
            return;
        }

        playerMovementManager.SetPlayerVelocity(Vector2.zero);
        playerMovementManager.currentPlayerState = PlayerState.Basic;
    }

    private void HandleSlide()
    {
        if (playerMovementManager.currentPlayerState != PlayerState.Sliding)
        {
            return;
        }

        playerMovementManager.SetPlayerVelocity(
            new Vector2(
                Mathf.Lerp(currentPlayerVelocity.x, 0, Time.deltaTime * slideVelocityDecreaseSpeed),
                Mathf.Lerp(currentPlayerVelocity.y, 0, Time.deltaTime * slideVelocityDecreaseSpeed)
            )
        );

        if (currentPlayerVelocity.magnitude < slideEndVelocity)
        {
            SlideEnded();
        }
    }
}
