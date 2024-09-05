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
        playerController.slideStart += SlideStarted;
        playerController.slideEnd += SlideEnded;
    }

    private void FixedUpdate()
    {
        HandleSlide();
    }

    private void SlideStarted()
    {
        playerController.currentPlayerState = PlayerState.Sliding;
    }

    private void SlideEnded()
    {
        if (playerController.currentPlayerState != PlayerState.Sliding)
        {
            return;
        }

        playerController.SetPlayerVelocity(Vector2.zero);
        playerController.currentPlayerState = PlayerState.Basic;
    }

    private void HandleSlide()
    {
        if (playerController.currentPlayerState != PlayerState.Sliding)
        {
            return;
        }

        playerController.SetPlayerVelocity(
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
