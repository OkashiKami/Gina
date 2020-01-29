using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

internal class Database
{
    public static void Save<T>(T value)
    {
        var type = typeof(T);

        if (type.Equals(typeof(PlayerData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Players").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var player = (PlayerData)(object)value;
            var file = Path.Combine(dir, $"{player.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
            var json = JsonConvert.SerializeObject(player, Formatting.Indented);
            if (File.Exists(file)) File.Delete(file);
            File.WriteAllText(file, json, Encoding.ASCII);
        }
        if (type.Equals(typeof(NpcData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "NPCs").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var npc = (NpcData)(object)value;
            var file = Path.Combine(dir, $"{npc.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
            var json = JsonConvert.SerializeObject(npc, Formatting.Indented);
            if (File.Exists(file)) File.Delete(file);
            File.WriteAllText(file, json, Encoding.ASCII);
        }
        if (type.Equals(typeof(ItemData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var item = (ItemData)(object)value;
            var file = Path.Combine(dir, $"{item.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
            if (File.Exists(file)) File.Delete(file);
            File.WriteAllText(file, json, Encoding.ASCII);
        }
        if (type.Equals(typeof(QuestData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Quests").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var quest = (QuestData)(object)value;
            var file = Path.Combine(dir, $"{quest.title.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
            var json = JsonConvert.SerializeObject(quest, Formatting.Indented);
            if (File.Exists(file)) File.Delete(file);
            File.WriteAllText(file, json, Encoding.ASCII);
        }
        if (type.Equals(typeof(LootData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "LootTables").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var lootTable = (LootData)(object)value;
            var file = Path.Combine(dir, $"{lootTable.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
            var json = JsonConvert.SerializeObject(lootTable, Formatting.Indented);
            if (File.Exists(file)) File.Delete(file);
            File.WriteAllText(file, json, Encoding.ASCII);
        }
    }
    public static void Save<T>(List<T> values)
    {
        var type = typeof(T);

        if (type.Equals(typeof(PlayerData)))
        {
            foreach (var value in values)
            {
                var dir = Path.Combine(Application.streamingAssetsPath, "Players").Replace("\\", "/");
                Directory.CreateDirectory(dir);
                var player = (PlayerData)(object)value;
                var file = Path.Combine(dir, $"{player.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
                var json = JsonConvert.SerializeObject(player, Formatting.Indented);
                if (File.Exists(file)) File.Delete(file);
                File.WriteAllText(file, json, Encoding.ASCII);
            }
        }
        if (type.Equals(typeof(NpcData)))
        {
            foreach (var value in values)
            {
                var dir = Path.Combine(Application.streamingAssetsPath, "NPCs").Replace("\\", "/");
                Directory.CreateDirectory(dir);
                var npc = (NpcData)(object)value;
                var file = Path.Combine(dir, $"{npc.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
                var json = JsonConvert.SerializeObject(npc, Formatting.Indented);
                if (File.Exists(file)) File.Delete(file);
                File.WriteAllText(file, json, Encoding.ASCII);
            }
        }
        if (type.Equals(typeof(ItemData)))
        {
            foreach (var value in values)
            {
                var dir = Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");
                Directory.CreateDirectory(dir);
                var item = (ItemData)(object)value;
                var file = Path.Combine(dir, $"{item.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
                var json = JsonConvert.SerializeObject(item, Formatting.Indented);
                if (File.Exists(file)) File.Delete(file);
                File.WriteAllText(file, json, Encoding.ASCII);
            }
        }
        if (type.Equals(typeof(QuestData)))
        {
            foreach (var value in values)
            {
                var dir = Path.Combine(Application.streamingAssetsPath, "Quests").Replace("\\", "/");
                Directory.CreateDirectory(dir);
                var quest = (QuestData)(object)value;
                var file = Path.Combine(dir, $"{quest.title.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
                var json = JsonConvert.SerializeObject(quest, Formatting.Indented);
                if (File.Exists(file)) File.Delete(file);
                File.WriteAllText(file, json, Encoding.ASCII);
            }
        }
        if (type.Equals(typeof(LootData)))
        {
            foreach (var value in values)
            {
                var dir = Path.Combine(Application.streamingAssetsPath, "LootTables").Replace("\\", "/");
                Directory.CreateDirectory(dir);
                var lootTable = (LootData)(object)value;
                var file = Path.Combine(dir, $"{lootTable.name.Replace(" ", "_").ToLower()}.json").Replace("\\", "/");
                var json = JsonConvert.SerializeObject(lootTable, Formatting.Indented);
                if (File.Exists(file)) File.Delete(file);
                File.WriteAllText(file, json, Encoding.ASCII);
            }
        }
    }
    public static T Load<T>( string value)
    {
        var type = typeof(T);

        if(type.Equals(typeof(PlayerData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Players").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var player = JsonConvert.DeserializeObject<PlayerData>(json);
                if (player.id == value || player.name == value)
                {
                    player.file = file;
                    return (T)(object)player;
                }
                else
                    player.Dispose();
            }
        }
        if (type.Equals(typeof(NpcData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "NPCs").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var npc = JsonConvert.DeserializeObject<NpcData>(json);
                if (npc.id == value || npc.name == value)
                {
                    npc.file = file;
                    return (T)(object)npc;
                }
                else
                    npc.Dispose();
            }
        }
        if (type.Equals(typeof(ItemData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var item = JsonConvert.DeserializeObject<ItemData>(json);
                if (item.id == value || item.name == value)
                {
                    item.file = file;
                    return (T)(object)item;
                }
                else
                    item.Dispose();
            }
        }
        if (type.Equals(typeof(QuestData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Quests").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var quests = JsonConvert.DeserializeObject<ItemData>(json);
                if (quests.id == value || quests.name == value)
                {
                    quests.file = file;
                    return (T)(object)quests;
                }
                else
                    quests.Dispose();
            }
        }
        if (type.Equals(typeof(LootData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "LootTables").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var lootTable = JsonConvert.DeserializeObject<ItemData>(json);
                if (lootTable.id == value || lootTable.name == value)
                {
                    lootTable.file = file;
                    return (T)(object)lootTable;
                }
                else
                    lootTable.Dispose();
            }
        }

        return default(T);
    }
    public static List<T> Load<T>()
    {
        var type = typeof(T);
        List<T> list = new List<T>();

        if (type.Equals(typeof(PlayerData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Players").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var player = JsonConvert.DeserializeObject<PlayerData>(json);
                if (player != null)
                {
                    player.file = file;
                    list.Add((T)(object)player);
                }
            }
            return list;
        }
        if (type.Equals(typeof(NpcData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "NPCs").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var npc = JsonConvert.DeserializeObject<NpcData>(json);
                if (npc != null)
                {
                    npc.file = file;
                    list.Add((T)(object)npc);
                }
            }
            return list;
        }
        if (type.Equals(typeof(ItemData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var item = JsonConvert.DeserializeObject<ItemData>(json);
                if (item != null && item.IsValid)
                {
                    item.file = file;
                    list.Add((T)(object)item);
                }
            }
            return list;
        }
        if (type.Equals(typeof(QuestData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Quests").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var quest = JsonConvert.DeserializeObject<QuestData>(json);
                if (quest != null)
                {
                    quest.file = file;
                    list.Add((T)(object)quest);
                }
            }
            return list;
        }
        if (type.Equals(typeof(LootData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "LootTables").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories))
            {
                var json = File.ReadAllText(file, Encoding.ASCII);
                var lootTable = JsonConvert.DeserializeObject<LootData>(json);
                if (lootTable != null)
                {
                    lootTable.file = file;
                    list.Add((T)(object)lootTable);
                }
            }
            return list;
        }

        return list;
    }

    public static void Delete<T>(T value)
    {
        var type = typeof(T);

        if (type.Equals(typeof(PlayerData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Players").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var player = (PlayerData)(object)value;
            var file = player.file;
            if (File.Exists(file)) File.Delete(file);
        }
        if (type.Equals(typeof(NpcData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "NPCs").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var npc = (NpcData)(object)value;
            var file = npc.file;
            if (File.Exists(file)) File.Delete(file);
        }
        if (type.Equals(typeof(ItemData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Items").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var item = (ItemData)(object)value;
            var file = item.file;
            if (File.Exists(file)) File.Delete(file);
        }
        if (type.Equals(typeof(QuestData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "Quests").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var quest = (QuestData)(object)value;
            var file = quest.file;
            if (File.Exists(file)) File.Delete(file);
        }
        if (type.Equals(typeof(LootData)))
        {
            var dir = Path.Combine(Application.streamingAssetsPath, "LootTables").Replace("\\", "/");
            Directory.CreateDirectory(dir);
            var lootTable = (LootData)(object)value;
            var file = lootTable.file;
            if (File.Exists(file)) File.Delete(file);
        }
    }

}