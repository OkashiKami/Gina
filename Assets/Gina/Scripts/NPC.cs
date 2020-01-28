using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npc_name = "npc_01";
    public Data data = new Data();
    
    private Player player;

    private void Awake()
    {
        FindObjectOfType<InputController>().onInteract += OnInteract;
        data.player.Set(PlayerData.Field.name, npc_name);
        data.LootTable = Database.Get<LootTable>($"gina:{npc_name.ToLower()}");
        data.player.onNameChaged += (n) => { npc_name = n;  gameObject.name = n; };
        data.player.onPositionChanged += (p) => transform.position = p;
        data.player.onRotationChanged += (r) => transform.rotation = Quaternion.Euler(r);
    }
    private void Start()
    {
        data.Load(npc_name);
        FunctionPeriodic.Create(() => data.Save(npc_name), 10);
    }
    private void OnApplicationQuit()
    {
        data.Save(npc_name);
    }
    private void Update()
    {
        data.player.Set(PlayerData.Field.position, transform.position);
        data.player.Set(PlayerData.Field.rotation, transform.rotation.eulerAngles);
    }
    private void OnInteract(Player player)
    {
        if (!this.player) return;

        FindObjectOfType<NPCWindowUI>().SHOW(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
            player = null;
        FindObjectOfType<NPCWindowUI>().HIDE(this);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
            player = other.GetComponent<Player>();
    }
}
