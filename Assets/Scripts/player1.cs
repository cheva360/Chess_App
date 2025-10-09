using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class player1 : MonoBehaviour
{
    private float speed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    // --- New Variables ---
    private Vector2 lastMoveDirection; // To store the last direction we moved
    private bool isDashing = false; // State to check if we are currently dashing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMoveDirection = Vector2.right; // Default direction (e.g., facing right)
    }

    // Use FixedUpdate for physics
    void FixedUpdate()
    {
        // Only allow normal movement if the player is NOT dashing
        if (!isDashing)
        {
            rb.linearVelocity = moveInput * speed;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // If the player is providing movement input, update the last direction
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        // Start the dash coroutine when the button is pressed
        if (context.performed && !isDashing)
        {
            StartCoroutine(PlayerDash());
        }
    }

    // This coroutine now handles the dash logic
    private IEnumerator PlayerDash()
    {
        isDashing = true;
        float dashPower = 8f; // The speed/power of the dash
        float dashTime = 0.3f; // How long the dash lasts

        // Apply a strong, instant velocity in the last known direction
        rb.linearVelocity = lastMoveDirection * dashPower;

        // Wait for the dash duration
        yield return new WaitForSeconds(dashTime);

        // Stop the dash
        rb.linearVelocity = Vector2.zero; // Stop the player immediately after dashing
        isDashing = false;
    }
}