using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// create invisible cubes that the player can move on to fake the player being on a 2D platform.
// then move the player's depth to the closest platform 

public class PlayerMovementManager : MonoBehaviour { 
	private PlayerMovement playerMove;
	public FacingDirection facingDirection; //Keeps track of the direction our player is oriented
	public GameObject player; //Access to the player gameObject, for getting spacial coordinates
	// private float degree = 0; //Used to tell the FezMove script how much to rotate 90 or -90 degrees depending on input
	public Transform level; //Access to the Transform containing Level Data - Platform
	public Transform building; //Access to the Transform containing Building Data - Obstacle
	public GameObject invisibleCube;
	private List<Transform> invisibleList = new List<Transform>(); //Holds our invisibleCubes so we can look at their locations or delete them
	private FacingDirection lastFacing; //Keeps track of the facing direction from the last frame, helps prevent us from needlessly re-building the location of our Invisicubes
	private float lastDepth = 0f; //Keeps track of the player depth from the last frame, helps prevent us from needlessly re-building the location of our Invisicubes
	public float worldUnits = 1.000f; //Dimensions of cubes used - so far only tested with 1. This could potentially be updated if cubes of a different size are needed - Note: All cubes must be same size
	// Use this for initialization
	void Start () {
					//Define our facing direction, must be the same as built in inspector
					//Cache our playerMove script located on the player and update our level data (create invisible cubes)
					facingDirection = FacingDirection.Front;
					playerMove = player.GetComponent<PlayerMovement> ();
					UpdateLevelData (true); 
	}
	
	void Update() 
	{
		// Logic to control the player depth
		// If we're on an invisible platform, move to a physical platform, 
		// this comes in handy to make rotating possible
		// Try to move us to the closest platform to the camera, 
		// will help when rotating to feel more natural
		// If we changed anything, update our level data which pertains to our inviscubes
		
		bool updateData = false;
		if (OnInvisiblePlatform())
			if (MovePlayerDepthToClosestPlatform())
				updateData = true;

		if (MoveToClosestPlatformToCamera())
			updateData = true;

		if (updateData)
			UpdateLevelData(false);
		

		// Handle Player input for rotation command
	// 	if (Input.GetKeyDown(KeyCode.RightArrow)) 
	// 	{
	// 		// If we rotate while on an invisible platform we must move to a physical platform
	// 		// If we don't, then we could be standing in mid air after the rotation
	// 		if (OnInvisiblePlatform())
	// 			MovePlayerDepthToClosestPlatform();
	//
	// 		lastfacing = facingDirection;
	// 		facingDirection = RotateDirectionRight();
	// 		degree -= 90f;
	// 		UpdateLevelData(false);
	// 		playerMove.UpdateToFacingDirection(facingDirection, degree);
	// 	} 
	// 	else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
	// 	{
	// 		if (OnInvisiblePlatform())
	// 			MovePlayerDepthToClosestPlatform();
	//
	// 		lastfacing = facingDirection;
	// 		facingDirection = RotateDirectionLeft();
	// 		degree += 90f;
	// 		UpdateLevelData(false);
	// 		playerMove.UpdateToFacingDirection(facingDirection, degree);
	// 	}
	}

	// Destroy current invisible platforms
	// Create new invisible platforms taking into account the
	// player's facing direction and the orthographic view of the 
	// platforms
	private void UpdateLevelData(bool forceRebuild)
	{
		//If facing direction and depth havent changed we do not need to rebuild
		if(!forceRebuild)
			if (lastFacing == facingDirection && lastDepth == GetPlayerDepth())
				return;
		foreach(Transform trans in invisibleList)
		{
			//Move obsolete invisicubes out of the way and delete

			trans.position = Vector3.zero;
			Destroy(trans.gameObject);
			
		}
		invisibleList.Clear();
		float newDepth = 0f;
		newDepth = GetPlayerDepth();
		CreateInvisibleCubesAtNewDepth (newDepth);
	}

	/// Returns true if the player is standing on an invisible platform
	private bool OnInvisiblePlatform()
	{
		foreach(Transform item in invisibleList)
		{
			if(Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits && Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits)
				if(playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0)
					return true;
		}
		return false;
	}

	// Moves the player to the closest platform with the same height to the camera
	// Only supports Unity cubes of size (1x1x1)
	private bool MoveToClosestPlatformToCamera()
	{
		bool moveCloser = false;
		foreach(Transform item in level)
		{
			//if(facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
			//{
			//	//When facing Front, find cubes that are close enough in the x position and the just below our current y value
			//	//This would have to be updated if using cubes bigger or smaller than (1,1,1)
			//	if(Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits +0.1f)
			//	{
					
			//		if(playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y >0)
			//		{
			//			if(facingDirection == FacingDirection.Front && item.position.z < playerMove.transform.position.z)
			//				moveCloser = true;

			//			if(facingDirection == FacingDirection.Back && item.position.z > playerMove.transform.position.z)
			//				moveCloser = true;
						

			//			if(moveCloser)
			//			{

			//				playerMove.transform.position = new Vector3(playerMove.transform.position.x, playerMove.transform.position.y, item.position.z);
			//				return true;
			//			}
			//		}

			//	}
				
			//}
			//else{
			//	if(Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits + 0.1f)
			//	{
			//		if(playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y >0)
			//		{
			//			if(facingDirection == FacingDirection.Right && item.position.x > playerMove.transform.position.x)
			//				moveCloser = true;

			//			if(facingDirection == FacingDirection.Left && item.position.x < playerMove.transform.position.x)
			//				moveCloser = true;

			//			if(moveCloser)
			//			{
			//				playerMove.transform.position = new Vector3(item.position.x, playerMove.transform.position.y, playerMove.transform.position.z);
			//				return true;
			//			}

			//		}

			//	}
			//}

			
		}
		return false;
	}

	
	// Looks for an invisibleCube in InvisibleList at position 'cube'
	private bool FindTransformInvisibleList(Vector3 cube)
	{
		foreach(Transform item in invisibleList)
		{
			if(item.position == cube)
				return true;
		}
		return false;

	}


	// Looks for a physical (visible) cube in our level data at position 'cube'
	private bool FindTransformLevel(Vector3 cube)
	{
		foreach(Transform item in level)
		{
			if(item.position == cube)
				return true;
		}
		return false;
	}
	
	/// Determines if any building cubes are between the "cube" and the camera
	private bool FindTransformBuilding(Vector3 cube)
	{
		foreach(Transform item in building)
		{
			//if(facingDirection == FacingDirection.Front )
			//{
			//	if(item.position.x == cube.x && item.position.y == cube.y && item.position.z < cube.z)
			//		return true;
			//}
			//else if(facingDirection == FacingDirection.Back )
			//{
			//	if(item.position.x == cube.x && item.position.y == cube.y && item.position.z > cube.z)
			//		return true;
			//}
			//else if(facingDirection == FacingDirection.Right )
			//{
			//	if(item.position.z == cube.z && item.position.y == cube.y && item.position.x > cube.x)
			//		return true;
			//}
			//else
			//{
			//	if(item.position.z == cube.z && item.position.y == cube.y && item.position.x < cube.x)
			//		return true;
			//}
		}
		return false;
	}

	// Moves player to closest platform with the same height
	// Intended to be used when player jumps onto an invisible platform
	private bool MovePlayerDepthToClosestPlatform()
	{
		foreach(Transform item in level)
		{
			//if(facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
			//{
			//	if(Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits + 0.1f)
			//		if(playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0)
			//		{
			//			playerMove.transform.position = new Vector3(playerMove.transform.position.x, playerMove.transform.position.y, item.position.z);
			//			return true;
			//		}
			//}
			//else
			//{
			//	if(Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits + 0.1f)
			//		if(playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0)
			//		{
			//			playerMove.transform.position = new Vector3(item.position.x, playerMove.transform.position.y, playerMove.transform.position.z);
			//			return true;
			//		}
			//}
		}
		return false;
	}
	
	// Creates an invisible cube at position
	private Transform CreateInvisibleCube(Vector3 position)
	{
		GameObject go = Instantiate (invisibleCube) as GameObject;
		go.transform.position = position;
		return go.transform;
	}
	//

	// Creates invisible cubes for the player to move on if the physical cubes that make up a platform are on a different depth
	private void CreateInvisibleCubesAtNewDepth(float newDepth)
	{

		Vector3 tempCube = Vector3.zero;
		foreach(Transform child in level)
		{
			//if(facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
			//{
			//	tempCube = new Vector3(child.position.x, child.position.y, newDepth);
			//	if(!FindTransformInvisibleList(tempCube) && !FindTransformLevel(tempCube) && !FindTransformBuilding(child.position))
			//	{
			//		Transform go = CreateInvisibleCube(tempCube);
			//		invisibleList.Add(go);
			//	}
			//}
			////z and y must match a level cube
			//else if(facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
			//{
			//	tempCube = new Vector3(newDepth, child.position.y, child.position.z);
			//	if (!FindTransformInvisibleList(tempCube) && !FindTransformLevel(tempCube) &&
			//	    !FindTransformBuilding(child.position))
			//	{
			//		Transform go = CreateInvisibleCube(tempCube);
			//		invisibleList.Add(go);
			//	}
			//}
		}
	}

	public void ReturnToStart()
	{
		UpdateLevelData (true);
	}

	/// Returns the player depth. Depth is how far from or close you are to the camera
	/// If we're facing Front or Back, this is Z
	/// If we're facing Right or Left it is X
	/// 

	/// The player depth.
	private float GetPlayerDepth()
	{
		float ClosestPoint = 0f;

		//if(facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
		//{
		//	ClosestPoint = playerMove.transform.position.z;
				
		//}
		//else if(facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
		//{
		//	ClosestPoint = playerMove.transform.position.x;
		//}
		return Mathf.Round(ClosestPoint);

	}


				/// 

				/// Determines the facing direction after we rotate to the right
				/// 

				/// The direction right.
	private FacingDirection RotateDirectionRight()
	{
		int change = (int)(facingDirection);
		change++;
								//Our FacingDirection enum only has 4 states, if we go past the last state, loop to the first
		if (change > 3)
			change = 0;
		return (FacingDirection) (change);
	}
				/// 

				/// Determines the facing direction after we rotate to the left
				/// 

				/// The direction left.
	private FacingDirection RotateDirectionLeft()
	{
		int change = (int)(facingDirection);
		change--;
								//Our FacingDirection enum only has 4 states, if we go below the first, go to the last state
		if (change < 0)
			change = 3;
		return (FacingDirection) (change);
	}

}

//Used frequently to keep track of the orientation of our player and camera
