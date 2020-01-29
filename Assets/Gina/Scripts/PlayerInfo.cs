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

public class PlayerInfo : MonoBehaviour
{
    public string player_name;
    public PlayerData data = new PlayerData();

    private void Awake()
    {
        data.onNameChanged += (n) => { player_name = n; gameObject.name = n; };
        data.onPositionChanged += (p) => transform.position = p;
        data.onRotationChanged += (r) => transform.rotation = Quaternion.Euler(r);
        data.Name = player_name;

        data.Set(Database.Load<PlayerData>(player_name));
        FunctionPeriodic.Create(() => Database.Save(data), 5000);
    }


    private void OnApplicationQuit()
    {
        Database.Save(data);
    }
    private void Update()
    {
        data.Position = transform.position;
        data.Rotation = transform.rotation.eulerAngles;
    }
}
