using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UVCube))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UVCube myScript = (UVCube)target;
        if(GUILayout.Button("Apply Texture"))
        {
            myScript.ApplyTexture();
        }
    }
}
