// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class PlayerFrame : MonoBehaviour
// {
//     // player action keyCode
//     public KeyCode dropOffFrameCode;
//
//     // pick up key and open blocks
//     public Transform blocks;
//     //public Transform keys;
// 	public Transform keypons;
// 	public GameObject keyponInHand;
// 	private GameObject inHandKeypon;
//     private int keyCounter = 0;
//     public UnityEngine.UI.Text keyText;
//     public CameraState cameraState;
//     public DirectionManager directionManager;
//     public float WorldUnit = 1.000f;
// 	public Transform enemies;
//     private EnemyManager enemyManager;
// 	private float xOffset = 1.2f;
//     private float zOffset = -0.56f;
//     private float rotationSpeed = 1800f;
// 	private bool hasCompletedFullRotation = false;
// 	private float rotationProgress = 0.0f;
//     // Start is called before the first frame update
//     void Start()
//     {
//         if (enemies != null)
//         {
//             enemyManager = enemies.GetComponent<EnemyManager>();
//         }
//         else
//         {
//             Debug.Log("KeyAndDoor.cs: No enemies!");
//         }
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         keyText.text = "Key: " + keyCounter;
//         //PickUpKey();
// 		PickUpKeypon();
//         if (Input.GetKeyDown(openDoorCode) && keyCounter > 0)
//         {
//             //OpenDoor();
// 			SlashAndOpen();
//         }
// 		if(hasCompletedFullRotation)
// 		{
// 			// Calculate the rotation angle for this frame
//    			 float rotationAngle = rotationSpeed * Time.deltaTime;
//    			// Increment the rotation progress
//     		rotationProgress += rotationAngle;
//     		// Rotate the object around the y-axis
//     		transform.Rotate(Vector3.up, rotationAngle);
// 			float radius = 1.7f; 
// 			Vector3 playerPosition = transform.position;
// 			//foreach(Transform enemy in player.GetComponent<PlayerMovement>().enemies){
// 			if(enemies != null){
// 				foreach(Transform enemy in enemies)
// 				{ 
// 					//Transform enemyModel = enemy.Find("EnemyModel");
// 					foreach(Transform enemyInstance in enemy)
// 					{
// 						//Vector3 enemyPosition = enemyModel.position;
// 						Vector3 enemyPosition = enemyInstance.position;
//     					float distanceX = enemyPosition.x - playerPosition.x;
//     					float distanceZ = enemyPosition.z - playerPosition.z;
//     					float distance = Mathf.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
//     					if (distance <= radius)
//     					{
//                             //Destroy(enemyInstance.gameObject);
//                             enemyManager.DestoryEnemy(enemyInstance);
// 							/*
//         					Destroy(enemy.gameObject);
// 							keyCounter--;
// 							KeyponDestroyed();
// 							*/
//     					}
// 					}
// 				}
// 			}
//     		// Check if the rotation has completed (3 full rotations, 1080 degrees)
//     		if (rotationProgress >= 1080.0f)
//     		{
// 				Vector3 targetRotation = new Vector3(0f, 0f, 0f);
// 				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime * 4);
// 				hasCompletedFullRotation = false;
//         		rotationProgress = 0.0f;
//     		}
// 		}
// 		KeyponDestroyed();
//     }
// 	
// 	private void PickUpKeypon()
//     {
//         if (keypons != null)
//         {
//             if (cameraState.GetFacingDirection() == FacingDirection.Front)
//             {
//                 foreach (Transform keypon in keypons)
//                 {
//                     if (Mathf.Abs(keypon.position.y - transform.position.y) < WorldUnit + 0.5f &&
//                         Mathf.Abs(keypon.position.x - transform.position.x) < WorldUnit + 0.5f)
//                     {
// 					
//                         Destroy(keypon.gameObject);
// 					    inHandKeypon = Instantiate(keyponInHand, transform);
//         			    inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
//                         keyCounter++;
//                         Debug.Log("Keys:" + keyCounter);
//                         break;
//                     }
//                 }
//             }
//             else if (cameraState.GetFacingDirection() == FacingDirection.Up)
//             {
//                 foreach (Transform keypon in keypons)
//                 {
//                     if (Mathf.Abs(keypon.position.z - transform.position.z) < WorldUnit + 0.25f &&
//                         Mathf.Abs(keypon.position.x - transform.position.x) < WorldUnit + 0.25f)
//                     {
//                         Destroy(keypon.gameObject);
// 					    inHandKeypon = Instantiate(keyponInHand, transform);
//         			    inHandKeypon.transform.localPosition = new Vector3(xOffset, 0, zOffset);
//                         keyCounter++;
//                         Debug.Log("Keys:" + keyCounter);
//                         break;
//                     }
//                 } 
//             }
//
//         }
//     }
//
// 	private void FrontViewOpenDoor(){
// 		foreach (Transform block in blocks)
//         {
// 			bool canOpen = false;
//             foreach (Transform blockCube in block)
//             {
// 				if (Mathf.Abs(blockCube.position.y - transform.position.y) < WorldUnit + 0.5f &&
//                 Mathf.Abs(blockCube.position.x - transform.position.x) < WorldUnit + 0.5f)
//             	{
// 					canOpen = true;
//                     break;
//                 }
//             }
//             if (canOpen)
//             {
//                 directionManager.DeleteBlockCubes(block);
//                 Destroy(block.gameObject);
//                 keyCounter--;
// 				KeyponDestroyed();
//                 Debug.Log("Keys:" + keyCounter);
//                 break;
//             }
//         }
// 	}
// 	private void KeyponDestroyed()
// 	{
// 		if(keyCounter == 0 && inHandKeypon!=null)
// 		{
// 			Destroy(inHandKeypon);
// 		}
// 	}
//
//     public void KeyDrop(){
//         if(keyCounter > 0){
//             keyCounter--;
//             Debug.Log("Oops! Be careful! " + keyCounter);
//         }
//     }
//
//     public int GetKeyCounter()
//     {
//         return keyCounter;
//     }
// }
