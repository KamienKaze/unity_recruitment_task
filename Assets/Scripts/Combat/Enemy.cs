using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D enemyRigidbody;

    [SerializeField]
    private int enemyHealth = 5;

    [SerializeField]
    private int enemyDamage = 1;

    [SerializeField]
    private float moveSpeed = 3f;

    private void Update()
    {
        CheckForDeath();
        Debug.Log(enemyHealth);
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 moveVelocity = player.transform.position - transform.position;
        enemyRigidbody.velocity = moveVelocity.normalized * moveSpeed;
    }

    private void CheckForDeath()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void RecieveDamage(int damage)
    {
        enemyHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
        {
            return;
        }

        PlayerMovementManager playerMovementManager =
            collider.GetComponent<PlayerMovementManager>();

        Dash playerDash = collider.GetComponent<Dash>();

        if (playerMovementManager.currentPlayerState == PlayerState.Dashing)
        {
            RecieveDamage(
                (int)
                    Mathf.Floor(
                        playerMovementManager.GetCurrentVelocity().magnitude
                            * playerDash.GetDashDamageMultiplier()
                    )
            );
        }

        if (enemyHealth <= 0)
        {
            playerDash.AddDashCharges(playerDash.GetDashChargesAfterKill());
            return;
        }

        playerMovementManager.UpdatePlayerHealth(-enemyDamage);
    }
}
