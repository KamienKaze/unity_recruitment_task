using UnityEngine;

[RequireComponent(typeof(PlayerMovementManager))]
public class MovementExtension : MonoBehaviour
{
    protected PlayerMovementManager playerMovementManager;

    protected Vector2 currentPlayerPosition;
    protected Vector2 currentPlayerVelocity;
    protected Vector2 currentCursorDirection;

    private void Awake()
    {
        playerMovementManager = gameObject.GetComponent<PlayerMovementManager>();
    }

    private void Update()
    {
        GetPlayerVariables();
    }

    private void GetPlayerVariables()
    {
        currentPlayerPosition = playerMovementManager.GetCurrentPosition();
        currentPlayerVelocity = playerMovementManager.GetCurrentVelocity();
        currentCursorDirection = playerMovementManager.GetCurrentCursorDirection();
    }
}
