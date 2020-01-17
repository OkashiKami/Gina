using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Dictionary<Options, object> _stats = DefaultStats.data;
    public Dictionary<Options, object>[] _inventory = new Dictionary<Options, object>[30];
    public Dictionary<Options, object>[] _actionbar = new Dictionary<Options, object>[12];
    public Dictionary<Options, object>[] _character = new Dictionary<Options, object>[15];

    public PlayerData() { }

    public Item stats { get => new Item(_stats); set => _stats = value.data; }
    public Item[] inventory { get => _inventory.Select(x => new Item(x)).ToArray(); set => _inventory = value.Select(x => x.data).ToArray(); }
    public Item[] actionbar { get => _actionbar.Select(x => new Item(x)).ToArray(); set => _actionbar = value.Select(x => x.data).ToArray(); }
    public Item[] character { get => _character.Select(x => new Item(x)).ToArray(); set => _character = value.Select(x => x.data).ToArray(); }



        


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