using UnityEngine;

[RequireComponent(typeof(PlayerMovementManager))]
public class MovementExtension : MonoBehaviour
{
    protected PlayerMovementManager playerController;

    protected Vector2 currentPlayerPosition;
    protected Vector2 currentPlayerVelocity;
    protected Vector2 currentCursorDirection;

    private void Awake()
    {
        playerController = gameObject.GetComponent<PlayerMovementManager>();
    }

    private void Update()
    {
        GetPlayerVariables();
    }

    private void GetPlayerVariables()
    {
        currentPlayerPosition = playerController.GetCurrentPosition();
        currentPlayerVelocity = playerController.GetCurrentVelocity();
        currentCursorDirection = playerController.GetCurrentCursorDirection();
    }
}
