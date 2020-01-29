using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData
{
    
    public string title;
    public string background;
    public List<QuestObjective> objectives = new List<QuestObjective>();
    public List<QuestReward> reward = new List<QuestReward>();

    public bool isComplete
    {
        get
        {
            var complete = true;
            if (objectives.FindAll(x => !x.isComplete).Count > 0) complete = false;
            return complete;
        } 
    }
    public QuestData() { }
    public QuestData(QuestData value)
    {
        this.title = value.title;
        this.background = value.background;
        this.objectives = new List<QuestObjective>(value.objectives);
        this.reward = new List<QuestReward>(value.reward);
    }

    [JsonIgnore] public string file;

    [JsonIgnore] public string GetID
    {
        get
        {
            var _namespace = Application.productName.ToLower();
            var _name = title.Replace(" ", "_").ToLower();
            return $"{_namespace}:{_name}";
        }
    }
    [JsonIgnore] public QuestData Copy => new QuestData(this);
}

[Serializable]
public class QuestObjective
{
    public string verb;
    public int amount;
    public string what;

    public bool isComplete;

    public QuestObjective() { }
}

[Serializable]
public class QuestReward
{
    public string item;
    public int amount;

    public QuestReward() { }
}