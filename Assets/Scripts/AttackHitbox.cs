using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    // This will be set by the player who creates the hitbox
    // so the player doesn't hit themselves.
    public GameObject owner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("attacked!");

        // We only care if we hit an object tagged as "Player"
        if (other.CompareTag("Player"))
        {

            // Make sure the player we hit is not the one who attacked
            if (other.gameObject != owner)
            {
                player1 opponent = other.GetComponent<player1>();
                if (opponent != null)
                {
                    // Tell the other player to take 1 damage
                    opponent.TakeDamage(1);
                }

                // Destroy the hitbox immediately after it lands a hit
                // to prevent it from hitting multiple times.
                Destroy(gameObject);
            }
        }
    }
}