using UnityEngine;
using System.Collections;

public class ReturnPlayer : MonoBehaviour {
 
    public Transform returnPoint;
 

    public void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CharacterController>().enabled = false;
        other.transform.position = returnPoint.position;
        other.GetComponent<CharacterController>().enabled = true;
  
    }
}
