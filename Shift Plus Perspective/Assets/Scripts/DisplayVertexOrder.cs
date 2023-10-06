using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class DisplayVertexOrder : MonoBehaviour
{
    public float markerSize = 0.05f;
    public Color markerColor = Color.red;
    

    void OnDrawGizmos()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        Gizmos.color = markerColor;

        for (int i = 0; i < vertices.Length; i++)
        {
            // Convert local vertex position to world space
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            float randomOffsetRange = 0.05f;  // Adjust this value as needed
            Vector3 randomOffset = new Vector3(
                Random.Range(-randomOffsetRange, randomOffsetRange),
                Random.Range(-randomOffsetRange, randomOffsetRange),
                Random.Range(-randomOffsetRange, randomOffsetRange)
            );

            // Draw a small sphere and label for each vertex
            Gizmos.DrawSphere(worldPos, markerSize);
            UnityEditor.Handles.Label(worldPos + randomOffset, i.ToString());
        }
    }
}