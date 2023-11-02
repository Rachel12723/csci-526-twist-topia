using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitOperation : MonoBehaviour
{
    private CircuitManager circuitManager;
    private Transform player;
    public CircuitType type;
    
    // Start is called before the first frame update
    void Start()
    {
        circuitManager = transform.parent.gameObject.GetComponent<CircuitManager>();
        player = circuitManager.player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
