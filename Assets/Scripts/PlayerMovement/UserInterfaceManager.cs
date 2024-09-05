using System;
using TMPro;
using UnityEngine;

public class UserInterfaceManager : MovementExtension
{
    [SerializeField]
    private TMP_Text velocityMeter;

    private void FixedUpdate()
    {
        velocityMeter.text =
            "Velocity: "
            + Math.Round(playerMovementManager.GetCurrentVelocity().magnitude * 100) / 100;
    }
}
