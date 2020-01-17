﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(CanvasGroup))]
public class ActionbarUI : MonoBehaviour
{
    private HorizontalLayoutGroup grid;
    private ActionbarSlot[] slots;
    private void Reset()
    {
        grid = GetComponentInChildren<HorizontalLayoutGroup>();
        slots = grid.GetComponentsInChildren<ActionbarSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].name = $"Act Slot {i + 1}";
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
            player.player_data.onActionbarChanged += OnActionbarChaged;
    }

    private void OnActionbarChaged(Dictionary<Options, object>[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            slots[i].Set(new Item(items[i]));
        }
    }
}
