using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npc_name = "npc_01";
    public Data npc_data;
    private Player player;

    private void Awake()
    {
        FindObjectOfType<InputController>().onInteract += OnInteract;
        npc_data = new Data();
        npc_data.onPositionChanged += (p) => transform.position = p;
        npc_data.onRotationChanged += (r) => transform.rotation = Quaternion.Euler(r);
        npc_data.Load(npc_name);
    }
    private void Start()
    {
        npc_data.Init();
        FunctionPeriodic.Create(() => npc_data.Save(npc_name), 10);
    }
    private void OnApplicationQuit()
    {
        npc_data.Save(npc_name);
    }


    private void OnInteract(Player player)
    {
        if (!player) return;

        FindObjectOfType<NPCWindowUI>().SHOW(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
            player = null;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
            player = other.GetComponent<Player>();
    }
}
