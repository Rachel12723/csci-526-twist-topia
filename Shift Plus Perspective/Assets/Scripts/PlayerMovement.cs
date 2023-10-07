using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	// private int Horizontal = 0;
	// public Animator animator;
	// public float moveSpeed = 5.0f;
	// private float degree = 0;
	//
	// void Update () {
	//     if (Input.GetAxis ("Horizontal") < 0)
	//         Horizontal = -1;
	//     else if (Input.GetAxis ("Horizontal") > 0)
	//         Horizontal = 1;
	//     else
	//         Horizontal = 0;
	//
	//     if(animator)
	//     {
	//         animator.SetInteger("Horizontal", Horizontal);
	//
	//         float horizontal = Input.GetAxis("Horizontal");
	//         float vertical = Input.GetAxis("Vertical");
	//
	//         Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
	//
	//         transform.Translate(movement * moveSpeed * Time.deltaTime);
	//     }
	//     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8);
	// }
	
	private int Horizontal = 0;

	public Animator animator;
	public float movementSpeed = 5f;
	public float gravity = 1f;
	public CharacterController charController;
	private FacingDirection _myFacingDirection;
	public float JumpHeight = 0f;
	public bool _jumping = false;
	private float degree = 0;


	public FacingDirection CmdFacingDirection {
		set{ _myFacingDirection = value; }
	}
	
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
			float moveFactor = movementSpeed * 10f;
			MoveCharacter(moveFactor);
			
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);
	}

	private void MoveCharacter(float moveFactor)
	{
		Vector3 trans = Vector3.zero;
		if(_myFacingDirection == FacingDirection.Front)
		{
			trans = new Vector3(Horizontal* moveFactor, -gravity * moveFactor, 0f);
		}
		else if(_myFacingDirection == FacingDirection.Right)
		{
			trans = new Vector3(0f, -gravity * moveFactor, Horizontal* moveFactor);
		}
		else if(_myFacingDirection == FacingDirection.Back)
		{
			trans = new Vector3(-Horizontal* moveFactor, -gravity * moveFactor, 0f);
		}
		else if(_myFacingDirection == FacingDirection.Left)
		{
			trans = new Vector3(0f, -gravity * moveFactor, -Horizontal* moveFactor);
		}
		charController.SimpleMove(trans);
	}
	
	public void UpdateToFacingDirection(FacingDirection newDirection, float angle)
	{
		_myFacingDirection = newDirection;
		degree = angle;
	}
}