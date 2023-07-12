using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class SlimeController : MonoBehaviour, IDataPersistence
{

    //Setup for big slime position check
    public bool isSplit;
    public bool aiming;
    public int currentSlime = 1;



    //Timer and timer check to fix spammable splitting
    public float timer, interval = 2f;
    public float timeCheck = 0;
    public static float totalPlaytime = 0;

    //Creates empty objects that are found on start
    public GameObject playerObj;
    public GameObject bigSlime;
    public GameObject smallSlime1;
    public GameObject smallSlime2;


    public int cameraFollowCheck;

    //Slime positions
    public Vector2 bigSlimePos;
    public Vector2 smallSlime1Pos;
    public Vector2 smallSlime2Pos;
    public Vector2 slimeTarget;




    // Start is called before the first frame update
    void Start()
    {
        if (bigSlime == null)
        {
            bigSlime = GameObject.Find("Slime"); //Big Slime name must be same as object
        }
        if (smallSlime1 == null)
        {
            smallSlime1 = GameObject.Find("SmallSlimeBlue"); //first small slime name must be same as object
        }
        if (smallSlime2 == null)
        {
            smallSlime2 = GameObject.Find("SmallSlimeYellow"); //second small slime name must be same as object
        }
        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player"); //empty object name that includes all slimes
        }
    }


    // Update is called once per frame
    void Update()
    {
        //sets up timer
        timer += Time.deltaTime;
        totalPlaytime += Time.deltaTime;
        //gets position for all slimes
        bigSlimePos = bigSlime.transform.position;
        smallSlime1Pos = smallSlime1.transform.position;
        smallSlime2Pos = smallSlime2.transform.position;
        //Position for spawning Big Slime
        slimeTarget = smallSlime1Pos + (smallSlime2Pos - smallSlime1Pos) / 2;


    }


}
