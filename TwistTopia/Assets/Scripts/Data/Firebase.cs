using UnityEngine;
using System.Collections;
using Proyecto26;
using System;
using UnityEngine.SceneManagement;

public class Firebase : MonoBehaviour
{
    public GameObject player;
    public GameObject goal;
    private double priv_y;
    private double ini_x;
    private const string DATABASE_URL = "https://twisttopia-kai-version-default-rtdb.firebaseio.com/.json";
    private int platform_num;
    private string platform_tag;
    private int priv_platform_num;
    private string priv_cube;
    private long priv_time;
    private bool priv_is_falling = false;
    
    private long start_time;
    private long end_time;


    //level 1
    private const int NUM1 = 9;
    private double [] platform_time1 = new double[NUM1]{0,0,0,0,0,0,0,0,0};
    private string [] platform_cube1 = new string[NUM1]{"","","","","","","","",""};
    private string [] platform_cube_all1 = new string[NUM1]{"","","","","","","","",""};

    //level 2
    private const int NUM2 = 9;
    private double [] platform_time2 = new double[NUM2]{0,0,0,0,0,0,0,0,0};
    private string [] platform_cube2 = new string[NUM2]{"","","","","","","","",""};
    private string [] platform_cube_all2 = new string[NUM2]{"","","","","","","","",""};

    //level 3
    private const int NUM3 = 19;
    private double [] platform_time3 = new double[NUM3]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
    private string [] platform_cube3 = new string[NUM3]{"","","","","","","","","","","","","","","","","","",""};
    private string [] platform_cube_all3 = new string[NUM3]{"","","","","","","","","","","","","","","","","","",""};


    private int NUM;
    private double [] platform_time;
    private string [] platform_cube;
    private string [] platform_cube_all;



    private double [] drop_pos = new double [3]{0,0,0};

    private string priv_scene_name = "";
    private int scene_num = 0;


    void Start()
    {
        start_time = System.DateTime.Now.Ticks;
        StartCoroutine(SendTestData());
    }

    void Update(){
        StartCoroutine(SendTestData());
    }

    bool IsEqual(double a,double b){
        if(Mathf.Abs((float)(a-b))<0.1){
            return true;
        }
        return false;
    }

    void SetPlatformNum(){
        //判断当前是level几

        bool is_falling = true;
        
        Transform platformSetTransform = GameObject.Find("Platform Cubes").transform;
        for(int i =0; i<platformSetTransform.childCount;i++){
            if(!is_falling)
                break;
            GameObject platform = platformSetTransform.GetChild(i).gameObject;
            Transform platformTransform =platform.transform;
            for(int j=0; j<platformTransform.childCount;j++){
	            GameObject cube = platformTransform.GetChild(j).gameObject;
                if(j>=platform_cube_all[i].Split('_').Length){
                    platform_cube_all[i]+="_"+cube.transform.position;
                }
	            if(is_falling && Mathf.Abs((float)(player.transform.position.x-cube.transform.position.x))<1 &&
	               Mathf.Abs((float)(player.transform.position.z-cube.transform.position.z))<1 &&
	               (Mathf.Abs((float)(player.transform.position.y-cube.transform.position.y))<2 ||
                   IsEqual(player.transform.position.y,36) || 
                   IsEqual(player.transform.position.y,31) || 
                   IsEqual(player.transform.position.y,34))){
	                    platform_num = i+1;
	                    platform_tag = platform.name;
                        is_falling = false;
                        if(""+cube.transform.position!=priv_cube){
                            platform_cube[i]+="_"+cube.transform.position;
                            priv_cube = ""+cube.transform.position;
                        }
                        
	            }
            }              
        }
         

        if(priv_platform_num != platform_num || priv_is_falling != is_falling){

            Debug.Log(">>>>>>>>>>>>>>>>"+player.transform.position+" "+priv_platform_num+" "+platform_num+" "+platform_tag+" "+priv_is_falling+" "+is_falling);

            long current_time = System.DateTime.Now.Ticks;
            //Debug.Log(">>>>>>>>>>>current time: "+current_time);
            if(priv_platform_num>=1 && priv_platform_num <= NUM ){
                platform_time[priv_platform_num -1] += (current_time - priv_time);
            }

            priv_time = current_time;
        }
        priv_platform_num = platform_num;
        priv_is_falling = is_falling;
    }

    void SendPrivData(){
        end_time = System.DateTime.Now.Ticks;
        string jsonData = "{";
        jsonData += "\"current level\": "+scene_num+",";
        jsonData += "\"Drop_point_x\": "+drop_pos[0]+",";
        jsonData += "\"Drop_point_y\": "+drop_pos[1]+",";
        jsonData += "\"platform\": "+(int)drop_pos[2]+",";
        for(int i = 0; i < NUM ; i++){
            jsonData += "\"platform"+(i+1)+" time\": "+platform_time[i]/10000000+",";
            jsonData += "\"platform"+(i+1)+" all_cubeXYZ\": \""+platform_cube_all[i]+"\",";
            jsonData += "\"platform"+(i+1)+" passBy_cubeXYZ\": \""+platform_cube[i]+"\",";
            
        }
        jsonData += "\"success_time\": "+(double)(end_time-start_time)/10000000;
        jsonData += "}";
        Debug.Log(jsonData);
        start_time = System.DateTime.Now.Ticks;

        RestClient.Post(DATABASE_URL, jsonData).Then(response =>
        {
            Debug.Log("Data sent successfully!");
        }).Catch(error =>
        {
            Debug.LogError("Error sending data: " + error.Message);
        });
        
    }
        
    bool IsGoal(){
        if(Mathf.Abs((float)(player.transform.position.x
        -goal.transform.position.x))<1 &&
	       Mathf.Abs((float)(player.transform.position.z-goal.transform.position.z))<2 &&
	       (Mathf.Abs((float)(player.transform.position.y-goal.transform.position.y))<1 ||
            IsEqual(player.transform.position.y,36) || 
            IsEqual(player.transform.position.y,31) || 
            IsEqual(player.transform.position.y,34))){
            return true;
        }
        return false;
    }

    bool IsFalling(double y){
        if(y<-10){
            return true;
        }
        return false;
    }


    IEnumerator SendTestData()
    {
        Scene scene = SceneManager.GetActiveScene ();
        if(scene.name!=priv_scene_name){
            Debug.Log(">>>>>>>>>>"+scene.name+">>>>>>>>>"+priv_scene_name);
            start_time = System.DateTime.Now.Ticks;
        }

        priv_scene_name = scene.name;
        goal = GameObject.FindGameObjectWithTag("Goal");

        if(IsGoal()){
            SendPrivData();
            drop_pos[0]=0;
            drop_pos[1]=0;
            drop_pos[2]=0;
        }

        if(scene.name == "Level_1"){
            NUM = NUM1;
            platform_time = platform_time1;
            platform_cube = platform_cube1;
            platform_cube_all = platform_cube_all1;
            scene_num = 1;
        }
        else if(scene.name == "Level_2"){
            NUM = NUM2;
            platform_time = platform_time2;
            platform_cube = platform_cube2;
            platform_cube_all = platform_cube_all2;
            scene_num = 2;
        }
        else if(scene.name == "Level_3"){
            NUM = NUM3;
            platform_time = platform_time3;
            platform_cube = platform_cube3;
            platform_cube_all = platform_cube_all3;
            scene_num = 3;
        }
        else{
            yield break;
        }

        SetPlatformNum();

        if (!IsFalling(player.transform.position.y)){
            priv_y = player.transform.position.y;
            yield break;
        }

        if(IsFalling(priv_y)){
            yield break;
        }

        ini_x = player.transform.position.x;
        priv_y = player.transform.position.y;

        drop_pos[0] = ini_x;
        drop_pos[1] = player.transform.position.y;
        drop_pos[2] = platform_num;
                
        string jsonData = "{";
        jsonData += "\"current level\": "+scene_num+",";
        jsonData += "\"Drop_point_x\": "+ini_x+",";
        jsonData += "\"Drop_point_y\": "+player.transform.position.y+",";
        jsonData += "\"platform\": "+platform_num+",";
        for(int i = 0; i < NUM ; i++){
            if(i==NUM-1){
                jsonData += "\"platform"+(i+1)+" time\": "+platform_time[i]/10000000+",";
                jsonData += "\"platform"+(i+1)+" all_cubeXYZ\": \""+platform_cube_all[i]+",";
                jsonData += "\"platform"+(i+1)+" passBy_cubeXYZ\": \""+platform_cube[i]+"\"";    
            }
            else{
                jsonData += "\"platform"+(i+1)+" time\": "+platform_time[i]/10000000+",";
                jsonData += "\"platform"+(i+1)+" all_cubeXYZ\": \""+platform_cube_all[i]+"\",";
                jsonData += "\"platform"+(i+1)+" passBy_cubeXYZ\": \""+platform_cube[i]+"\",";
            }
        }
        jsonData += "}";
        Debug.Log(jsonData);



        RestClient.Post(DATABASE_URL, jsonData).Then(response =>
        {
            Debug.Log("Data sent successfully!");
        }).Catch(error =>
        {
            Debug.LogError("Error sending data: " + error.Message);
        });

        Debug.Log("over!");
        start_time = System.DateTime.Now.Ticks;
        yield break;

    }
}

