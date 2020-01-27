using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class LootTable
{
    public string name;
    public List<LootEntry> items = new List<LootEntry>();
    [JsonIgnore] public string file;


    public LootTable() { }
    public LootTable(LootTable value)
    {
        this.name = value.name;
        this.items = new List<LootEntry>(value.items);
        this.file = value.file;
    }
    [JsonIgnore] public LootTable Copy => new LootTable(this);
    [JsonIgnore] public string GetID
    {
        get
        {
            var _namespace = Application.productName.ToLower();
            var _name = name.Replace(" ", "_").ToLower();
            return $"{_namespace}:{_name}";
        }
    }
}

[Serializable]
public class LootEntry
{
    public string item;
    public int dropPercentage;

    public LootEntry() { }
}