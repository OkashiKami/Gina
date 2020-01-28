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
    public Data data = new Data();

    private void Awake()
    {
        data.player.Set(PlayerData.Field.name, player_name);
        data.player.onNameChaged += (n) => { player_name = n; gameObject.name = n; };
        data.player.onPositionChanged += (p) => transform.position = p;
        data.player.onRotationChanged += (r) => transform.rotation = Quaternion.Euler(r);
        data.character.onChanged += Character_onChanged;
    }

    private void OnValidate() => data.OnValidate();


    private void Character_onChanged(Item[] value)
    {
        
    }

    private void Start()
    {
        data.Load();
        FunctionPeriodic.Create(() => data.Save(), 10);
    }

    private void OnApplicationQuit()
    {
        data.Save();
    }
    private void Update()
    {
        data.player.Set(PlayerData.Field.position, transform.position);
        data.player.Set(PlayerData.Field.rotation, transform.rotation.eulerAngles);
    }
}
