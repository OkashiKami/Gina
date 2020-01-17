using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

internal class Database
{
    private static string item_base_folder => Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");

    internal static Item GetItemByID(string v)
    {
        if (items == null || items.Count <= 0) Refresh();
        var item = items.Find(x => x.id == v);
        return item != null ? item : null;
    }

    private static List<Item> items = new List<Item>();
    public static bool saving;

    public static void Refresh()
    {
        items = new List<Item>();
        foreach (var itemfile in Directory.GetFiles(item_base_folder, "*.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(itemfile, Encoding.UTF8);
            var item = JsonConvert.DeserializeObject<Dictionary<Options, object>>(json);
            if (item != null)
            {
                var _item = new Item(item);
                _item.file = itemfile;
                items.Add(_item);
            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

    internal static List<Item> GetItems 
    {
        get
        {
            if (items == null || items.Count <= 0)
                Refresh();
            return items;
        }
    }

    internal static void Save(Item value)
    {
        new Thread(new ThreadStart(() =>
        {
            if (!string.IsNullOrEmpty(value.file) && File.Exists(value.file))
                File.Delete(value.file);
            var name = value.Get<string>(Options.name);
            var json = JsonConvert.SerializeObject(value.data, Formatting.Indented);
            name = name.Replace(" ", "_").ToLower();
            var savefile = Path.Combine(item_base_folder, $"{name}.json").Replace("\\", "/");
            File.WriteAllText(savefile, json, Encoding.UTF8);
            GameManager.ExecuteAction(Refresh);

        })).Start();
    }
    internal static void Save(PlayerData value)
    {
        new Thread(new ThreadStart(() =>
        {
            var savepath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
            if (!string.IsNullOrEmpty(savepath) && File.Exists(savepath))
                File.Delete(savepath);
            var json = JsonConvert.SerializeObject(value, Formatting.Indented);
            File.WriteAllText(savepath, json, Encoding.UTF8);
            Debug.Log("Saved");
            GameManager.ExecuteAction(Refresh);

        })).Start();
    }

    internal static void LoadPlayerData(PlayerData into, Action action = null)
    {
        new Thread(new ThreadStart(() =>
        {
            saving = true;
            PlayerData data = null;
            Directory.CreateDirectory(Application.streamingAssetsPath);
            var loadpath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
            if (File.Exists(loadpath))
            {
                var json = File.ReadAllText(loadpath, Encoding.UTF8);
                data = JsonConvert.DeserializeObject<PlayerData>(json);
            }
            if (data == null)
            {
                data = new PlayerData();
                data.stats = PlayerData.DefaultStats;
                data.inventory = new Item[30];
                data.actionbar = new Item[12];
                data.character = new Item[15];
            }
            else
            {
                ((PlayerData)(object)into).stats = new Item(data.stats.data);
                Array.Copy(data.inventory, ((PlayerData)(object)into).inventory, 30);
                Array.Copy(data.actionbar, ((PlayerData)(object)into).actionbar, 12);
                Array.Copy(data.character, ((PlayerData)(object)into).character, 15);
            }
            if (action != null)
                GameManager.ExecuteAction(action);
            saving = false;
        })).Start();
    }

    internal static void IsValidObject(string v = null)
    {
#if UNITY_EDITOR
        var msg = string.Empty;
        var type = UnityEditor.MessageType.None;
        if (string.IsNullOrEmpty(v))
        {
            msg = "Input field can not be empty.";
            type = UnityEditor.MessageType.Error;
        }
        var temp = Resources.Load(v);
        if(temp != null)
        {
            msg = "Resource Link Passed.";
            type = UnityEditor.MessageType.Info;
        }
        else
        {
            msg = "Resource Link Faild, can't find resouce at that location.";
            type = UnityEditor.MessageType.Error;
        }
        UnityEditor.EditorGUILayout.HelpBox(msg, type);
#endif
    }
}