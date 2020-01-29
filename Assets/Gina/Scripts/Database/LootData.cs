using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LootData : IDisposable
{
    public string name;
    public List<LootEntry> items = new List<LootEntry>();
    [JsonIgnore] public string file;
    [JsonIgnore] public string id => $"{Application.productName}:{name}".Replace(" ", "_").ToLower();

    public LootData() { }
    public LootData(LootData value)
    {
        this.name = value.name;
        this.items = new List<LootEntry>(value.items);
        this.file = value.file;
    }
    [JsonIgnore] public LootData Copy => new LootData(this);
    [Serializable] public class LootEntry
    {
        public string item_indicator;
        public int dropPercentage;

        public LootEntry() { }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
