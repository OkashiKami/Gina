using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


[Serializable]
public class Data
{
    public PlayerData player = new PlayerData();
    public string loottable = null;
    public InventoryData inventory = new InventoryData();
    public ActionbarData actionbar = new ActionbarData();
    public CharacterData character = new CharacterData();

    //values
    internal void Init()
    {
        player.Init();
        onLootTableChanged?.Invoke(LootTable);
        inventory.Init();
        actionbar.Init();
        character.Init();
    }

    // Events
    public delegate void OnLootTableChanged(LootTable vlue); public event OnLootTableChanged onLootTableChanged;

    internal void OnValidate()
    {
        Init();
    }

    public Data() { }

    public void SetPlayer<T>(PlayerData.Field field, T value = default) => player.Set(field, value);
   [JsonIgnore] public LootTable LootTable { get { return Database.Get<LootTable>(loottable);  } set { loottable = value != null ? value.GetID : null; onLootTableChanged?.Invoke(value); } }
    public void SetInventory(int index = -1, Item value = null) => inventory.Set(index, value);
    public void SetActionbar(int index = -1, Item value = null) => actionbar.Set(index, value);
    public void SetCharacter(int index = -1, Item value = null) => character.Set(index, value);


    public void Save(string savename = default)
    {
        var savepath = Path.Combine(Application.streamingAssetsPath, $"{(string.IsNullOrEmpty(savename) ? "player" : savename)}_.json").Replace("\\", "/");
        if (!string.IsNullOrEmpty(savepath) && File.Exists(savepath))
            File.Delete(savepath);

        File.WriteAllText(savepath, JsonConvert.SerializeObject(this, Formatting.Indented), Encoding.UTF8);
        Debug.Log("Data has been Saved");
    }
    public void Load(string loadname = default)
    {
        Directory.CreateDirectory(Application.streamingAssetsPath);
        var loadpath = Path.Combine(Application.streamingAssetsPath, $"{(string.IsNullOrEmpty(loadname) ? "player" : loadname)}_.json").Replace("\\", "/");
        if (File.Exists(loadpath))
        {
            var json = File.ReadAllText(loadpath, Encoding.UTF8);
            var d  = JsonConvert.DeserializeObject<Data>(json);
            if(d != null)
            {
                player.Set(d.player);
                LootTable = d.LootTable;
                inventory.Set(d.inventory);
                actionbar.Set(d.actionbar);
                character.Set(d.character);

                Debug.Log("data was loaded!");
            }
        }
    }
}

[Serializable]
public class PlayerData
{
    public enum Field { name, level, health, stamina, mama, exp, position, rotation }

    public string name;
    public int level = 1;
    public float[] health = new float[] { 100, 100f };
    public float[] stamina = new float[] { 100f, 100f };
    public float[] mana = new float[] { 100f, 100f };
    public float[] exp = new float[] { 0f, 1000f };
    public float[] position = new float[3] { 0f, 0f, 0f };
    public float[] rotation = new float[3] { 0f, 0f, 0f };


    public delegate void OnNameChaged(string value); public event OnNameChaged onNameChaged;
    public delegate void OnLevelChaged(int value); public event OnLevelChaged onLevelChaged;
    public delegate void OnHealthChagned(float[] value); public event OnHealthChagned onHealthChagned;
    public delegate void OnStaminaChagned(float[] value); public event OnStaminaChagned onStaminaChagned;
    public delegate void OnManaChagned(float[] value); public event OnManaChagned onManaChagned;
    public delegate void OnExperianceChagned(float[] value); public event OnExperianceChagned onExperianceChagned;
    public delegate void OnPositionChanged(Vector3 value); public event OnPositionChanged onPositionChanged;
    public delegate void OnRotationChanged(Vector3 value); public event OnRotationChanged onRotationChanged;

    [JsonIgnore] public string Name { get { return name; } set { name = value; onNameChaged?.Invoke(value); } }
    [JsonIgnore] public int Level { get { return level; } set { level = value; onLevelChaged?.Invoke(value); } }
    [JsonIgnore] public float[] Health { get { return health; } set { health = value; onHealthChagned?.Invoke(value); } }
    [JsonIgnore] public float[] Stamina { get { return stamina; } set { stamina = value; onStaminaChagned?.Invoke(value); } }
    [JsonIgnore] public float[] Mana { get { return mana; } set { mana = value; onManaChagned?.Invoke(value); } }
    [JsonIgnore] public float[] Exp { get { return exp; } set { exp = value; onExperianceChagned?.Invoke(value); } }
    [JsonIgnore] public Vector3 Position { get { return new Vector3(position[0], position[1], position[2]); } set { position = new float[] { value.x, value.y, value.z }; onPositionChanged?.Invoke(value); } }
    [JsonIgnore] public Vector3 Rotation { get { return new Vector3(rotation[0], rotation[1], rotation[2]); } set { rotation = new float[] { value.x, value.y, value.z }; onRotationChanged?.Invoke(value); } }
    public PlayerData() { }

    public void Set(Field field, object value = default)
    {
        if (value == null) return;
        switch(field)
        {
            case Field.name: Name = (string)value; break;
            case Field.level: Level = (int)value; break;
            case Field.health: Health = (float[])value; break;
            case Field.stamina: Stamina = (float[])value; break;
            case Field.mama: Mana = (float[])value; break;
            case Field.exp: Exp = (float[])value; break;
            case Field.position: Position = (Vector3)value; break;
            case Field.rotation: Rotation = (Vector3)value; break;
        }
    }
    public void Set(PlayerData value)
    {
        name = value.name;
        level = value.level;
        health = value.health;
        stamina = value.stamina;
        mana = value.mana;
        exp = value.exp;
        position = value.position;
        rotation = value.rotation;


        onNameChaged?.Invoke(name);
        onLevelChaged?.Invoke(level);
        onHealthChagned?.Invoke(health);
        onStaminaChagned?.Invoke(stamina);
        onManaChagned?.Invoke(mana);
        onExperianceChagned?.Invoke(exp);
        onPositionChanged?.Invoke(new Vector3(position[0], position[1], position[2]));
        onRotationChanged?.Invoke(new Vector3(rotation[0], rotation[1], rotation[2]));
    }

    public void Init()
    {
        onNameChaged?.Invoke(name);
        onLevelChaged?.Invoke(level);
        onHealthChagned?.Invoke(health);
        onStaminaChagned?.Invoke(stamina);
        onManaChagned?.Invoke(mana);
        onExperianceChagned?.Invoke(exp);
        onPositionChanged?.Invoke(new Vector3(position[0], position[1], position[2]));
        onRotationChanged?.Invoke(new Vector3(rotation[0], rotation[1], rotation[2]));
    }
}

[Serializable]
public class InventoryData
{
    public Item[] data = new Item[30];
    public bool isFull
    {
        get
        {
            var used = data.ToList().FindAll(x => x != null && x.IsValid);
            return used.ToArray().Length >= data.Length;
        }
    }
    public delegate void OnChanged(Item[] value);
    public event OnChanged onChanged;
    public InventoryData() 
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = null;
        }
    }
    public void Set(int index = -1, Item value = null)
    {
        if (index >= 0)
        {
            if (value != null && value.IsValid)
                data[index] = value;
            else
                data[index] = null;
        }
        else
        {
            bool alreadyAdded = false;
            // Check every slot to see if item is already there then add onto it if the item is stakable
            for (int i = 0; i < data.Length; i++)
            {
                if (value == null) break;
                var _itemdata = data[i];
                if (_itemdata != null)
                {
                    var _item = _itemdata;
                    if (_item.IsValid && _item.GetID == value.GetID)
                    {
                        if (_item.isStackable && _item.curStack < _item.maxStack)
                        {
                            _item.curStack = _item.curStack + value.curStack;
                            data[i] = _item;
                            alreadyAdded = true;
                            break;
                        }
                    }
                }
            }


            if (!alreadyAdded)
            {
                // Add the item to the first empty slot
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null || !data[i].IsValid)
                    {
                        if (value != null && value.IsValid)
                            data[i] = value;
                        else
                            data[i] = null;
                        break;
                    }

                    var slot = data[i];
                    if(value != null && value.IsValid)
                    {
                        if (value.GetID == slot.GetID)
                        {
                            if (slot.isStackable)
                            {
                                if (slot.curStack < slot.maxStack)
                                {
                                    slot.curStack++;
                                    break;
                                }
                            }
                            else continue;
                        }
                    }
                }
            }
        }
        onChanged?.Invoke(data);
    }
    public void Set(InventoryData value)
    {
        Array.Copy(value.data, 0, data, 0, value.data.Length);
        onChanged?.Invoke(data);
    }
    public void Init() => onChanged?.Invoke(data);
}
[Serializable]
public class ActionbarData
{
    public Item[] data = new Item[12];
    public delegate void OnChanged(Item[] value);
    public event OnChanged onChanged;

    public ActionbarData()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = null;
        }
    }
    public void Set(int index = -1, Item value = null)
    {
        if (index >= 0)
        {
            if (value != null && value.IsValid)
                data[index] = value;
            else
                data[index] = null;
        }
        else
        {
            bool alreadyAdded = false;
            // Check every slot to see if item is already there then add onto it if the item is stakable
            for (int i = 0; i < data.Length; i++)
            {
                if (value == null) break;
                var _itemdata = data[i];
                if (_itemdata != null)
                {
                    var _item = _itemdata;
                    if (_item.IsValid && _item.GetID == value.GetID)
                    {
                        if (_item.isStackable && _item.curStack < _item.maxStack)
                        {
                            _item.curStack = _item.curStack + value.curStack;
                            data[i] = _item;
                            alreadyAdded = true;
                            break;
                        }
                    }
                }
            }


            if (!alreadyAdded)
            {
                // Add the item to the first empty slot
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null || !data[i].IsValid)
                    {
                        if (value != null && value.IsValid)
                            data[i] = value;
                        else
                            data[i] = null;
                        break;
                    }

                    var slot = data[i];
                    if (value != null && value.IsValid)
                    {
                        if (value.GetID == slot.GetID)
                        {
                            if (slot.isStackable)
                            {
                                if (slot.curStack < slot.maxStack)
                                {
                                    slot.curStack++;
                                    break;
                                }
                            }
                            else continue;
                        }
                    }
                }
            }
        }
        onChanged?.Invoke(data);
    }
    public void Set(ActionbarData value)
    {
        Array.Copy(value.data, 0, data, 0, value.data.Length);
        onChanged?.Invoke(data);
    }
    public void Init() => onChanged?.Invoke(data);
}
[Serializable]
public class CharacterData
{
    public Item[] data = new Item[15];
    public delegate void OnChanged(Item[] value);
    public event OnChanged onChanged;
    public CharacterData()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = null;
        }
    }
    public void Set(int index = -1, Item value = null)
    {
        if (index >= 0)
        {
            if (value != null && value.IsValid)
                data[index] = value;
            else
                data[index] = null;
        }
        else
        {
            bool alreadyAdded = false;
            // Check every slot to see if item is already there then add onto it if the item is stakable
            for (int i = 0; i < data.Length; i++)
            {
                if (value == null) break;
                var _itemdata = data[i];
                if (_itemdata != null)
                {
                    var _item = _itemdata;
                    if (_item.IsValid && _item.GetID == value.GetID)
                    {
                        if (_item.isStackable && _item.curStack < _item.maxStack)
                        {
                            _item.curStack = _item.curStack + value.curStack;
                            data[i] = _item;
                            alreadyAdded = true;
                            break;
                        }
                    }
                }
            }


            if (!alreadyAdded)
            {
                // Add the item to the first empty slot
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == null || !data[i].IsValid)
                    {
                        if (value != null && value.IsValid)
                            data[i] = value;
                        else
                            data[i] = null;
                        break;
                    }

                    var slot = data[i];
                    if (value != null && value.IsValid)
                    {
                        if (value.GetID == slot.GetID)
                        {
                            if (slot.isStackable)
                            {
                                if (slot.curStack < slot.maxStack)
                                {
                                    slot.curStack++;
                                    break;
                                }
                            }
                            else continue;
                        }
                    }
                }
            }
        }
        onChanged?.Invoke(data);
    }
    public void Set(CharacterData value)
    {
        Array.Copy(value.data, 0, data, 0, value.data.Length);
        onChanged?.Invoke(data);
    }
    public void Init() => onChanged?.Invoke(data);
}