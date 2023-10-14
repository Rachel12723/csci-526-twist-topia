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
    public GameObject Goal;
    private PlayerState playerState;
    private PlayerMovement playerMovement;
    public bool positionUpdated = true;

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
        playerState = player.GetComponent<PlayerState>();
        playerMovement = player.GetComponent<PlayerMovement>();
        UpdateInvisibleCubes();
        player.GetComponent<PlayerReturn>().SetCheckPoint(player.transform.position);
        player.GetComponent<PlayerReturn>().SetFrontMinY(GetMinCubeYOfPlatformAndBlockCubes());
        player.GetComponent<PlayerReturn>().SetUpMinY(GetMaxCubeYOfPlatformAndBlockCubes() + invisibleCubesOffsetY);
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
                    positionUpdated = false;
                    facingDirection = FacingDirection.Up;
                    playerState.SetFacingDirection(facingDirection);
                    playerState.SetIsRotating(true);
                }
                else if (facingDirection == FacingDirection.Up)
                {
                    positionUpdated = false;
                    MovePlayerToClosestPlatformCube();
                    positionUpdated = true;
                    facingDirection = FacingDirection.Front;
                    playerState.SetFacingDirection(facingDirection);
                    playerState.SetIsRotating(true);
                }
                playerState.SetIsRotating(true);
                UpdateInvisibleCubes();
            }
            if (facingDirection == FacingDirection.Front)
            {
                if (OnInvisibleCube())
                {
                    MovePlayerToClosestPlatformCube();
                    UpdateInvisibleCubes();
                }
            }
            if (facingDirection == FacingDirection.Up && !playerState.GetIsRotating())
            {
                if (!positionUpdated)
                {
                    MovePlayerToClosestInvisibleCube();
                    positionUpdated = true;
                }
                if (!OnInvisibleCube())
                {
                    playerState.SetUpIsDropping(true);
                }
            }

            //Scale
            if (facingDirection == FacingDirection.Front)
            {
                
                if (player.transform.Find("Sphere").localScale != Vector3.one)
                {
                    //player.GetComponent<CharacterController>().enabled = false;
                    player.transform.Find("Sphere").localScale = Vector3.one;
                    //player.GetComponent<CharacterController>().enabled = true;
                }
            }
            else if (facingDirection == FacingDirection.Up && !playerState.GetIsRotating())
            {
                if (player.transform.position.y < GetMaxCubeYOfPlatformAndBlockCubes() + invisibleCubesOffsetY)
                {
                    float scale = 10f / (GetMaxCubeYOfPlatformAndBlockCubes() + invisibleCubesOffsetY - player.transform.position.y + 10);
                    //player.GetComponent<CharacterController>().enabled = false;
                    player.transform.Find("Sphere").localScale = new Vector3(scale, scale, scale);
                    //player.GetComponent<CharacterController>().enabled = true;
                }
                else if(player.transform.Find("Sphere").localScale != Vector3.one)
                {
                    //player.GetComponent<CharacterController>().enabled = false;
                    player.transform.Find("Sphere").localScale = Vector3.one;
                    //player.GetComponent<CharacterController>().enabled = true;
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
                && player.transform.position.y - cube.position.y <= WorldUnit + 0.2f
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
                    && player.transform.position.y - platform.GetChild(i).position.y > 0)
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
    public void MovePlayerToClosestInvisibleCube()
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
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit)
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
                if (facingDirection == FacingDirection.Front)
                {
                    newCubePosition = new Vector3(block.GetChild(i).position.x, block.GetChild(i).position.y, newCubeZ);
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
        return GetMaxCubeYOfPlatformAndBlockCubes() + invisibleCubesOffsetY;
    }

    // Get the max Y of platform and block cubes
    private float GetMaxCubeYOfPlatformAndBlockCubes()
    {
        float cubeDepth = float.MinValue;
        foreach (Transform platform in platformCubes)
        {
            for (int i = 0; i < platform.childCount; i++)
            {
                cubeDepth = Mathf.Max(cubeDepth, platform.GetChild(i).position.y);
            }
        }
        foreach (Transform block in blockCubes)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                cubeDepth = Mathf.Max(cubeDepth, block.GetChild(i).position.y);
            }
        }
        return Mathf.Round(cubeDepth);
    }

    // Get the min Y of platform and block cubes
    private float GetMinCubeYOfPlatformAndBlockCubes()
    {
        float cubeDepth = float.MaxValue;
        foreach (Transform platform in platformCubes)
        {
            for (int i = 0; i < platform.childCount; i++)
            {
                cubeDepth = Mathf.Min(cubeDepth, platform.GetChild(i).position.y);
            }
        }
        foreach (Transform block in blockCubes)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                cubeDepth = Mathf.Min(cubeDepth, block.GetChild(i).position.y);
            }
        }
        return Mathf.Round(cubeDepth);
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

    public float returnInvisibleCubesOffsetY()
    {
        return invisibleCubesOffsetY;
    }
}
