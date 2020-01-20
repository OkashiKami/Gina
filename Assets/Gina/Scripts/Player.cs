using CodeMonkey.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData player_data;

    private void Awake()
    {
        player_data = new PlayerData();
        player_data.onPositionChanged += (p) => transform.position = p;
        player_data.onRotationChanged += (r) => transform.rotation = Quaternion.Euler(r);
        player_data.Load();


    }

    private void Start()
    {
        
        player_data.Init();
        FunctionPeriodic.Create(() => player_data.Save(), 10);
    }

    private void OnApplicationQuit()
    {
        player_data.Save();
    }
    private void Update()
    {
        player_data.SetPotion(transform.position);
        player_data.SetRotation(transform.rotation.eulerAngles);
    }


    


}
