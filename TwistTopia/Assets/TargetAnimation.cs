using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetAnimation : MonoBehaviour
{
    public Transform flagWorld; // Reference to the world space flag (the actual flag GameObject)
    public Image flagUI; // Reference to the UI Image that represents the flag
    public float transitionDuration = 2f;
    public Transform topBarTarget;
    public Camera camera;
    
    void Start()
    {
        StartCoroutine(BounceFlag());
    }

    private IEnumerator BounceFlag()
    {
        // Bounce the actual flag GameObject in the world
        Vector3 originalScale = flagWorld.localScale;
        Vector3 bounceScale = originalScale * 1.5f; 

        // Bounce the flag
        for (int i = 0; i < 3; i++) 
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
        
        // move the flag UI to the top bar
        // Vector2 screenPos = camera.WorldToScreenPoint(flagWorld.position);
        // flagUI.rectTransform.position = screenPos;
        flagUI.gameObject.SetActive(true);
        Vector3 topBarTargetPosition = topBarTarget.GetComponent<RectTransform>().position;
        yield return StartCoroutine(MoveFlagToTopBar(flagUI.gameObject, topBarTargetPosition, transitionDuration));
    }

    private IEnumerator MoveFlagToTopBar(GameObject objectToMove, Vector3 end, float seconds)
    {
        RectTransform rectTransform = objectToMove.GetComponent<RectTransform>();
        Vector3 startingPos = rectTransform.position;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            rectTransform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        rectTransform.position = end;
        flagUI.gameObject.SetActive(false);
    }
  
    
    // // After bouncing, set the UI Image to the current screen position of the flag
    // Vector2 screenPos = Camera.main.WorldToScreenPoint(flagWorld.position);
    // flagUI.rectTransform.position = screenPos;
    //
    // // Optionally wait before starting the move to top bar animation
    // yield return new WaitForSeconds(1f);
    //
    // // Start the UI move animation
    // flagAnimator.SetTrigger("MoveToTopBar");

    // Additional methods may be added here to handle the end of the top bar animation,
    // such as fading out the flag UI or starting a new UI indicator for the flag's location.
}
