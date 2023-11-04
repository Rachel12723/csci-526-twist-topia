using UnityEngine;
using UnityEditor;
using System.IO;

public class FlagImageCapture : MonoBehaviour
{
    [MenuItem("Utilities/Capture Flag Image")]
    public static void CaptureFlagImage()
    {
        // Set up your camera and flag specifics here
        Camera flagCamera = GameObject.Find("FlagCamera").GetComponent<Camera>(); // Your flag camera
        GameObject flag = GameObject.Find("Goal"); // Your flag GameObject

        // Render the flag to the RenderTexture
        RenderTexture renderTexture = new RenderTexture(256, 256, 24);
        flagCamera.targetTexture = renderTexture;
        flagCamera.Render();

        // Transfer image from RenderTexture to Texture2D
        RenderTexture.active = renderTexture;
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height);
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;

        // Save Texture2D to PNG
        byte[] bytes = texture2D.EncodeToPNG();
        string path = "Assets/Texture/FlagImage.png";
        File.WriteAllBytes(path, bytes);

        // Import the saved PNG as a new asset
        AssetDatabase.ImportAsset(path);

        // Clean up
        flagCamera.targetTexture = null;
        RenderTexture.DestroyImmediate(renderTexture);
        Texture2D.DestroyImmediate(texture2D);

        Debug.Log("Flag image saved to " + path);
    }
}