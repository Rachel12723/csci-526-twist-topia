using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Transform> enemyList;
    public List<Transform> guardList;
    public List<Transform> patrolList;

    // Start is called before the first frame update
    void Start()
    {
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
