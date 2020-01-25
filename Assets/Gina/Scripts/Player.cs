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
    public string player_name;
    public Data data;

    private void Awake()
    {
        data = new Data(inven: true, stats: true, posrot: true);
        data.Name = player_name;
        data.onPositionChanged += (p) => transform.position = p;
        data.onRotationChanged += (r) => transform.rotation = Quaternion.Euler(r);
        data.Load();
    }

    private void Start()
    {
        FunctionPeriodic.Create(() => data.Save(), 10);
    }

    private void OnApplicationQuit()
    {
        data.Save();
    }
    private void Update()
    {
        data.Position = transform.position;
        data.Rotation = transform.rotation.eulerAngles;
    }
}
