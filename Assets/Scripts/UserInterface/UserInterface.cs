using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private TMP_Text velocityMeter;

    [SerializeField]
    private TMP_Text healthBar;

    [SerializeField]
    private TMP_Text dashBar;

    [SerializeField]
    private PlayerMovementManager playerMovementManager;

    [SerializeField]
    private Dash dash;

    private void Update()
    {
        UpdateUserInterface();
    }

    private void UpdateUserInterface()
    {
        if (playerMovementManager.IsDestroyed())
        {
            return;
        }

        velocityMeter.text =
            "Velocity: "
            + Math.Round(playerMovementManager.GetCurrentVelocity().magnitude * 100) / 100;
        healthBar.text = "Health: " + playerMovementManager.GetPlayerHealth();
        dashBar.text = "Dash: " + dash.currentDashCharges;
    }
}
