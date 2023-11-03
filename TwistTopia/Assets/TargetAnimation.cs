using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetAnimation : MonoBehaviour
{
    public Transform flagWorld; // Reference to the world space flag (the actual flag GameObject)
    public Image flagUI; // Reference to the UI Image that represents the flag
    public Animator flagAnimator; // Reference to the Animator component on the flagUI
    // public Canvas canvas; // Reference to the Canvas where the UI Image is placed

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to bounce the flag in the world
        StartCoroutine(BounceFlag());
    }

    private IEnumerator BounceFlag()
    {
        // Bounce the actual flag GameObject in the world
        Vector3 originalScale = flagWorld.localScale;
        Vector3 bounceScale = originalScale * 1.5f; // Increase the scale by 20% for the bounce effect

        // Bounce the flag
        for (int i = 0; i < 3; i++) // Let's say it bounces 3 times
        {
            // Scale up
            float elapsedTime = 0f;
            while (elapsedTime < 0.25f) // Takes 0.25 seconds to bounce up
            {
                flagWorld.localScale = Vector3.Lerp(originalScale, bounceScale, (elapsedTime / 0.25f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Scale down
            elapsedTime = 0f;
            while (elapsedTime < 0.25f) // Takes 0.25 seconds to bounce down
            {
                flagWorld.localScale = Vector3.Lerp(bounceScale, originalScale, (elapsedTime / 0.25f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        // // After bouncing, set the UI Image to the current screen position of the flag
        // Vector2 screenPos = Camera.main.WorldToScreenPoint(flagWorld.position);
        // flagUI.rectTransform.position = screenPos;

        // Optionally wait before starting the move to top bar animation
        // yield return new WaitForSeconds(1f);

        // Start the UI move animation
        // flagAnimator.SetTrigger("MoveToTopBar");
    }

    // Additional methods may be added here to handle the end of the top bar animation,
    // such as fading out the flag UI or starting a new UI indicator for the flag's location.
}
