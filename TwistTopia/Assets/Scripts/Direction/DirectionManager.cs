using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour
{
    // public float cooldownDuration = 0.5f; // Cooldown duration in seconds
    // private float lastShiftTime = -1.0f; // Time when the perspective was last shifted
    
    // Rotate
    public CameraState cameraState;
    public KeyCode rotateKeyCode;
    public KeyCode pickUpKeyCode;
    public KeyCode openDoorCode;

    // Player
    public GameObject player;
    public GameObject Goal;
    private PlayerState playerState;
    private PlayerReturn playerReturn;


    // Map
    public Transform map;
    // Platform Cubes
    private Transform platformCubes;
    // Block Cubes
    private Transform blockCubes;
    private List<Transform> blockList = new List<Transform>();
    // Invisible Cubes
    public GameObject invisibleCube;
    private Transform invisibleCubes;
    private float invisibleCubesY;
    private List<Transform> invisibleList = new List<Transform>();


    // World Unit
    public float WorldUnit = 1.000f;

    // Input Manager
    public InputManager inputManager;

    //Data
    public delegate void RotateEventHandler(Vector3 playerPosition);
    public static event RotateEventHandler OnRotateKeyPressed;

    void Start()
    {
        platformCubes= map.Find("Platform Cubes");
        blockCubes = map.Find("Block Cubes");
        invisibleCubes = map.Find("Invisible Cubes");
        for (int i = 0; i < blockCubes.childCount; i++)
        {
            blockList.Add(blockCubes.GetChild(i));
        }

        playerState = player.GetComponent<PlayerState>();
        playerReturn = player.GetComponent<PlayerReturn>();
        
        playerReturn.SetFrontMinY(GetMinCubeYOfPlatformAndBlockCubes());
        float upMinY=Mathf.Max(GetMaxCubeYOfPlatformAndBlockCubes() + 2f, player.transform.position.y + 1f);
        playerReturn.SetUpMinY(upMinY);
        invisibleCubesY = playerReturn.GetDropY()+upMinY;

        UpdateInvisibleCubes();
    }

    void Update()
    {
        if (inputManager.GetAllowShiftPerspective())
        {
            if (Input.GetKeyDown(rotateKeyCode)) //&& Time.time - lastShiftTime > cooldownDuration
            {
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    playerState.SetPositionUpdating(true);
                    cameraState.SetFacingDirection(FacingDirection.Up);
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
                    playerState.SetPositionUpdating(true);
                    MovePlayerToClosestPlatformCube();
                    playerState.SetPositionUpdating(false);
                    cameraState.SetFacingDirection(FacingDirection.Front);
                }
                OnRotateKeyPressed?.Invoke(player.transform.position);
                UpdateInvisibleCubes();
            }
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                if (OnInvisibleCube())
                {
                    MovePlayerToClosestPlatformCube();
                    UpdateInvisibleCubes();
                }
            }
            if (cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating() && !cameraState.GetIsUsing3DView())
            {
                if (playerState.GetPositionUpdating())
                {
                    MovePlayerToClosestInvisibleCube();
                    playerState.SetPositionUpdating(false);
                }
                if (!OnInvisibleCube())
                {
                    playerState.SetUpIsDropping(true);
                }
            }
        }

        //Scale
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            if (player.transform.localScale != Vector3.one)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.localScale = Vector3.one;
                player.GetComponent<CharacterController>().enabled = true;
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up && !cameraState.GetIsRotating() && !cameraState.GetIsUsing3DView())
        {
            if (player.transform.position.y < invisibleCubesY)
            {
                float scale = playerReturn.dropY / (invisibleCubesY - player.transform.position.y + playerReturn.dropY);
                scale = Mathf.Max(0.5f, scale);
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.localScale = new Vector3(scale, scale, scale);
                player.GetComponent<CharacterController>().enabled = true;
            }
            else if (player.transform.localScale != Vector3.one)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.localScale = Vector3.one;
                player.GetComponent<CharacterController>().enabled = true;
            }
        }
    }

    // Determines if the player is on an invisible cube
    private bool OnInvisibleCube()
    {
        foreach (Transform cube in invisibleList)
        {
            if (cameraState.GetFacingDirection() == FacingDirection.Front)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit / 2
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit / 2
                && player.transform.position.y - cube.position.y <= WorldUnit + 0.2f
                && player.transform.position.y - cube.position.y > 0)
                {
                    return true;
                }
            }
            else if (cameraState.GetFacingDirection() == FacingDirection.Up)
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
    public void MovePlayerToClosestPlatformCube()
    {
        // Get the Vector
        float frontZ = float.MaxValue;
        float upY = float.MinValue;
        foreach (Transform platform in platformCubes)
        {
            if (platform.gameObject.activeSelf)
            {
                for (int i = 0; i < platform.childCount; i++)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Front)
                    {
                        if (Mathf.Abs(platform.GetChild(i).position.x - player.transform.position.x) < WorldUnit / 2
                        && player.transform.position.y - platform.GetChild(i).position.y <= WorldUnit + 0.2f
                        && player.transform.position.y - platform.GetChild(i).position.y > 0)
                        {
                            frontZ = Mathf.Min(frontZ, platform.GetChild(i).transform.position.z);
                        }
                    }
                    else if (cameraState.GetFacingDirection() == FacingDirection.Up)
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
        }

        // Change the Position
        if (cameraState.GetFacingDirection() == FacingDirection.Front && frontZ != float.MaxValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, frontZ);
            player.GetComponent<CharacterController>().enabled = true;
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up && upY != float.MinValue)
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
        if (cameraState.GetFacingDirection() == FacingDirection.Front)
        {
            foreach (Transform cube in invisibleCubes)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit / 2
                && player.transform.position.y - cube.position.y <= WorldUnit + 0.2f
                && player.transform.position.y - cube.position.y > 0)
                {
                    frontZ = Mathf.Min(frontZ, cube.position.z);
                }
            }
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
        {
            foreach(Transform platform in platformCubes)
            {
                if (platform.gameObject.activeSelf)
                {
                    foreach (Transform platformCube in platform)
                    {
                        if (Mathf.Abs(platformCube.position.x - player.transform.position.x) < WorldUnit
                        && Mathf.Abs(platformCube.position.z - player.transform.position.z) < WorldUnit
                        && player.transform.position.y - platformCube.position.y > 0)
                        {
                            foreach (Transform cube in invisibleCubes)
                            {
                                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit
                                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit)
                                {
                                    upY = Mathf.Min(upY, cube.position.y + 1);
                                }
                            }

                        }
                    }
                }
            }
        }
        Debug.Log(upY);

        // Change the Position
        if (cameraState.GetFacingDirection() == FacingDirection.Front && frontZ != float.MaxValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, frontZ);
            player.GetComponent<CharacterController>().enabled = true;
        }
        else if (cameraState.GetFacingDirection() == FacingDirection.Up && upY != float.MaxValue)
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
        float newCubeY = invisibleCubesY;
        foreach (Transform platform in platformCubes)
        {
            if (platform.gameObject.activeSelf)
            {
                for (int i = 0; i < platform.childCount; i++)
                {
                    if (cameraState.GetFacingDirection() == FacingDirection.Front)
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
                    else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                    {
                        newCubePosition = new Vector3(platform.GetChild(i).position.x, newCubeY, platform.GetChild(i).position.z);
                        GameObject newCube = Instantiate(invisibleCube) as GameObject;
                        newCube.transform.position = newCubePosition;
                        invisibleList.Add(newCube.transform);
                        newCube.transform.SetParent(invisibleCubes);
                    }
                }
            }
        }
        foreach (Transform block in blockList)
        {
            for (int i = 0; i < block.childCount; i++)
            {
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
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
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
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
            if (cubes.gameObject.activeSelf)
            {
                for (int i = 0; i < cubes.childCount; i++)
                {
                    if (cubes.GetChild(i).position == newCube)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
