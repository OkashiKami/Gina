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

    private void Start()
    {
        player_data = new PlayerData();
        player_data.onStatsChanged += (a) =>
        {
            var pos = a.Get<Vector3>(Options.position);
            var rot = a.Get<Vector3>(Options.rotation);

            transform.position = pos;
            transform.eulerAngles = rot;
        };
        Database.LoadPlayerData(player_data, action: player_data.StatWasChanged);
        FunctionPeriodic.Create(() => Database.Save(player_data), 30);
    }
    
    private void OnApplicationQuit()
    {
        Database.Save(player_data);
        while (Database.saving) { }
    }
    private void Update()
    {
        player_data.SetStat(Options.position, transform.position);
        player_data.SetStat(Options.rotation, transform.eulerAngles);
    }


    


}
