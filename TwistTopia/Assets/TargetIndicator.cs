using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour
{
    public Transform target;  // The target to point to
    // public Transform player;  // The player's transform
    // public float offScreenOffset = 10f;  // Distance from player to consider target as off-screen
    public Camera playerCamera;
    public RectTransform arrowRectTransform;  // The RectTransform component of the arrow
    public bool tutorialOne = false;

    // private void Update()
    // {
    //     float directionToTargetX = target.position.x - player.position.x;
    //     Debug.Log("-------------------------"+directionToTargetX);
    //     
    //     // Check if target is considered off-screen based on the offset
    //     if (Mathf.Abs(directionToTargetX) > offScreenOffset)
    //     {
    //         // Target is considered off-screen
    //         arrowRectTransform.gameObject.SetActive(true);
    //         Debug.Log("+++++++++++++off screen set active");
    //
    //         // Flip the arrow based on the direction to the target
    //         if (directionToTargetX < 0)
    //         {
    //             // Target is to the left of the player
    //             Debug.Log("+++++++++++=is left");
    //             arrowRectTransform.localScale = new Vector3(-1, 1, 1); // Flip left
    //         }
    //         else
    //         {
    //             // Target is to the right of the player
    //             Debug.Log("++++++++++++is right");
    //             arrowRectTransform.localScale = new Vector3(1, 1, 1); // Flip right
    //         }
    //     }
    //     else
    //     {
    //         // Target is considered on-screen
    //         Debug.Log("++++++++++++++++on screen deactive");
    //         arrowRectTransform.gameObject.SetActive(false);
    //     }
    // }
    private void Update()
    {
        Vector3 targetScreenPos = playerCamera.WorldToScreenPoint(target.position);
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        bool isOnScreen = (targetScreenPos.x >= 0 && targetScreenPos.x <= Screen.width) && 
                          (targetScreenPos.y >= 0 && targetScreenPos.y <= Screen.height);

        if (isOnScreen && tutorialOne == false)
        {
            arrowRectTransform.gameObject.SetActive(false);
        }
        else
        {
            if (targetScreenPos.x < screenCenter.x)
            {
                // Target is to the left of screen center
                arrowRectTransform.localScale = new Vector3(-1, 1, 1); // Flip left
            }
            else
            {
                // Target is to the right of screen center
                arrowRectTransform.localScale = new Vector3(1, 1, 1); // Flip right
            }
            arrowRectTransform.gameObject.SetActive(true);
        }
    }

}
