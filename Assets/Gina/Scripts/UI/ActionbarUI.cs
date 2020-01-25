using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ActionbarUI : MonoBehaviour
{
    private HorizontalLayoutGroup grid;
    public ActionbarSlot[] slots;

    private void Reset()
    {
        grid = GetComponentInChildren<HorizontalLayoutGroup>();
        slots = grid.GetComponentsInChildren<ActionbarSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].name = $"Act Slot {i + 1} + [{slots[i].requireType}]";
            slots[i].Set();
            slots[i].Awake();
            slots[i].Update();
        }
    }

    private void Awake()
    {
        if (slots == null || slots.Length <= 0) Reset();
        var player = FindObjectOfType<Player>();
        if (player)
            player.data.onActionbarChanged += OnActionbarChaged;
    }

    private void OnActionbarChaged(Dictionary<string, object>[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            slots[i].item = new Item(items[i]);
        }
    }
}
