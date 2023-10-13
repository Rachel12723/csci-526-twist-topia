using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script uses UV mapping to project a 2D texture onto a 3D object.
public class UVCube : MonoBehaviour
{
    private MeshFilter meshFilter; // A reference to the MeshFilter component of the GameObject. The MeshFilter holds the mesh for the GameObject, which in this case is expected to be a cube.
    public float tileSize = 0.125f; // Represents the size of each tile (or section) of the texture. This value determines how much of the texture is displayed on each face of the cube.
    
    void Start () {
        ApplyTexture ();
    }
    
    public void ApplyTexture()
    {
        meshFilter = gameObject.GetComponent<MeshFilter> ();
        if(meshFilter)
        {
            Mesh mesh = meshFilter.sharedMesh;
            if(mesh)
            {
                //FRBLUD - Freeblood - stands for Front, Right, Back, Left, Up and Down.
                //This is the order in which our image should be constructed to keep track when designing textures
                Vector2[] meshUVCoordinates = mesh.uv;
                // Front
                meshUVCoordinates[0] = new Vector2(0f, 0f);	//Bottom Left
                meshUVCoordinates[1] = new Vector2(tileSize, 0f); //Bottom Right
                meshUVCoordinates[2] = new Vector2(0f, 1f); //Top Left
                meshUVCoordinates[3] = new Vector2(tileSize, 1f); // Top Right
                // Right
                meshUVCoordinates[16] = new Vector2(tileSize * 1.001f, 0f);
                meshUVCoordinates[19] = new Vector2(tileSize * 2.001f, 0f);
                meshUVCoordinates[17] = new Vector2(tileSize * 1.001f, 1f);
                meshUVCoordinates[18] = new Vector2(tileSize * 2.001f, 1f);
                // Back
                meshUVCoordinates[10] = new Vector2((tileSize * 2.001f), 1f);
                meshUVCoordinates[11] = new Vector2((tileSize * 3.001f), 1f);
                meshUVCoordinates[6] = new Vector2((tileSize * 2.001f), 0f);
                meshUVCoordinates[7] = new Vector2((tileSize * 3.001f), 0f);
                // Right
                meshUVCoordinates[20] = new Vector2(tileSize * 3.001f, 0f);
                meshUVCoordinates[23] = new Vector2(tileSize * 4.001f, 0f);
                meshUVCoordinates[21] = new Vector2(tileSize * 3.001f, 1f);
                meshUVCoordinates[22] = new Vector2(tileSize * 4.001f, 1f);
                // Left
                meshUVCoordinates[8] = new Vector2(tileSize * 4.001f, 0f);
                meshUVCoordinates[9] = new Vector2(tileSize * 5.001f, 0f);
                meshUVCoordinates[4] = new Vector2(tileSize * 4.001f, 1f);
                meshUVCoordinates[5] = new Vector2(tileSize * 5.001f, 1f);
                // Down
                meshUVCoordinates[14] = new Vector2(tileSize * 5.001f, 0f);
                meshUVCoordinates[13] = new Vector2(tileSize * 6.001f, 0f);
                meshUVCoordinates[15] = new Vector2(tileSize * 5.001f, 1f);
                meshUVCoordinates[12] = new Vector2(tileSize * 6.001f, 1f);

                mesh.uv = meshUVCoordinates;
                // // Front
                // meshUVCoordinates[5] = new Vector2(0f, 0f); //Bottom Left
                // meshUVCoordinates[6] = new Vector2(tileSize, 0f); //Bottom Right
                // meshUVCoordinates[4] = new Vector2(0f, 1f); //Top Left
                // meshUVCoordinates[7] = new Vector2(tileSize, 1f); // Top Right
                //
                // // Right
                // meshUVCoordinates[20] = new Vector2(tileSize * 1.001f, 0f);
                // meshUVCoordinates[23] = new Vector2(tileSize * 2.001f, 0f);
                // meshUVCoordinates[21] = new Vector2(tileSize * 1.001f, 1f);
                // meshUVCoordinates[22] = new Vector2(tileSize * 2.001f, 1f);
                //
                // // Back
                // meshUVCoordinates[0] = new Vector2((tileSize * 2.001f), 1f);
                // meshUVCoordinates[3] = new Vector2((tileSize * 3.001f), 1f);
                // meshUVCoordinates[1] = new Vector2((tileSize * 2.001f), 0f);
                // meshUVCoordinates[2] = new Vector2((tileSize * 3.001f), 0f);
                //
                // // Left
                // meshUVCoordinates[16] = new Vector2(tileSize * 3.001f, 0f);
                // meshUVCoordinates[19] = new Vector2(tileSize * 4.001f, 0f);
                // meshUVCoordinates[17] = new Vector2(tileSize * 3.001f, 1f);
                // meshUVCoordinates[18] = new Vector2(tileSize * 4.001f, 1f);
                //
                // // Up
                // meshUVCoordinates[11] = new Vector2(tileSize * 4.001f, 0f);
                // meshUVCoordinates[8] = new Vector2(tileSize * 5.001f, 0f);
                // meshUVCoordinates[10] = new Vector2(tileSize * 4.001f, 1f);
                // meshUVCoordinates[9] = new Vector2(tileSize * 5.001f, 1f);
                //
                // // Down
                // meshUVCoordinates[15] = new Vector2(tileSize * 5.001f, 0f);
                // meshUVCoordinates[12] = new Vector2(tileSize * 6.001f, 0f);
                // meshUVCoordinates[14] = new Vector2(tileSize * 5.001f, 1f);
                // meshUVCoordinates[13] = new Vector2(tileSize * 6.001f, 1f);
                //
                // mesh.uv = meshUVCoordinates;
            }
        }
        else
            Debug.Log("No mesh filter attached");
    }
}
