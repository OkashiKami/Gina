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
        var item = items.Find(x => x.GetID() == v);
        return item != null ? item : null;
    }

    private static List<Item> items = new List<Item>();
    public static bool loading_player_data;
    private static bool saving_player_data;

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
        if (loading_player_data) return;
        new Thread(new ThreadStart(() =>
        {
            loading_player_data = true;
            if (!string.IsNullOrEmpty(value.file) && File.Exists(value.file))
                File.Delete(value.file);
            var name = value.Get<string>(Options.name);
            var json = JsonConvert.SerializeObject(value.data, Formatting.Indented);
            name = name.Replace(" ", "_").ToLower();
            var savefile = Path.Combine(item_base_folder, $"{name}.json").Replace("\\", "/");
            File.WriteAllText(savefile, json, Encoding.UTF8);
            GameManager.ExecuteAction(Refresh);
            loading_player_data = false;
        })).Start();
    }
    internal static void Save(PlayerData value)
    {
        if (saving_player_data) return;
        new Thread(new ThreadStart(() =>
        {
            saving_player_data = true;
            var statjson = JsonConvert.SerializeObject(value._stats, Formatting.Indented);
            var invjson = JsonConvert.SerializeObject(value._inventory, Formatting.Indented);
            var actjson = JsonConvert.SerializeObject(value._actionbar, Formatting.Indented);
            var charjson = JsonConvert.SerializeObject(value._character, Formatting.Indented);
            var sb = new StringBuilder();
            sb.AppendLine(statjson);
            sb.AppendLine("-");
            sb.AppendLine(invjson);
            sb.AppendLine("-");
            sb.AppendLine(actjson);
            sb.AppendLine("-");
            sb.AppendLine(charjson);

            var savepath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
            if (!string.IsNullOrEmpty(savepath) && File.Exists(savepath))
            File.Delete(savepath);
           
            File.WriteAllText(savepath, sb.ToString(), Encoding.UTF8);
            Debug.Log("Saved");
            GameManager.ExecuteAction(Refresh);
            saving_player_data = false;

        })).Start();
    }

    internal static void LoadPlayerData(PlayerData output, Action action = null)
    {
        new Thread(new ThreadStart(() =>
        {
            loading_player_data = true;
            PlayerData data = null;
            Directory.CreateDirectory(Application.streamingAssetsPath);
            var loadpath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
            if (File.Exists(loadpath))
            {
                data = new PlayerData();
                var json = File.ReadAllText(loadpath, Encoding.UTF8).Split('-');
                data._stats = JsonConvert.DeserializeObject<Dictionary<Options, object>>(json[0]);
                data._inventory = JsonConvert.DeserializeObject<Dictionary<Options, object>[]>(json[1]);
                data._actionbar = JsonConvert.DeserializeObject<Dictionary<Options, object>[]>(json[2]);
                data._character = JsonConvert.DeserializeObject<Dictionary<Options, object>[]>(json[3]);

            }
            if (data == null)
            {
                data = new PlayerData();
                data._stats = PlayerData.DefaultStats.data;
                data._inventory = new Dictionary<Options, object>[30];
                data._actionbar = new Dictionary<Options, object>[12];
                data._character = new Dictionary<Options, object>[15];
            }
            else
            {
                output._stats = data._stats;
                output._inventory = data._inventory;
                output._actionbar = data._actionbar;
                output._character = data._character;
            }
            if (action != null)
                GameManager.ExecuteAction(action);
            loading_player_data = false;
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