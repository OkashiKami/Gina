using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

internal class Database
{
    private static string item_base_folder => Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");
    private static string loot_base_folder => Path.Combine(Application.streamingAssetsPath, "LootTables").Replace("\\", "/");

    private static List<Item> items = new List<Item>();
    private static List<Item> lootTables = new List<Item>();

    public static bool refreshing;
    public static bool loading_player_data;
    private static bool saving_player_data;

    private static bool saving_item_data;
    private static bool saving_loot_data;
    
    internal static Item GetItemByID(string value)
    {
        if (items == null || items.Count <= 0) Refresh();
        var item = items.Find(x => x.GetID == value);
        return item != null ? item : null;
    }
    internal static Item GetLootTableByID(string value)
    {
        if (lootTables == null || lootTables.Count <= 0) Refresh();
        var item = lootTables.Find(x => x.GetID == value);
        return item != null ? item : null;
    }

    internal static void SaveItem(Item value)
    {
        Directory.CreateDirectory(item_base_folder);
        if (saving_item_data) return;
        saving_item_data = true;
        if (!string.IsNullOrEmpty(value.file) && File.Exists(value.file))
            File.Delete(value.file);
        var name = value.Get<string>(paramname.name);
        var json = JsonConvert.SerializeObject(value.data, Formatting.Indented);
        name = name.Replace(" ", "_").ToLower();
        var savefile = Path.Combine(item_base_folder, $"{name}.json").Replace("\\", "/");
        File.WriteAllText(savefile, json, Encoding.UTF8);
        saving_item_data = false;
        Refresh();
    }
    internal static void SaveLoot(Item value)
    {
        Directory.CreateDirectory(loot_base_folder);
        if (saving_loot_data) return;
        saving_loot_data = true;
        if (!string.IsNullOrEmpty(value.file) && File.Exists(value.file))
            File.Delete(value.file);
        var name = value.Get<string>(paramname.name);
        var json = JsonConvert.SerializeObject(value.data, Formatting.Indented);
        name = name.Replace(" ", "_").ToLower();
        var savefile = Path.Combine(loot_base_folder, $"{name}.json").Replace("\\", "/");
        File.WriteAllText(savefile, json, Encoding.UTF8);
        saving_loot_data = false;
        Refresh();
    }


    public static void Delete(Item value)
    {
        if (!string.IsNullOrEmpty(value.file) && File.Exists(value.file))
            File.Delete(value.file);
        Refresh();
    }
    
    public static Item Duplicate(Item value)
    {
        var temp = value.Copy;
        temp.Set(paramname.name, temp.Get<string>(paramname.name) + " [COPY]");
        temp.file = string.Empty;
        temp._texture = null;
        temp._sprite = null;
        return temp;
    }
  
    public static void Refresh()
    {
        if (refreshing) return;

        refreshing = true;
        items = new List<Item>();
        lootTables = new List<Item>();

        foreach (var itemfile in Directory.GetFiles(item_base_folder, "*.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(itemfile, Encoding.UTF8);
            var item = JsonConvert.DeserializeObject<Dictionary<paramname, object>>(json);
            if (item != null)
            {
                var _item = new Item(item);
                _item.file = itemfile;
                items.Add(_item);
            }
        }
        foreach (var lootfile in Directory.GetFiles(loot_base_folder, "*.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(lootfile, Encoding.UTF8);
            var loot = JsonConvert.DeserializeObject<Dictionary<paramname, object>>(json);
            if (loot != null)
            {
                var _loot = new Item(loot);
                _loot.file = lootfile;
                lootTables.Add(_loot);
            }
        }
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        refreshing = false;
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
    internal static List<Item> GetLootTables
    {
        get
        {
            if (lootTables == null || lootTables.Count <= 0)
                Refresh();
            return lootTables;
        }
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