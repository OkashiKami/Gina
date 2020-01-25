using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class NPC : MonoBehaviour
{
    public string npc_name = "npc_01";
    public Data data;
    
    private Player player;

    private void Awake()
    {
        FindObjectOfType<InputController>().onInteract += OnInteract;
        data = new Data(npc: true);
        data.Name = npc_name;
        data.Set(pname.loot, npc_name.ToLower());
        data.onNameChaged += (n) => { npc_name = n;  gameObject.name = n; };
        data.Load(npc_name);
    }
    private void Start()
    {
        FunctionPeriodic.Create(() => data.Save(npc_name), 10);
    }
    private void OnApplicationQuit()
    {
        data.Save(npc_name);
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

#if UNITY_EDITOR
[CustomEditor(typeof(NPC)), DisallowMultipleComponent, CanEditMultipleObjects]
public class NPCeditor: Editor
{
    public override void OnInspectorGUI()
    {
        Repaint();

        var npc = (NPC)target;

        npc.npc_name = EditorGUILayout.TextField("FileNmae", npc.npc_name);
        GUILayout.Box("DATA", GUILayout.ExpandWidth(true));
        if (npc.data != null)
        {
            npc.data.Name = EditorGUILayout.TextField("Name", npc.data.Name);
            var lootid = npc.data.LootTable != null ? npc.data.LootTable.GetID : string.Empty;
            lootid = EditorGUILayout.TextField("Loot Table", lootid);
            npc.data.LootTable = Database.GetItemByID(lootid);
        }
    }
}

#endif