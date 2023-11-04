using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{
    public float offset = 2f;
    public float speed = 2f;
    private EnemyManager enemyManager;
    [HideInInspector] public CameraState cameraState;
    [HideInInspector] public PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        cameraState = enemyManager.cameraState;
        playerState = enemyManager.player.GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
