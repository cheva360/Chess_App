using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class player1 : MonoBehaviour
{
    private float speed = 1f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashCooldown = 1f;
    public CooldownBar dashCooldownBar;

    // --- NEW VARIABLE ---
    // Add a public reference to the arrow script
    public player1arrow arrowIndicator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMoveDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = moveInput * speed;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        // This remains the single point of control for the attack/dash cooldown
        if (context.performed && canDash)
        {
            StartCoroutine(PlayerDash());
        }
    }

    private IEnumerator PlayerDash()
    {
        canDash = false;
        isDashing = true;
        float dashPower = 4f;
        float dashTime = 0.3f;

        // --- TRIGGER VISUALS (UPDATED) ---
        // Tell the cooldown bar to start
        if (dashCooldownBar != null)
        {
            dashCooldownBar.StartCooldown(dashCooldown);
        }

        // Tell the arrow to play its visual effect
        if (arrowIndicator != null)
        {
            arrowIndicator.PlayAttackEffect();
        }

        // Dash Physics
        rb.linearVelocity = lastMoveDirection * dashPower;
        yield return new WaitForSeconds(dashTime);
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        // Cooldown Timer
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

