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
        if (guards != null)
        {
            foreach (Transform guard in guards)
            {
                enemyList.Add(guard);
                guardList.Add(guard);
            }
        }
        if (patrols != null)
        {
            foreach (Transform patrol in patrols)
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
        TouchEnemy();
    }

    private void TouchPlayer()
    {
        foreach (Transform enemy in enemyList)
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

    private void TouchEnemy()
    {
        Transform enemy1;
        Transform enemy2;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].gameObject.activeSelf)
            {
                for (int j = i + 1; j < enemyList.Count; j++)
                {
                    if (enemyList[j].gameObject.activeSelf)
                    {
                        if (cameraState.GetFacingDirection() == FacingDirection.Front)
                        {
                            if (Mathf.Abs(enemyList[i].position.x - enemyList[j].position.x) <= 1f &&
                               Mathf.Abs(enemyList[i].position.y - enemyList[j].position.y) <= 1f)
                            {
                                enemy1 = enemyList[i];
                                enemy2 = enemyList[j];
                                DestroyEnemy(enemy1);
                                DestroyEnemy(enemy2);
                                return;
                            }
                        }
                        else if (cameraState.GetFacingDirection() == FacingDirection.Up)
                        {
                            if (Mathf.Abs(enemyList[i].position.x - enemyList[j].position.x) <= 1f &&
                               Mathf.Abs(enemyList[i].position.z - enemyList[j].position.z) <= 1f)
                            {
                                enemy1 = enemyList[i];
                                enemy2 = enemyList[j];
                                DestroyEnemy(enemy1);
                                DestroyEnemy(enemy2);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    public void DestroyEnemy(Transform enemy)
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
            Destroy(enemy.gameObject);
        }
    }

}
