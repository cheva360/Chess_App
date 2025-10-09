using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class player1arrow : MonoBehaviour
{

    public float speed = 5f;
    public Rigidbody2D P1rb;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * speed;
        // 1. Calculate the direction from your object to the target.
        // Note we use Vector2 for positions.
        Vector2 direction = P1rb.position - rb.position;

        // 2. Calculate the angle in degrees from the direction.
        // Atan2 gives us the angle from the x-axis to the direction vector.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.SetRotation(angle + 90);

    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    // gets player attack keyboard input and employs PlayerAttack Coroutine
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Start the coroutine instead of putting the logic here
            StartCoroutine(PlayerAttack());
        }
    }   
    // player attack coroutine
    private IEnumerator PlayerAttack()
    {
        sr.color = Color.yellow;
        Debug.Log("space");
        yield return new WaitForSeconds(.3f);
        sr.color = Color.white;
    }
}
