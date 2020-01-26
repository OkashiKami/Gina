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
    public string name;
    public int level = 1;
    public float[] health = new float[] { 100, 100f };
    public float[] stamina = new float[] { 100f, 100f };
    public float[] mana = new float[] { 100f, 100f };
    public float[] exp = new float[] { 0f, 1000f };
    public float[] position = new float[3] { 0f, 0f, 0f };
    public float[] rotation = new float[3] { 0f, 0f, 0f };
    public string loottable = null;
    public Inventory inventory = new Inventory();
    public Actionbar actionbar = new Actionbar();
    public Character character = new Character();

    //values
    internal void Init()
    {
        onNameChaged?.Invoke(Name);
        onLevelChaged?.Invoke(Level);
        onHealthChagned?.Invoke(Health);
        onStaminaChagned?.Invoke(Stamina);
        onManaChagned?.Invoke(Mana);
        onExperianceChagned?.Invoke(Exp);
        onPositionChanged?.Invoke(Position);
        onRotationChanged?.Invoke(Rotation);
        onLootTableChanged?.Invoke(LootTable);
        inventory.Init();
        actionbar.Init();
        character.Init();
    }

    // Events
    public delegate void OnNameChaged(string value); public event OnNameChaged onNameChaged;
    public delegate void OnLevelChaged(int value);  public event OnLevelChaged onLevelChaged;
    public delegate void OnHealthChagned(float[] value); public event OnHealthChagned onHealthChagned;
    public delegate void OnStaminaChagned(float[] value); public event OnStaminaChagned onStaminaChagned;
    public delegate void OnManaChagned(float[] value); public event OnManaChagned onManaChagned;
    public delegate void OnExperianceChagned(float[] value); public event OnExperianceChagned onExperianceChagned;
    public delegate void OnPositionChanged(Vector3 value); public event OnPositionChanged onPositionChanged;
    public delegate void OnRotationChanged(Vector3 value); public event OnRotationChanged onRotationChanged;
    public delegate void OnLootTableChanged(Item vlue); public event OnLootTableChanged onLootTableChanged;

    public Data() { }

    
    [JsonIgnore] public string Name { get { return name; } set { name = value; onNameChaged?.Invoke(value); } }
    [JsonIgnore] public int Level { get { return level; } set { level = value; onLevelChaged?.Invoke(value); } }
    [JsonIgnore] public float[] Health { get { return health; } set { health = value; onHealthChagned?.Invoke(value); } }
    [JsonIgnore] public float[] Stamina { get { return stamina; } set { stamina = value; onStaminaChagned?.Invoke(value); } }
    [JsonIgnore] public float[] Mana { get { return mana; } set { mana = value; onManaChagned?.Invoke(value); } }
    [JsonIgnore] public float[] Exp { get { return exp; } set { exp = value; onExperianceChagned?.Invoke(value); } }
    [JsonIgnore] public Vector3 Position { get { return new Vector3(position[0], position[1], position[2]); } set { position = new float[] { value.x, value.y, value.z }; onPositionChanged?.Invoke(value); } }
    [JsonIgnore] public Vector3 Rotation { get { return new Vector3(rotation[0], rotation[1], rotation[2]); } set { rotation = new float[] { value.x, value.y, value.z }; onRotationChanged?.Invoke(value); } }
    [JsonIgnore] public Item LootTable { get { return Database.GetLootTableByID(loottable);  } set { loottable = value != null ? value.GetID : null; onLootTableChanged?.Invoke(value); } }
    public void SetInventory(int index = -1, Dictionary<string, object> value = null) => inventory.Set(index, value);
    public void SetActionbar(int index = -1, Dictionary<string, object> value = null) => actionbar.Set(index, value);
    public void SetCharacter(int index = -1, Dictionary<string, object> value = null) => character.Set(index, value);


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
                Name = d.Name;
                Level = d.Level;
                Health = d.Health;
                Stamina = d.Stamina;
                Mana = d.Mana;
                Exp = d.Exp;
                Position = d.Position;
                Rotation = d.Rotation;
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
public class Inventory
{
    public Dictionary<string, object>[] data = new Dictionary<string, object>[30];
    public bool isFull
    {
        get
        {
            var used = data.Select(x => new Item(x)).ToList().FindAll(x => x != null && x.IsValid);
            return used.ToArray().Length >= data.Length;
        }
    }
    public delegate void OnChanged(Dictionary<string, object>[] value);
    public event OnChanged onChanged;
    public Inventory() { }
    public void Set(int index = -1, Dictionary<string, object> value = null)
    {
        
        if (index >= 0) data[index] = value;
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
                    var _item = new Item(_itemdata);
                    if (_item.IsValid && _item.GetID == new Item(value).GetID)
                    {
                        if(_item.IsStackable && _item.Get<int>(pname.curStack) < _item.Get<int>(pname.maxStack))
                        {
                            _item.Set(pname.curStack, _item.Get<int>(pname.curStack) + new Item(value).Get<int>(pname.curStack));
                            data[i] = _item.data;
                            alreadyAdded = true;
                            break;
                        }
                    }
                }
            }


            if(!alreadyAdded)
            {
                // Add the item to the first empty slot
                for (int i = 0; i < data.Length; i++)
                {
                    if(data[i] == null || !new Item(data[i]).IsValid)
                    {
                        data[i] = value;
                        break;
                    }

                    var slot = new Item(data[i]);
                    var item = new Item(value);
                    if(item.GetID == slot.GetID)
                    {
                        if (slot.IsStackable)
                        {
                            if (slot.Get<int>(pname.curStack) < slot.Get<int>(pname.maxStack))
                            {
                                slot.Set(pname.curStack, slot.Get<int>(pname.curStack) + 1);
                                break;
                            }
                        }
                        else continue;
                    }
                }
            }
        }
        onChanged?.Invoke(data);
    }
    public void Set(Inventory value)
    {
        Array.Copy(value.data, 0, data, 0, value.data.Length);
        onChanged?.Invoke(data);
    }

    public void Init() => onChanged?.Invoke(data);
}
[Serializable]
public class Actionbar
{
    public Dictionary<string, object>[] data = new Dictionary<string, object>[12];
    public delegate void OnChanged(Dictionary<string, object>[] value);
    public event OnChanged onChanged;

    public Actionbar() { }
    public void Set(int index = -1, Dictionary<string, object> value = null)
    {

        if (index >= 0) data[index] = value;
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
                    var _item = new Item(_itemdata);
                    if (_item.IsValid && _item.GetID == new Item(value).GetID)
                    {
                        if (_item.IsStackable && _item.Get<int>(pname.curStack) < _item.Get<int>(pname.maxStack))
                        {
                            _item.Set(pname.curStack, _item.Get<int>(pname.curStack) + new Item(value).Get<int>(pname.curStack));
                            data[i] = _item.data;
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
                    if (data[i] == null || !new Item(data[i]).IsValid)
                    {
                        data[i] = value;
                        break;
                    }

                    var slot = new Item(data[i]);
                    var item = new Item(value);
                    if (item.GetID == slot.GetID)
                    {
                        if (slot.IsStackable)
                        {
                            if (slot.Get<int>(pname.curStack) < slot.Get<int>(pname.maxStack))
                            {
                                slot.Set(pname.curStack, slot.Get<int>(pname.curStack) + 1);
                                break;
                            }
                        }
                        else continue;
                    }
                }
            }
        }
        onChanged?.Invoke(data);
    }
    public void Set(Actionbar value)
    {
        Array.Copy(value.data, 0, data, 0, value.data.Length);
        onChanged?.Invoke(data);
    }
    public void Init() => onChanged?.Invoke(data);
}
[Serializable]
public class Character
{
    public Dictionary<string, object>[] data = new Dictionary<string, object>[15];
    public delegate void OnChanged(Dictionary<string, object>[] value);
    public event OnChanged onChanged;
    public Character() { }
    public void Set(int index = -1, Dictionary<string, object> value = null)
    {

        if (index >= 0) data[index] = value;
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
                    var _item = new Item(_itemdata);
                    if (_item.IsValid && _item.GetID == new Item(value).GetID)
                    {
                        if (_item.IsStackable && _item.Get<int>(pname.curStack) < _item.Get<int>(pname.maxStack))
                        {
                            _item.Set(pname.curStack, _item.Get<int>(pname.curStack) + new Item(value).Get<int>(pname.curStack));
                            data[i] = _item.data;
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
                    if (data[i] == null || !new Item(data[i]).IsValid)
                    {
                        data[i] = value;
                        break;
                    }

                    var slot = new Item(data[i]);
                    var item = new Item(value);
                    if (item.GetID == slot.GetID)
                    {
                        if (slot.IsStackable)
                        {
                            if (slot.Get<int>(pname.curStack) < slot.Get<int>(pname.maxStack))
                            {
                                slot.Set(pname.curStack, slot.Get<int>(pname.curStack) + 1);
                                break;
                            }
                        }
                        else continue;
                    }
                }
            }
        }
        onChanged?.Invoke(data);
    }
    public void Set(Character value)
    {
        Array.Copy(value.data, 0, data, 0, value.data.Length);
        onChanged?.Invoke(data);
    }
    public void Init() => onChanged?.Invoke(data);
}