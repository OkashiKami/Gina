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
        Database.LoadPlayerData(player_data, action: () =>
        {
            player_data.StatWasChanged();
            transform.position = player_data.stats.Get<Vector3>(Options.position);
            transform.rotation = Quaternion.Euler(player_data.stats.Get<Vector3>(Options.rotation));
        });
        FunctionPeriodic.Create(() => Database.Save(player_data), 10);
    }
    
    private void OnApplicationQuit()
    {
        Database.Save(player_data);
    }
    private void Update()
    {
        player_data.SetStat(Options.position, transform.position);
        player_data.SetStat(Options.rotation, transform.rotation.eulerAngles);
    }


    


}
