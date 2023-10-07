using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private int Horizontal = 0;
    public Animator animator;
    public float moveSpeed = 5.0f;
    private float degree = 0;
    
    // Update is called once per frame
    void Update () {
        if (Input.GetAxis ("Horizontal") < 0)
            Horizontal = -1;
        else if (Input.GetAxis ("Horizontal") > 0)
            Horizontal = 1;
        else
            Horizontal = 0;

        if(animator)
        {
            animator.SetInteger("Horizontal", Horizontal);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

            transform.Translate(movement * moveSpeed * Time.deltaTime);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8);
    }
}