using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour
{
    // Rotate
    public KeyCode rotateKeyCode;
    public KeyCode pickUpKeyCode;
    public KeyCode openDoorCode;
    private FacingDirection facingDirection = FacingDirection.Front;

    // Player
    public GameObject player;
    private PlayerMovement playerMovement;
    public GameObject playerReturn;

    // Platform Cubes
    public Transform platformCubes;

    // Block Cubes
    public Transform blockCubes;
    public List<Transform> blockList = new List<Transform>();

    // Invisible Cubes
    public GameObject invisibleCube;
    public Transform invisibleCubes;
    public float invisibleCubesOffsetY = 50f;
    private List<Transform> invisibleList = new List<Transform>();


    // World Unit
    public float WorldUnit = 1.000f;

    //Menu
    public GameObject menuPanel;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        UpdateInvisibleCubes();
        playerReturn.GetComponent<PlayerReturn>().SetCheckPoint(player.transform.position);
        for (int i = 0; i < blockCubes.childCount; i++)
        {
            blockList.Add(blockCubes.GetChild(i));
        }
    }

    void Update()
    {
        if (!menuPanel.activeSelf)
        {
            if (Input.GetKeyDown(rotateKeyCode))
            {
                if (facingDirection == FacingDirection.Front)
                {
                    facingDirection = FacingDirection.Up;
                    playerMovement.UpdateFacingDirection(facingDirection);
                }
                else if (facingDirection == FacingDirection.Up)
                {
                    MovePlayerToClosestPlatformCube();
                    facingDirection = FacingDirection.Front;
                    playerMovement.UpdateFacingDirection(facingDirection);
                }
                playerMovement.SetIsRotating(true);
                UpdateInvisibleCubes();
            }
            if (facingDirection == FacingDirection.Up && !playerMovement.GetIsRotating())
            {
                MovePlayerToClosestInvisibleCube();
            }
            if (facingDirection == FacingDirection.Front)
            {
                if (OnInvisibleCube())
                {
                    MovePlayerToClosestPlatformCube();
                    UpdateInvisibleCubes();
                }
            }
        }
    }

    // Determines if the player is on an invisible cube
    private bool OnInvisibleCube()
    {
        foreach (Transform cube in invisibleList)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit / 2
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit / 2
                && player.transform.position.y - cube.position.y <= WorldUnit + 0.2f
                && player.transform.position.y - cube.position.y > 0)
                {
                    return true;
                }
            }
            else if (facingDirection == FacingDirection.Up)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit
                && player.transform.position.y - cube.position.y > 0)
                {
                    return true;
                }
            }

        }
        return false;
    }

    // Move player to the closest platform cube when player on an Invisible Cube
    private void MovePlayerToClosestPlatformCube()
    {
        // Get the Vector
        float frontZ = float.MaxValue;
        float upY = float.MinValue;
        foreach (Transform platform in platformCubes)
        {
            for (int i = 0; i < platform.childCount; i++)
            {
                if (facingDirection == FacingDirection.Front)
                {
                    if (Mathf.Abs(platform.GetChild(i).position.x - player.transform.position.x) < WorldUnit / 2
                    && player.transform.position.y - platform.GetChild(i).position.y <= WorldUnit + 0.2f
                    && player.transform.position.y - platform.GetChild(i).position.y > 0)
                    {
                        frontZ = Mathf.Min(frontZ, platform.GetChild(i).transform.position.z);
                    }
                }
                else if (facingDirection == FacingDirection.Up)
                {
                    if (Mathf.Abs(platform.GetChild(i).position.x - player.transform.position.x) < WorldUnit
                    && Mathf.Abs(platform.GetChild(i).position.z - player.transform.position.z) < WorldUnit
                    && player.transform.position.y > 1)
                    {
                        upY = Mathf.Max(upY, platform.GetChild(i).transform.position.y + 1);
                    }
                }

            }
        }

        // Change the Position
        if (facingDirection == FacingDirection.Front && frontZ != float.MaxValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, frontZ);
            player.GetComponent<CharacterController>().enabled = true;
        }
        else if (facingDirection == FacingDirection.Up && upY != float.MinValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, upY, player.transform.position.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    // Move player to the closest invisible cube when player on a platform cube
    private void MovePlayerToClosestInvisibleCube()
    {
        // Get the Vector
        float frontZ = float.MaxValue;
        float upY = float.MaxValue;
        foreach (Transform cube in invisibleCubes)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit / 2
                && player.transform.position.y - cube.position.y <= WorldUnit + 0.2f
                && player.transform.position.y - cube.position.y > 0)
                {
                    frontZ = Mathf.Min(frontZ, cube.position.z);
                }
            }
            else if (facingDirection == FacingDirection.Up)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit
                && player.transform.position.y >= 1)
                {
                    upY = Mathf.Min(upY, cube.position.y + 1);
                }
            }
        }

        // Change the Position
        if (facingDirection == FacingDirection.Front && frontZ != float.MaxValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, frontZ);
            player.GetComponent<CharacterController>().enabled = true;
        }
        else if (facingDirection == FacingDirection.Up && upY != float.MaxValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, upY, player.transform.position.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    // Delete the block in the blockList
    public void DeleteBlockCubes(Transform block)
    {
        blockList.Remove(block);
		UpdateInvisibleCubes();
    }

    // Update the invisible cubes when the direction changes
    public void UpdateInvisibleCubes()
    {
        // Delete the invisible cubes
        foreach (Transform cube in invisibleList)
        {
            Destroy(cube.gameObject);
        }
        invisibleList.Clear();

        // Create new invisible cubes
        Vector3 newCubePosition = Vector3.zero;
        float newCubeZ = GetCubeZByPlayer();
        float newCubeY = GetCubeYByPlatformAndBlockCubes();
        foreach (Transform platform in platformCubes)
        {
            for (int i = 0; i < platform.childCount; i++)
            {
                if (facingDirection == FacingDirection.Front)
                {
                    newCubePosition = new Vector3(platform.GetChild(i).position.x, platform.GetChild(i).position.y, newCubeZ);
                    if (!ExistCube(invisibleList, newCubePosition) && !ExistCube(platformCubes, newCubePosition) && !ExistCube(blockCubes, newCubePosition))
                    {
                        GameObject newCube = Instantiate(invisibleCube) as GameObject;
                        newCube.transform.position = newCubePosition;
                        invisibleList.Add(newCube.transform);
                        newCube.transform.SetParent(invisibleCubes);
                    }
                }
                else if (facingDirection == FacingDirection.Up)
                {
                    newCubePosition = new Vector3(platform.GetChild(i).position.x, newCubeY, platform.GetChild(i).position.z);
                    GameObject newCube = Instantiate(invisibleCube) as GameObject;
                    newCube.transform.position = newCubePosition;
                    invisibleList.Add(newCube.transform);
                    newCube.transform.SetParent(invisibleCubes);
                }
            }
        }
        foreach (Transform block in blockList)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                if (facingDirection == FacingDirection.Up)
                {
                    newCubePosition = new Vector3(block.GetChild(i).position.x, newCubeY + 1, block.GetChild(i).position.z);
                    GameObject newCube = Instantiate(invisibleCube) as GameObject;
                    newCube.transform.position = newCubePosition;
                    invisibleList.Add(newCube.transform);
                    newCube.transform.SetParent(invisibleCubes);
                }
            }
        }

    }

    // Get z axis of cube by player
    private float GetCubeZByPlayer()
    {
        return Mathf.Round(player.transform.position.z);
    }

    // Get y axis of cube by platform and block cubes
    private float GetCubeYByPlatformAndBlockCubes()
    {
        float platformCubeDepth = float.MinValue;
        foreach (Transform platform in platformCubes)
        {
            for (int i = 0; i < platform.childCount; i++)
            {
                platformCubeDepth = Mathf.Max(platformCubeDepth, platform.GetChild(i).position.y + invisibleCubesOffsetY);
            }
        }
        foreach (Transform block in blockCubes)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                platformCubeDepth = Mathf.Max(platformCubeDepth, block.GetChild(i).position.y + invisibleCubesOffsetY);
            }
        }
        return Mathf.Round(platformCubeDepth);
    }

    // Find if exists invisible cube
    private bool ExistCube(List<Transform> list, Vector3 newCube)
    {
        foreach (Transform cube in list)
        {
            if (cube.position == newCube)
            {
                return true;
            }
        }
        return false;
    }

    // Find if exists platform cube
    private bool ExistCube(Transform transform, Vector3 newCube)
    {
        foreach (Transform cubes in transform)
        {
            for (int i = 0; i < cubes.childCount; i++)
            {
                if (cubes.GetChild(i).position == newCube)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
