using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInfo : MonoBehaviour
{
    public string npc_name = "npc_01";
    public NpcData data = new NpcData();
    
    private PlayerInfo player;

    private void Awake()
    {
        data.onNameChanged += (n) => { npc_name = n;  gameObject.name = n; };
        data.onPositionChanged += (p) => transform.position = p;
        data.onRotationChanged += (r) => transform.rotation = Quaternion.Euler(r);
        data.Name = npc_name;

        data.Set(Database.Load<NpcData>(npc_name));
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

    private void OnInteract(PlayerInfo player)
    {
        if (!this.player) return;

        FindObjectOfType<NPCWindowUI>().SHOW(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInfo>())
            player = null;
        FindObjectOfType<NPCWindowUI>().HIDE(this);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerInfo>())
            player = other.GetComponent<PlayerInfo>();
    }
}
