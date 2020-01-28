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
        StartCoroutine(Connect());
    }

    private IEnumerator Connect()
    {
        Player player = null;
        Debug.Log("Waiting for player");
        yield return new WaitUntil(() =>
        {
            player = FindObjectOfType<Player>();
            return player;
        });
        Debug.Log("Player Found!");
        OnChanged(player.data.actionbar.data);
        player.data.actionbar.onChanged += OnChanged;
    }

    private void OnChanged(Item[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            var slot = slots[i];
            var value = values[i];

            if (value != null && value.IsValid)
                slot.item = value;
            else
                slot.item = null;
        }
    }
}
