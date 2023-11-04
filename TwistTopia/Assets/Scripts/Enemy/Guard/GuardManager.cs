using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    public float speed;

    private EnemyManager enemyManager;
    [HideInInspector] public GameObject player;
    [HideInInspector] public CameraState cameraState;

    public Transform keys;
    public GameObject key;


    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        player = enemyManager.player;
        cameraState = enemyManager.cameraState;
    }

    void Update()
    {

    }
}
