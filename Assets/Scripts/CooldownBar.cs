using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for working with UI elements like Image

public class CooldownBar : MonoBehaviour
{
    private Image cooldownImage;
    private RectTransform rectTransform;
    private float maxWidth;

    void Awake()
    {
        // Get the components from the UI object this script is attached to
        cooldownImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        // Store the original width of the bar
        maxWidth = rectTransform.sizeDelta.x;

        // Start with the bar invisible
        cooldownImage.enabled = false;
    }

    // This public method will be called by the player script to start the cooldown animation
    public void StartCooldown(float cooldownDuration)
    {
        // Stop any previous cooldown animations that might be running
        StopAllCoroutines();
        // Start the new cooldown animation
        StartCoroutine(CooldownTimer(cooldownDuration));
    }

    private IEnumerator CooldownTimer(float duration)
    {
        // Make the bar visible
        cooldownImage.enabled = true;

        float elapsedTime = 0f;

        // Loop until the cooldown is over
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the remaining time percentage (from 1 down to 0)
            float remainingPercentage = 1f - (elapsedTime / duration);

            // Set the width of the bar based on the remaining time
            // sizeDelta is the width and height of a UI element in its RectTransform
            rectTransform.sizeDelta = new Vector2(maxWidth * remainingPercentage, rectTransform.sizeDelta.y);

            // Wait for the next frame
            yield return null;
        }

        // Once the loop is done, make the bar invisible again
        cooldownImage.enabled = false;
    }
}
