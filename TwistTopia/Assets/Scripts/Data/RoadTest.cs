using UnityEngine;
using System.Collections;
using Proyecto26;
using UnityEngine.SceneManagement;

public class RoadTest : MonoBehaviour
{
    private const string DATABASE_URL = "https://project-fc340-default-rtdb.firebaseio.com/.json";


    void Start()
    {
        
        FrameAction.OnEnemyCatched += HandleEnemyCatch;
        
    }
    
    void HandleEnemyCatch(string road)
    {
        string jsonData = "{\"Road\": \"" + road + "\"}";
        Debug.Log(road);
        RestClient.Post(DATABASE_URL, jsonData).Then(response =>
        {
            Debug.Log("Data sent successfully!");
        }).Catch(error =>
        {
            Debug.LogError("Error sending data: " + error.Message);
        });
    }


    
}

