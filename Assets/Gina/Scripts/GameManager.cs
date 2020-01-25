using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static int maxLevel = 80;
    public static List<Action> actions = new List<Action>();
    // Start is called before the first frame update
    void Start()
    {
        //WorldItem.Create(Database.GetLootTableByID("gina:basic_chest"), point: true, table: true);
    }

    // Update is called once per frame
    void Update()
    {
        if(actions.Count > 0)
        {
            actions[0]?.Invoke();
            actions.RemoveAt(0);
        }
    }

    internal static void ExecuteAction(Action action) => actions.Add(action);
}
