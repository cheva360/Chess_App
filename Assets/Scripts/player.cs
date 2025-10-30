using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class player1 : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;

    

    int matrixX;
    int matrixY;
    // Movement & Dash
    private float speed = 1f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private bool isDashing = false;
    private bool canDash = true;
    public float dashCooldown = 1f;
    public float dashTime = 0.4f;

    // Components & Prefabs
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    public CooldownBar dashCooldownBar;
    public player1arrow arrowIndicator;
    public int playerNumber = 1; // Set this in the Inspector to 1 or 2

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
        //Debug.Log("activated");
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
        if (dashCooldownBar != null)
        {
            // Offset by 1 unit below the player
            dashCooldownBar.transform.position = transform.position + new Vector3(0, -.6f, 0);
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
        

        GameObject attackInstance = null;

        // --- SPAWN ATTACK HITBOX ---
        if (attackPrefab != null)
        {
            // Calculate initial spawn position in front of the player
            Vector2 spawnPos = (Vector2)transform.position + lastMoveDirection * attackOffset;
            attackInstance = Instantiate(attackPrefab, spawnPos, Quaternion.identity);

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

        float elapsed = 0f;
        Vector2 dashDirection = lastMoveDirection;
        while (elapsed < dashTime)
        {
            if (attackInstance != null)
            {
                // Keep the hitbox in front of the player during the dash
                attackInstance.transform.position = (Vector2)transform.position + dashDirection * attackOffset;
                
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

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

    //public void OnMouseUp()
    //{
    //    controller = GameObject.FindGameObjectWithTag("GameController");
    //    Game game = controller.GetComponent<Game>();
    //    Chessman chessman = reference.GetComponent<Chessman>();
    //    GameObject cp = game.GetPosition(matrixX, matrixY);

    //}

    void Die()
    {

        //controller = GameObject.FindGameObjectWithTag("GameController");
        //Game game = controller.GetComponent<Game>();
        //Chessman chessman = reference.GetComponent<Chessman>();
        //    GameObject cp = game.GetPosition(matrixX, matrixY);


        //deathlogic
        DeathLogic deathLogic = controller.GetComponent<DeathLogic>();
        
        if (playerNumber == 1)
        {
            deathLogic.attacker = "white";
            deathLogic.attackerIsWhite = true;
            deathLogic.target = "black";
            deathLogic.attackdead = true;
        }
        else
        {
            deathLogic.attacker = "black";
            deathLogic.attackerIsWhite = false;
            deathLogic.target = "white";
            deathLogic.targetdead = true;

        }

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

        ////Set the Chesspiece's original location to be empty
        //game.SetPositionEmpty(chessman.GetXBoard(),
        //chessman.GetYBoard());

        ////Move reference chess piece to this position
        //chessman.SetXBoard(matrixX);
        //chessman.SetYBoard(matrixY);
        //chessman.SetCoords();


        ////Update the matrix
        //game.SetPosition(reference);
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
        
        //dashCooldownBar.transform.position = transform.position;


        // Re-enable visuals and collider
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;
        if (healthText != null) healthText.gameObject.SetActive(true);

        // Stop all movement
        if (rb != null) rb.linearVelocity = Vector2.zero;


        

    }



    
}

