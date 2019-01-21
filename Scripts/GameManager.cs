﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Class for managing the Game functions
public class GameManager : MonoBehaviour
{
    // GameManager variables/objects
    private bool IsPlayerLoaded = false;
    public Camera StartCamera;
    public Text UITEXT;
    public GameObject PlayerObject = null;
    //public Transform PlayerTransform = null;
    public Vector3 playerpos;
    public static GameManager instance;
    private MainLoopable main;
    private readonly List<Delegate> _Delegates = new List<Delegate>();

    // Noise variables
    public float dx = 1;
    public float dz = 1;
    public float my = 1;
    public float cutoff = 1;
    public float mul = 1;
    // Static noise variables
    public static float Sdx = 50;
    public static float Sdz = 50;
    public static float Smy = 0.23f;
    public static float Scutoff = 1.8f;
    public static float Smul = 1;

    // Start is called before the first frame update
    // GameManager Start: Register Files, Create Texture Atlas
    void Start()
    {
        Debug.Log("GameManager.Start() executing...");
        FileManager.RegisterFiles();
        instance = this;
        TextureAtlas._Instance.CreateAtlas();
        MainLoopable.Instantiate();
        this.main = MainLoopable.GetInstance();
        this.main.Start();
    }

    // Update is called once per frame
    // GameManager Update: Set Player position if Player exists, Update MainLoopable Instance, Invoke Delegates and Remove them
    void Update()
    {
        if(this.PlayerObject != null)
        {
            this.playerpos = this.PlayerObject.transform.position;
            this.IsPlayerLoaded = true;
        }
        else
        {
            Debug.Log("Player is null. playerpos = " + playerpos);
        }
        // Uncomment the following lines for DevChunk testing
        // in World.Start() for lines   _LoadedChunk.Add(new Chunk...
        // change to                    _LoadedChunk.Add(new DevChunk...
        // then move sliders in editor to test Noise values on chunk gen
        /*
        Sdx = dx;
        Sdz = dz;
        Smy = my;
        Scutoff = cutoff;
        Smul = mul;
        */
        this.main.Update();
        for(int i = this._Delegates.Count - 1; i >= 0; i--)
        {
            this._Delegates[i].DynamicInvoke();
            this._Delegates.Remove(this._Delegates[i]);
        }
    }

    // GameManager On Application Quit
    void OnApplicationQuit()
    {
        this.main.OnApplicationQuit();
    }

    // Exit Game internal
    internal static void ExitGame()
    {
        instance.ExitGameInstance();
    }

    // Exit Game method
    public void ExitGameInstance()
    {
        this.OnApplicationQuit();
    }

    // Register Delegates
    public void RegisterDelegate(Delegate d)
    {
        this._Delegates.Add(d);
    }

    // Check if Player is loaded into world
    internal static bool PlayerLoaded()
    {
        return instance.IsPlayerLoaded;
    }

    // Create player in world and destroy starting UI
    public void StartPlayer(Vector3 Pos)
    {
        Debug.Log("Running StartPlayer Method from GameManager");
        Destroy(this.StartCamera);
        Destroy(this.UITEXT);
        GameObject PlayerObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/Player"), Pos, Quaternion.identity) as GameObject;
        playerpos = PlayerObject.transform.position;
    }
}
