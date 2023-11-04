using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public CameraState cameraState;
    public GameObject player;
    private PlayerReturn playerReturn;
    public List<Transform> enemyList;
    public List<Transform> guardList;
    public List<Transform> patrolList;

    // Start is called before the first frame update
    void Start()
    {
        playerReturn = player.GetComponent<PlayerReturn>();
        Transform guards = transform.Find("Guards");
        Transform patrols = transform.Find("Patrols");
        if(guards != null)
        {
            foreach(Transform guard in guards)
            {
                enemyList.Add(guard);
                guardList.Add(guard);
            }
        }
        if(patrols != null)
        {
            foreach(Transform patrol in patrols)
            {
                enemyList.Add(patrol);
                patrolList.Add(patrol);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TouchPlayer();
    }
    
    private void TouchPlayer()
    {
        foreach(Transform enemy in enemyList)
        {
            if (enemy.gameObject.activeSelf)
            {
                // Reset player
                if (cameraState.GetFacingDirection() == FacingDirection.Front)
                {
                    if (Mathf.Abs(enemy.transform.position.x - player.transform.position.x) <= 1f && 
                        Mathf.Abs(enemy.transform.position.y - player.transform.position.y) <= 1f)
                    {
                        playerReturn.ResetPlayer();
                    }
                }
                else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                {
                    if (Mathf.Abs(enemy.transform.position.x - player.transform.position.x) <= 1f &&
                        Mathf.Abs(enemy.transform.position.z - player.transform.position.z) <= 1f)
                    {
                        playerReturn.ResetPlayer();
                    }
                }
            }
    
        }
    }

    public void DestoryEnemy(Transform enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
        }
        if (guardList.Contains(enemy))
        {
            guardList.Remove(enemy);
            enemy.gameObject.GetComponent<GuardMovement>().DestroyGuard();
        }
        if (patrolList.Contains(enemy))
        {
            patrolList.Remove(enemy);
        }
    }

}
