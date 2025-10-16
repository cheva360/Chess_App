using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class player1 : MonoBehaviour
{
    // Movement & Dash
    private float speed = 1f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashCooldown = 1f;

    // Components & Prefabs
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    public CooldownBar dashCooldownBar;
    public player1arrow arrowIndicator;

    [Header("Attack Settings")]
    public GameObject attackPrefab; // The hitbox prefab you created
    public float attackOffset = 0.5f; // How far in front of the player it spawns

    [Header("Health Settings")]
    public int maxHealth = 2;
    private int currentHealth;
    public TextMeshPro healthText; // The text object for health display
    private bool isDead = false;

    private bool hasReactivated = false;

    public Vector2 spawnPoint; // Assign this in the Inspector or at runtime


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // Get the player's collider
        lastMoveDirection = Vector2.right;

        // Initialize Health
        currentHealth = maxHealth;
        UpdateHealthText();
        Debug.Log("activated");
    }

    void Update()
    {
        // Stop all actions if dead
        //if (isDead) return;

        // Flip the sprite based on horizontal movement
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        // Return if the player is dead
        //if (isDead) return;

        // Movement logic should only run when the player is not dashing.
        // The dash velocity is handled exclusively within the PlayerDash coroutine.
        if (!isDashing)
        {
            // Check if there is any movement input.
            // Using sqrMagnitude is more efficient than magnitude as it avoids a square root calculation.
            if (moveInput.sqrMagnitude > 0.01f)
            {
                // Apply movement velocity
                rb.linearVelocity = moveInput * speed;
            }
            else
            {
                // If there is no movement input, explicitly set velocity to zero to stop the player.
                // This ensures the player stops completely when input ceases.
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        //if (isDead) return;
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //if (isDead) return;

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

        // --- SPAWN ATTACK HITBOX ---
        if (attackPrefab != null)
        {
            // Calculate spawn position in front of the player
            Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * attackOffset;
            GameObject attackInstance = Instantiate(attackPrefab, spawnPos, Quaternion.identity);

            // Tell the hitbox who created it to avoid self-damage
            attackInstance.GetComponent<AttackHitbox>().owner = this.gameObject;

            // The hitbox will only exist for the duration of the dash
            Destroy(attackInstance, dashTime);
        }

        if (dashCooldownBar != null)
        {
            dashCooldownBar.StartCooldown(dashCooldown);
        }

        if (arrowIndicator != null)
        {
            arrowIndicator.PlayAttackEffect();
        }

        rb.linearVelocity = lastMoveDirection * dashPower;
        yield return new WaitForSeconds(dashTime);
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void TakeDamage(int damage)
    {
        //if (isDead) return; // Can't damage a dead player

        currentHealth -= damage;
        UpdateHealthText();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }


    void Die()
    {
        //isDead = true;

        //// Make the player invisible and unable to collide with anything
        //spriteRenderer.enabled = false;
        //playerCollider.enabled = false;

        //// Make the health text invisible too
        //if (healthText != null)
        //{
        //    healthText.gameObject.SetActive(false);
        //}

        //// Stop all movement
        //rb.linearVelocity = Vector2.zero;



        foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.tag == "Untagged" && obj.scene.IsValid())
            {
                obj.SetActive(true);
            }
            if (obj.CompareTag("Player"))
            {
                obj.SetActive(false);
            }
        }

        hasReactivated = false; // Reset flag so ReactivatePlayer can be called next time
    }

    void OnEnable()
    {
        // Reset dash state
        canDash = true;
        isDashing = false;

        // Reset health
        currentHealth = maxHealth;
        UpdateHealthText();
        isDead = false;

        // Teleport to spawn point
        transform.position = spawnPoint;

        // Re-enable visuals and collider
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;
        if (healthText != null) healthText.gameObject.SetActive(true);

        // Stop all movement
        if (rb != null) rb.linearVelocity = Vector2.zero;
        
    }

    
}

