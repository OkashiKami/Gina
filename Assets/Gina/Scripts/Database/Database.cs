using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

internal class Database
{
    private static string item_base_folder => Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");
    private static string loot_base_folder => Path.Combine(Application.streamingAssetsPath, "LootTables").Replace("\\", "/");
    private static string quest_base_folder => Path.Combine(Application.streamingAssetsPath, "Quests").Replace("\\", "/");

    private static List<Item> items = new List<Item>();
    private static List<LootTable> lootTables = new List<LootTable>();
    private static List<Quest> quests = new List<Quest>();

    public static bool refreshing;
    public static bool loading;
    private static bool saving;
    
    public static List<T> GetAll<T>(bool auto = true)
    {
        if (typeof(T).Equals(typeof(Item)))
        {
            if (items == null || items.Count <= 0 && auto)
                Refresh();
            return (List<T>)(object)items;
        }
        else if (typeof(T).Equals(typeof(LootTable)))
        {
            if (lootTables == null || lootTables.Count <= 0 && auto)
                Refresh();
            return (List<T>)(object)lootTables;
        }
        else if (typeof(T).Equals(typeof(Quest)))
        {
            if (quests == null || quests.Count <= 0 && auto)
                Refresh();
            return (List<T>)(object)quests;
        }
        else
            return default(List<T>);
    }
    public static T Get<T>(string value)
    {
        if (typeof(T).Equals(typeof(Item)))
        {
            if (items == null || items.Count <= 0) Refresh();
            var item = items.Find(x => x.GetID == value);
            return item != null &&  item.IsValid ? (T)(object)item : default(T);
        }
        else if (typeof(T).Equals(typeof(LootTable)))
        {
            if (lootTables == null || lootTables.Count <= 0) Refresh();
            var loot = lootTables.Find(x => x.GetID == value);
            return loot != null ? (T)(object)loot : default(T);
        }
        else 
            return default(T);
    }
    public static void Save<T>(T value)
    {
        if(typeof(T).Equals(typeof(Item)))
        {
            var item = (Item)(object)value;
            Directory.CreateDirectory(item_base_folder);
            if (saving) return;
            saving = true;
            if (!string.IsNullOrEmpty(item.file) && File.Exists(item.file))
                File.Delete(item.file);
            var name = item.name;
            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
            name = name.Replace(" ", "_").ToLower();
            var savefile = Path.Combine(item_base_folder, $"{name}.json").Replace("\\", "/");
            File.WriteAllText(savefile, json, Encoding.UTF8);
            saving = false;
            Refresh();
        }
        else if(typeof(T).Equals(typeof(LootTable)))
        {
            var loot = (LootTable)(object)value;
            Directory.CreateDirectory(loot_base_folder);
            if (saving) return;
            saving = true;
            if (!string.IsNullOrEmpty(loot.file) && File.Exists(loot.file))
                File.Delete(loot.file);
            var name = loot.name;
            var json = JsonConvert.SerializeObject(loot, Formatting.Indented);
            name = name.Replace(" ", "_").ToLower();
            var savefile = Path.Combine(loot_base_folder, $"{name}.json").Replace("\\", "/");
            File.WriteAllText(savefile, json, Encoding.UTF8);
            saving = false;
            Refresh();
        }
        else if (typeof(T).Equals(typeof(Quest)))
        {
            var quest = (Quest)(object)value;
            Directory.CreateDirectory(quest_base_folder);
            if (saving) return;
            saving = true;
            if (!string.IsNullOrEmpty(quest.file) && File.Exists(quest.file))
                File.Delete(quest.file);
            var name = quest.title;
            var json = JsonConvert.SerializeObject(quest, Formatting.Indented);
            name = name.Replace(" ", "_").ToLower();
            var savefile = Path.Combine(quest_base_folder, $"{name}.json").Replace("\\", "/");
            File.WriteAllText(savefile, json, Encoding.UTF8);
            saving = false;
            Refresh();
        }
    }
    public static void Delete<T>(T value)
    {
        if (typeof(T).Equals(typeof(Item)))
        {
            var item = (Item)(object)value;
            if (!string.IsNullOrEmpty(item.file) && File.Exists(item.file))
                File.Delete(item.file);
        }
        else if (typeof(T).Equals(typeof(LootTable)))
        {
            var loot = (LootTable)(object)value;
            if (!string.IsNullOrEmpty(loot.file) && File.Exists(loot.file))
                File.Delete(loot.file);
        }
        else if (typeof(T).Equals(typeof(Quest)))
        {
            var quest = (Quest)(object)value;
            if (!string.IsNullOrEmpty(quest.file) && File.Exists(quest.file))
                File.Delete(quest.file);
        }
        Refresh();
    }
    public static T Duplicate<T>(T value)
    {
        if (typeof(T).Equals(typeof(Item)))
        {
            var temp = ((Item)(object)value).Copy;
            temp.name = temp.name + " [COPY]";
            temp.file = string.Empty;
            temp._texture = null;
            temp._sprite = null;
            return (T)(object)temp;
        }
        else if (typeof(T).Equals(typeof(LootTable)))
        {
            var temp = ((LootTable)(object)value).Copy;
            temp.name = temp.name + " [COpy]";
            temp.file = string.Empty;
            return (T)(object)temp;
        }
        else if(typeof(T).Equals(typeof(Quest)))
        {
            var temp = ((Quest)(object)value).Copy;
            temp.title = temp.title + " [COpy]";
            temp.file = string.Empty;
            return (T)(object)temp;
        }
        else
            return default(T);
    }
  
    public static void Refresh()
    {
        if (refreshing) return;

        refreshing = true;
        items = new List<Item>();
        lootTables = new List<LootTable>();
        quests = new List<Quest>();

        Directory.CreateDirectory(item_base_folder);
        Directory.CreateDirectory(loot_base_folder);
        Directory.CreateDirectory(quest_base_folder);


        foreach (var itemfile in Directory.GetFiles(item_base_folder, "*.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(itemfile, Encoding.UTF8);
            var item = JsonConvert.DeserializeObject<Item>(json);
            if (item != null)
            {
                item.file = itemfile;
                items.Add(item);
            }
        }
        foreach (var lootfile in Directory.GetFiles(loot_base_folder, "*.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(lootfile, Encoding.UTF8);
            var loot = JsonConvert.DeserializeObject<LootTable>(json);
            if (loot != null)
            {
                loot.file = lootfile;
                lootTables.Add(loot);
            }
        }
        foreach (var questfile in Directory.GetFiles(quest_base_folder, "*.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(questfile, Encoding.UTF8);
            var quest = JsonConvert.DeserializeObject<Quest>(json);
            if (quest != null)
            {
                quest.file = questfile;
                quests.Add(quest);
            }
        }
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        refreshing = false;
    }
}