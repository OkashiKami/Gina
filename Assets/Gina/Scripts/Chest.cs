using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChestState { Closed, Animation, Open }
public class Chest : MonoBehaviour
{
    public List<string> loottable = new List<string>();
    public ChestState state = ChestState.Closed;
    private Transform pivit;
    public bool locked = false;
    [Range(0, 150)] public float x;

    private void OnValidate()
    {
        if (!pivit)
            pivit = transform.Find("pivot");
        pivit.localRotation = Quaternion.Euler(locked ? 0f : -x, 0f, 0f);
    }

    private void Start()
    {
        var table = new List<string>();
        for (int i = 0; i < loottable.Count; i++)
        {
            table.Add(loottable[UnityEngine.Random.Range(0, loottable.Count)]);
        }
        foreach (var item in table)
        {
            //WorldItem.Create(Database.Get<ItemData>(item), transform.Find("spawnpoint").position, -1, atPoint: true);
        }
    }

    private void Update()
    {
        if (!pivit)
            pivit = transform.Find("pivot");

        switch(state)
        {
            case ChestState.Open:
                if (x < 150)
                    x += 75 * Time.deltaTime;
                else if (x >= 150)
                    state = ChestState.Animation;
                break;
            case ChestState.Animation:

                break;
            case ChestState.Closed:
                if (x > 0)
                    x -= 75 * Time.deltaTime;
                break;
        }
        x = Mathf.Clamp(x, 0f, 150f);

        pivit.localRotation = Quaternion.Euler(locked ? 0f : -x, 0f, 0f);
    }
}
