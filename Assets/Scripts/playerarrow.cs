using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class player1arrow : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D P1rb; // The player's Rigidbody2D to point towards
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        rb.linearVelocity = moveInput * speed;

        // Make sure the player Rigidbody is assigned in the Inspector
        if (P1rb != null)
        {
            // Points the arrow towards the player's position
            Vector2 direction = P1rb.position - rb.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.SetRotation(angle + 90);
        }
    }

    // This method can still be used for other inputs if needed
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // --- METHOD HAS BEEN REMOVED ---
    // The Attack(InputAction.CallbackContext context) method has been removed.
    // This script no longer listens for the attack input directly.

    // This new public method is called BY THE PLAYER SCRIPT to start the visual effect.
    public void PlayAttackEffect()
    {
        StartCoroutine(AttackEffectCoroutine());
    }

    // The coroutine that handles the visual effect (renamed for clarity).
    private IEnumerator AttackEffectCoroutine()
    {
        sr.color = Color.yellow;
        // The duration of the visual effect
        yield return new WaitForSeconds(0.3f);

        sr.color = Color.white;
    }

    private void OnEnable()
    {
        sr.color = Color.white;
    }
}
