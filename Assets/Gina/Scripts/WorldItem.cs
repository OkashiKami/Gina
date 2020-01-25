using CodeMonkey.Utils;
using System;
using TMPro;
using UnityEngine;
using Invector.CharacterController;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
internal class WorldItem : MonoBehaviour
{
    private SpriteRenderer icon;
    private TextMeshPro amount;
    public  Item item = null;

    internal static void Create(Item item, Vector3 position = default, int amount = 1, float despawn = 300, bool point = false, bool table = false)
    {
        var wip = Resources.Load("Prefabs/World_Item") as GameObject;
        if(!table)
        {
            for (int i = 0; i < amount; i++)
            {
                var x = position.x + (point ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));
                var z = position.z + (point ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));

                var wi = Instantiate(wip, new Vector3(x, position.y, z), Quaternion.identity).GetComponent<WorldItem>();
                wi.name = $"[WORLD ITEM]: {item.Get<string>(pname.name)}";
                wi.item = item;
                if (despawn > 0)
                    Destroy(wi.gameObject, despawn);
                wi.transform.SetAsLastSibling();
            }
        }
        else
        {
            Dictionary<int, Item> tabledata = new Dictionary<int, Item>();
            var curentry = item.data.Keys.ToList().FindAll(x => x.StartsWith("loot_entry"));
            for (int i = 0; i < curentry.Count; i++)
            {
                var entry = curentry[i];

                var e = item.Get<string>(entry).Split(',')[0];
                var d = item.Get<string>(entry).Split(',')[1];
                tabledata.Add(int.Parse(d), Database.GetItemByID(e));
            }

            foreach (var key in tabledata.Keys)
            {
                var roll = UnityEngine.Random.Range(0, 100);
                if(roll <= key)
                {
                    WorldItem.Create(tabledata[key], position, point: true);
                }

            }
        }
    }

    private void Awake()
    {
        icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
        amount = transform.Find("Amount").GetComponent<TextMeshPro>();

        FunctionPeriodic.Create(() =>
        {
            Destroy(this.gameObject);
        }, 300);

        FindObjectOfType<InputController>().onInteract += OnInteract;
    }

    private void OnInteract(Player player)
    {
        var dis = Vector3.Distance(player.transform.position, transform.position);
        if (dis < 1.06f)
        {
            player.player_data.SetInventory(value: item.data);
            FindObjectOfType<InputController>().onInteract -= OnInteract;
            Destroy(gameObject);
        }
        Debug.Log($"Interact {dis}");
    }

    private void Update()
    {
        if(icon)
        {
            icon.transform.Rotate(Vector3.up, 3, Space.World);

            if (item != null && item.IsValid)
                icon.sprite = item.Get<Sprite>(pname.icon);
            else
                icon.sprite = null;
            icon.enabled = icon.sprite != null;
        }
        if (amount)
        {
            amount.transform.LookAt(Camera.main.transform, Vector3.up);
            if (item != null && item.IsValid)
                amount.text = item.Has(pname.curStack) ? item.Get<int>(pname.curStack) > 1 ? item.Get<int>(pname.curStack).ToString() : string.Empty : string.Empty;
            else
                amount.text = string.Empty;
            amount.enabled = !string.IsNullOrEmpty(amount.text);
        }        

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WorldItem))]
public class WorldItemEdtir : Editor
{
    public override void OnInspectorGUI()
    {
        Repaint();
        var tar = (WorldItem)target;
        if(tar.item.IsValid)
        {
            foreach (var item in tar.item.data)
            {
                EditorGUILayout.LabelField(new GUIContent(item.Key.ToString()),  new GUIContent(item.Value.ToString()));
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No Item has be set!", MessageType.Warning);
        }
        base.OnInspectorGUI();
    }
}
#endif