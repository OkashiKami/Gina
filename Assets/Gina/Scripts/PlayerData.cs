using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Item stats = DefaultStats;
    public Item[] inventory = new Item[30];
    public Item[] actionbar = new Item[12];
    public Item[] character = new Item[15];

    public static Item DefaultStats
    {
        get
        {
            var stats = new Item();
            stats.Set(Options.name, "Player");
            stats.Set(Options.level, 1);

            stats.Set(Options.curHealth, 1f);
            stats.Set(Options.curStamina, 1f);
            stats.Set(Options.curMana, 1f);
            stats.Set(Options.curExp, 0f);

            stats.Set(Options.maxHealth, 100f);
            stats.Set(Options.maxStamina, 100f);
            stats.Set(Options.maxMana, 100f);
            stats.Set(Options.maxExp, 1000f);

            stats.Set(Options.position, Vector3.zero);
            stats.Set(Options.rotation, Vector3.zero);

            return stats;
        }
    }

    public void SetStat(Options option, object value = null)
    {
        stats.Set(option, value);
        onStatsChanged?.Invoke(stats);
    }
    public void StatWasChanged() => onStatsChanged?.Invoke(stats);
    public delegate void OnStatChanged(Item data);
    public event OnStatChanged onStatsChanged;


    public void SetInventoryItem(int slot = -1, Item value = null)
    {
        if (slot < 0 || slot > inventory.Length - 1) return;
        inventory[slot] = value != null ? value.Copy : null;
        onInventoryItemChaged?.Invoke(this.inventory.ToList());
    }
    public delegate void OnInventoryItemChaged(List<Item> items);
    public event OnInventoryItemChaged onInventoryItemChaged;

    public void SetActionbarItem(int slot = -1, Item value = null)
    {
        if (slot < 0 || slot > actionbar.Length - 1) return;
        actionbar[slot] = value != null ? value.Copy : null;
        onActionbarItemChaged?.Invoke(this.actionbar.ToList());
    }
    public delegate void OnActionbarItemChaged(List<Item> items);
    public event OnActionbarItemChaged onActionbarItemChaged;

    public void SetCharacterItem(int slot = -1, Item value = null)
    {
        if (slot < 0 || slot > character.Length - 1) return;
        character[slot] = value != null ? value.Copy : null;
        onCharacterItemChaged?.Invoke(this.character.ToList());
    }
    public delegate void OnCharacterItemChaged(List<Item> items);
    public event OnCharacterItemChaged onCharacterItemChaged;
}