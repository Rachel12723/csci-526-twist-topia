using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputState inputState;
    // Start is called before the first frame update
    void Start()
    {
        inputState = InputState.NoInput;
    }

    void LateUpdate()
    {
        
    }

    public InputState GetInputState()
    {
        Debug.Log("Get: "+inputState);
        return inputState;
    }

    private void SetInputState(InputState inputState)
    {
        Debug.Log("Set: " + inputState);
        this.inputState = inputState;
    }
}
