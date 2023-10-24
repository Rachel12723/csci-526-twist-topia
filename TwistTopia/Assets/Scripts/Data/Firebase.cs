using UnityEngine;
using System.Collections;
using Proyecto26;
using System;
using UnityEngine.SceneManagement;

public class Firebase : MonoBehaviour
{
    public GameObject player;
    private double priv_y;
    private double ini_x;
    private const string DATABASE_URL = "https://twisttopia-kai-version-default-rtdb.firebaseio.com/.json";
    private int platform_num;
    private int priv_platform_num;
    private long priv_time;
    private bool priv_is_falling = false;


    //level 1
    private const int NUM1 = 9;
    private double [] platform_time1 = new double[NUM1]{0,0,0,0,0,0,0,0,0};
    //x1,x2,z1,z2,y
    private double [,] platform_pos1 = new double [NUM1,5] {
        {8, 11, 7, 8, 9} ,  
        {11, 16, 3, 4, 5} ,   
        {16, 25, 5, 10, 7}, 
        {26, 30, 4, 5, 7}, 
        {28, 33, 6, 7, 14},   
        {34, 47, 2, 3, 14},  
        {44, 47, 0, 1, 8}, 
        {40, 43, -4, -3, 6},   
        {34, 40, -4, -3, 4}   
    };

    //level 2
    private const int NUM2 = 9;
    private double [] platform_time2 = new double[NUM2]{0,0,0,0,0,0,0,0,0};
    //x1,x2,z1,z2,y
    private double [,] platform_pos2 = new double [NUM2,5] {
        {0, 39, 0, 3, 1} ,
        {-5, 4, 9, 11, -2} ,
        {-10, -1, 6, 8, 2} ,
        {28, 29, 4, 5, -2} ,
        {24, 29, 6, 7, 4} ,
        {20, 23, -4, -3, 5} ,
        {24, 31, -4, -3, -2} ,
        {32, 35, 10, 11, -2} ,
        {28, 31, 10, 11, -5} 
    };

    //level 3
    private const int NUM3 = 19;
    
    private double [] platform_time3 = new double[NUM3]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
    private double [,] platform_pos3 = new double [NUM3,5] {
        {0,8,0,2,1} , //1
        {-3,1,3,5,4} , //2
        {-7,-3,-4,-2,1} ,//3
        {8,12,-4,-2,6} ,//4
        {11,22,3,5,3} ,//5
        {21,28,6,7,6} ,//6
        {27,42,7,9,9},//7
        {40,49,6,7,12},//8
        {22,24,1,2,0} ,//9
        {25,27,1,2,-3} ,//10
        {28,30,1,2,-6} ,//11
        {27,42,-1,1,-3} ,//12
        {44,48,1,2,-6} ,//13
        {47,87,-1,8,3} ,//14
        {82,91,13,15,0} ,//15
        {89,93,-2,0,0} ,//16
        {83,91,-2,0,-9} ,//17
        {84,93,8,10,-6} ,//18
        {77,79,-2,-1,-6}     //19
    };

    private int NUM;
    private double [] platform_time;
    private double [,] platform_pos;

    private string priv_scene_name = "";

    private double [] drop_pos = new double [3]{0,0,0};

    private int scene_num = 0;


    //public double platform_time[3];


    void Start()
    {
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
        
        for(int i =0; i<NUM ;i++){
            if(player.transform.position.x>=platform_pos[i,0] && 
                player.transform.position.x<=platform_pos[i,1] &&
                player.transform.position.z>=platform_pos[i,2] &&
                player.transform.position.z<=platform_pos[i,3] &&
                (IsEqual(player.transform.position.y,platform_pos[i,4]) || IsEqual(player.transform.position.y,36) || IsEqual(player.transform.position.y,31) || IsEqual(player.transform.position.y,34))){
                    platform_num = i+1;
                    is_falling = false;
            }
        }
        /*
        if(player.transform.position.x>=0 && 
           player.transform.position.x<=5 &&
           player.transform.position.z>=-2 &&
           player.transform.position.z<=0 &&
           (IsEqual(player.transform.position.y,1) || IsEqual(player.transform.position.y,27))){
            platform_num = 1;
        }
  
        */
        //Debug.Log(">>>>>>>>>>>>>>>>"+player.transform.position.x+" "+player.transform.position.y+" "+player.transform.position.z+" "+platform_num+" "+priv_is_falling+" "+is_falling);

        if(priv_platform_num != platform_num || priv_is_falling != is_falling){

            Debug.Log(">>>>>>>>>>>>>>>>"+priv_platform_num+" "+platform_num+" "+priv_is_falling+" "+is_falling);

            long current_time = System.DateTime.Now.Ticks;
            //Debug.Log(">>>>>>>>>>>current time: "+current_time);
            if(priv_platform_num>=1 && priv_platform_num <= NUM ){
                platform_time[priv_platform_num -1] += (current_time - priv_time);
            }

            /*
            if(priv_platform_num == 1){
                platform1_time += (current_time - priv_time);
            }
            */

            priv_time = current_time;
        }
        priv_platform_num = platform_num;
        priv_is_falling = is_falling;
    }

    void SendPrivData(){
        string jsonData = "{";
        jsonData += "\"current level\": "+scene_num+",";
        jsonData += "\"Drop_point_x\": "+drop_pos[0]+",";
        jsonData += "\"Drop_point_y\": "+drop_pos[1]+",";
        jsonData += "\"platform\": "+(int)drop_pos[2]+",";
        for(int i = 0; i < NUM ; i++){
            if(i==NUM-1){
                jsonData += "\"platform"+(i+1)+" time\": "+platform_time[i]/10000000;    
            }
            else{
                jsonData += "\"platform"+(i+1)+" time\": "+platform_time[i]/10000000+",";
            }
        }
        /*
        jsonData += "\"platform1 time\": "+platform1_time/10000000+",";
        jsonData += "\"platform2 time\": "+platform2_time/10000000+",";
        jsonData += "\"platform3 time\": "+platform3_time/10000000;
        */
        jsonData += "}";

        Debug.Log(jsonData);



        RestClient.Post(DATABASE_URL, jsonData).Then(response =>
        {
            Debug.Log("Data sent successfully!");
        }).Catch(error =>
        {
            Debug.LogError("Error sending data: " + error.Message);
        });
        
    }
        
    bool IsGoal1(){
        if(IsEqual(player.transform.position.x,35.2) && IsEqual(player.transform.position.y,4) && IsEqual(player.transform.position.z,-4)){
            return true;
        }
        return false;
    }

    bool IsGoal2(){
        if(IsEqual(player.transform.position.x,37) && IsEqual(player.transform.position.y,1) && IsEqual(player.transform.position.z,1)){
            return true;
        }
        return false;
    }
    bool IsGoal3(){
        if(IsEqual(player.transform.position.x,8) && IsEqual(player.transform.position.y,1) && IsEqual(player.transform.position.z,3)){
            return true;
        }
        return false;
    }

    bool IsFalling(double y){
        if(y<-10){
            return true;
        }
        //if(y<36 && y>29){
            //return true;
        //}
        return false;
    }


    IEnumerator SendTestData()
    {
        Scene scene = SceneManager.GetActiveScene ();
        Debug.Log(">>>>>>>>>>"+scene.name+">>>>>>>>>"+priv_scene_name);

        if(scene.name =="Level_1" && IsGoal1()){
            SendPrivData();
            drop_pos[0]=0;
            drop_pos[1]=0;
            drop_pos[2]=0;
        }
        else if(scene.name =="Level_2" && IsGoal2()){
            SendPrivData();
            drop_pos[0]=0;
            drop_pos[1]=0;
            drop_pos[2]=0;
        }
        
        else if(scene.name =="Level_3" && IsGoal3()){
            SendPrivData();
            yield break;
        }

        priv_scene_name = scene.name;

        if(scene.name == "Level_1"){
            NUM = NUM1;
            platform_time = platform_time1;
            platform_pos = platform_pos1;
            scene_num = 1;
        }
        else if(scene.name == "Level_2"){
            NUM = NUM2;
            platform_time = platform_time2;
            platform_pos = platform_pos2;
            scene_num = 2;
        }
        else if(scene.name == "Level_3"){
            NUM = NUM3;
            platform_time = platform_time3;
            platform_pos = platform_pos3;
            scene_num = 3;
        }
        else{
            yield break;
        }

        Debug.Log(scene.name);
        
        //Debug.Log("=========>>>>>x="+player.transform.position.x);
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
                jsonData += "\"platform"+(i+1)+" time\": "+platform_time[i]/10000000;    
            }
            else{
                jsonData += "\"platform"+(i+1)+" time\": "+platform_time[i]/10000000+",";
            }
        }
        /*
        jsonData += "\"platform1 time\": "+platform1_time/10000000+",";
        jsonData += "\"platform2 time\": "+platform2_time/10000000+",";
        jsonData += "\"platform3 time\": "+platform3_time/10000000;
        */
        jsonData += "}";

        Debug.Log(jsonData);



        RestClient.Post(DATABASE_URL, jsonData).Then(response =>
        {
            Debug.Log("Data sent successfully!");
        }).Catch(error =>
        {
            Debug.LogError("Error sending data: " + error.Message);
        });
        
        /*
        for(int i= 0; i<NUM; i++){
            platform_time[i] =0;
        }
        */

        Debug.Log("over!");
        yield break;

    }
}

