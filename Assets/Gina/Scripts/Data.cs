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
    private Dictionary<string, object> data;

    public Data(bool inven = false, bool stats = false, bool posrot = false, bool npc = false)
    {
        data = new Dictionary<string, object>();
        data.Add(pname.name, "Entity");
        if(stats)
        {
            data.Add(pname.level, 1);
            data.Add(pname.curHealth, 1f);
            data.Add(pname.maxHealth, 100f);
            data.Add(pname.mulHealth, 0f);
            data.Add(pname.curStamina, 1f);
            data.Add(pname.maxStamina, 100f);
            data.Add(pname.mulStamina, 0f);
            data.Add(pname.curMana, 1f);
            data.Add(pname.maxMana, 100f);
            data.Add(pname.mulMana, 0f);
            data.Add(pname.curExp, 0f);
            data.Add(pname.maxExp, 1000f);
            data.Add(pname.mulExp, 0f);
        }
        if(posrot)
        {
            data.Add(pname.positionx, 0f);
            data.Add(pname.positiony, 0f);
            data.Add(pname.positionz, 0f);
            data.Add(pname.rotationx, 0f); 
            data.Add(pname.rotationy, 0f);
            data.Add(pname.rotationz, 0f);
        }
        if(inven)
        {
            data.Add(pname.inventory, new Dictionary<string, object>[30]);
            data.Add(pname.actionbar, new Dictionary<string, object>[12]);
            data.Add(pname.character, new Dictionary<string, object>[15]);
        }
        if(npc)
        {
            data.Add(pname.loot, string.Empty);
        }
    }


    public void Set<T>(string name, T value)
    {
        if (data.ContainsKey(name))
            data[name] = value;
        else if (!data.ContainsKey(name))
            data.Add(name, value);
    }
    public T Get<T>(string name)
    {
        if (data == null || data.Count <= 0 || !data.ContainsKey(name))
            return default(T);
        try
        {
            return (T)data[name];
        } 
        catch(Exception ex)
        {
            Debug.Log(name);
            Debug.LogError(ex);
            return default(T);
        }
    }
    //values
    internal void Init()
    {
        onNameChaged?.Invoke(Name);
        onLevelChaged?.Invoke(Level);
        onHealthChagned?.Invoke(CurHealth, MaxHealth, MulHeaht);
        onStaminaChagned?.Invoke(CurStamina, MaxStamina, MulStamina);
        onManaChagned?.Invoke(CurMana, MaxMana, MulMana);
        onExperianceChagned?.Invoke(CurExp, MaxExp, MulExp);
        onPositionChanged?.Invoke(Position);
        onRotationChanged?.Invoke(Rotation);

        onLootTableChanged?.Invoke(LootTable);

        onInventoryChanged?.Invoke(Inventory);
        onActionbarChanged?.Invoke(Actionbar);
        onCharacterChanged?.Invoke(Character);
    }

    // Events
    public delegate void OnNameChaged(string value); public event OnNameChaged onNameChaged;
    public delegate void OnLevelChaged(int value); public event OnLevelChaged onLevelChaged;
    public delegate void OnHealthChagned(float cur, float max, float mul); public event OnHealthChagned onHealthChagned;
    public delegate void OnStaminaChagned(float cur, float max, float mul); public event OnStaminaChagned onStaminaChagned;
    public delegate void OnManaChagned(float cur, float max, float mul); public event OnManaChagned onManaChagned;
    public delegate void OnExperianceChagned(float cur, float max, float mul); public event OnExperianceChagned onExperianceChagned;
    public delegate void OnPositionChanged(Vector3 value); public event OnPositionChanged onPositionChanged;
    public delegate void OnRotationChanged(Vector3 value); public event OnRotationChanged onRotationChanged;
    public delegate void OnLootTableChanged(Item vlue); public event OnLootTableChanged onLootTableChanged;
    public delegate void OnInventoryChanged(Dictionary<string, object>[] value); public event OnInventoryChanged onInventoryChanged;
    public delegate void OnActionbarChanged(Dictionary<string, object>[] value); public event OnActionbarChanged onActionbarChanged;
    public delegate void OnCharacterChanged(Dictionary<string, object>[] value); public event OnCharacterChanged onCharacterChanged;


    public Data() { }
    
    public void SetAll(Dictionary<string, object> data)
    {
        this.data = new Dictionary<string, object>(data);
        Init();
    }

    public string Name
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.name))
                return default;
            return Get<string>(pname.name);
        }
        set
        {
            if (data == null) return;
            Set(pname.name, value);
            onNameChaged?.Invoke(value);
        }
    }
    public int Level
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.level))
                return default;
            return Get<int>(pname.level);
        }
        set
        {
            if (data == null) return;
            Set(pname.level, value);
            onLevelChaged?.Invoke(value);
        }
    }

    public float CurHealth
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.curHealth))
                return default;
            return Get<float>(pname.curHealth);
        }
        set
        {
            if (data == null) return;
            Set(pname.curHealth, value);
            onHealthChagned?.Invoke(value, MaxHealth, MulHeaht);
        }
    }
    public float MaxHealth
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.maxHealth))
                return default;
            return Get<float>(pname.maxHealth);
        }
        set
        {
            if (data == null) return;
            Set(pname.maxHealth, value);
            onHealthChagned?.Invoke(CurHealth, value, MulHeaht);
        }
    }
    public float MulHeaht
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.mulHealth))
                return default;
            return Get<float>(pname.mulHealth);
        }
        set
        {
            if (data == null) return;
            Set(pname.mulHealth, value);
            onHealthChagned?.Invoke(CurHealth, MaxHealth, value);
        }
    }

    
    public float CurStamina
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.curStamina))
                return default;
            return Get<float>(pname.curStamina);
        }
        set
        {
            if (data == null) return;
            Set(pname.curStamina, value);
            onStaminaChagned?.Invoke(value, MaxStamina, MulStamina);
        }
    }
    public float MaxStamina
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.maxStamina))
                return default;
            return Get<float>(pname.maxStamina);
        }
        set
        {
            if (data == null) return;
            Set(pname.maxStamina, value);
            onStaminaChagned?.Invoke(CurStamina, value, MulStamina);
        }
    }
    public float MulStamina
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.mulStamina))
                return default;
            return Get<float>(pname.mulStamina);
        }
        set
        {
            if (data == null) return;
            Set(pname.mulStamina, value);
            onStaminaChagned?.Invoke(CurStamina, MaxStamina, value);
        }
    }

    public float CurMana
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.curMana))
                return default;
            return Get<float>(pname.curMana);
        }
        set
        {
            if (data == null) return;
            Set(pname.curMana, value);
            onManaChagned?.Invoke(value, MaxMana, MulMana);
        }
    }
    public float MaxMana
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.maxMana))
                return default;
            return Get<float>(pname.maxMana);
        }
        set
        {
            if (data == null) return;
            Set(pname.maxMana, value);
            onManaChagned?.Invoke(CurMana, value, MulMana);
        }
    }
    public float MulMana
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.mulMana))
                return default;
            return Get<float>(pname.mulMana);
        }
        set
        {
            if (data == null) return;
            Set(pname.mulMana, value);
            onManaChagned?.Invoke(CurMana, MaxMana, value);
        }
    }

    public float CurExp
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.curExp))
                return default;
            return Get<float>(pname.curExp);
        }
        set
        {
            if (data == null) return;
            Set(pname.curExp, value);
            if (CurExp >= MaxExp)
            {
                Set(pname.curExp, 0);
                Level = Level + 1;
                MulExp = MulExp + Level * .25f;
            }
            onExperianceChagned?.Invoke(value, MaxExp, MulExp);
        }
    }
    public float MaxExp
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.maxExp))
                return default;
            return Get<float>(pname.maxExp);
        }
        set
        {
            if (data == null) return;
            Set(pname.maxExp, value);
            onExperianceChagned?.Invoke(CurExp, value, MulExp);
        }
    }
    public float MulExp
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.mulExp))
                return default;
            return Get<float>(pname.mulExp);
        }
        set
        {
            if (data == null) return;
            Set(pname.mulExp, value);
            onExperianceChagned?.Invoke(CurExp, MaxExp, value);
        }
    }

    public Vector3 Position
    {
        get
        {
            if(data == null ||
                !data.ContainsKey(pname.positionx) &&
                !data.ContainsKey(pname.positiony) && 
                !data.ContainsKey(pname.positionz))
                return default;

            var x = Get<float>(pname.positionx);
            var y = Get<float>(pname.positiony);
            var z = Get<float>(pname.positionz);

            return new Vector3(x, y, z);

        }
        set
        {
            if (data == null) return;
            Set(pname.positionx, value.x);
            Set(pname.positiony, value.y);
            Set(pname.positionz, value.z);

            onPositionChanged?.Invoke(value);
        }
    }
    public Vector3 Rotation
    {
        get
        {
            if (data == null ||
                !data.ContainsKey(pname.rotationx) && 
                !data.ContainsKey(pname.rotationy) && 
                !data.ContainsKey(pname.rotationz))
                return default;

            var x = Get<float>(pname.rotationx);
            var y = Get<float>(pname.rotationy);
            var z = Get<float>(pname.rotationz);

            return new Vector3(x, y, z);

        }
        set
        {
            if (data == null) return;
            Set(pname.rotationx, value.x);
            Set(pname.rotationy, value.y);
            Set(pname.rotationz, value.z);

            onRotationChanged?.Invoke(value);
        }
    }

    public Item LootTable
    {
        get
        {

            if (data == null || !data.ContainsKey(pname.loot))
                return null;
            return Database.GetLootTableByID(Get<string>(pname.loot));
        }
        set
        {
            if (data == null) return;
            if (value == null) return;
            Set(pname.loot, value.GetID);
            onLootTableChanged?.Invoke(value);
        }
    }
    public Dictionary<string, object>[] Inventory
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.inventory))
                return default;
            return Get<Dictionary<string, object>[]>(pname.inventory);
        }
        set
        {
            if (data == null) return;
            Set(pname.inventory, value);
            onInventoryChanged?.Invoke(value);
        }
    }
    public Dictionary<string, object>[] Actionbar
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.actionbar))
                return default;
            return Get<Dictionary<string, object>[]>(pname.actionbar);
        }
        set
        {
            if (data == null) return;
            Set(pname.actionbar, value);
            onActionbarChanged?.Invoke(value);
        }
    }
    public Dictionary<string, object>[] Character
    {
        get
        {
            if (data == null || !data.ContainsKey(pname.character))
                return default;
            return Get<Dictionary<string, object>[]>(pname.character);
        }
        set
        {
            if (data == null) return;
            Set(pname.character, value);
            onActionbarChanged?.Invoke(value);
        }
    }

    public void SetInventory(int index = -1, Dictionary<string, object> value = null)
    {
        var inventory = this.Inventory;
        if (index >= 0) inventory[index] = value;
        else
        {
            bool alreadyAdded = false;
            // Check every slot to see if item is already there then add onto it if the item is stakable
            for (int i = 0; i < inventory.Length; i++)
            {
                if (value == null) break;
                var _itemdata = inventory[i];
                if (_itemdata != null)
                {
                    var _item = new Item(_itemdata);
                    if (_item.IsValid && _item.GetID == new Item(value).GetID)
                    {
                        if(_item.IsStackable && _item.Get<int>(pname.curStack) < _item.Get<int>(pname.maxStack))
                        {
                            _item.Set(pname.curStack, _item.Get<int>(pname.curStack) + new Item(value).Get<int>(pname.curStack));
                            inventory[i] = _item.data;
                            alreadyAdded = true;
                            break;
                        }
                    }
                }
            }


            if(!alreadyAdded)
            {
                // Add the item to the first empty slot
                for (int i = 0; i < inventory.Length; i++)
                {
                    if(inventory[i] == null || !new Item(inventory[i]).IsValid)
                    {
                        inventory[i] = value;
                        break;
                    }

                    var slot = new Item(inventory[i]);
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
        this.Inventory = inventory;
    }
    public void SetActionbar(int index = 0, Dictionary<string, object> value = null)
    {
        var actionbar = this.Actionbar;
        actionbar[index] = value;
        this.Actionbar = actionbar;
    }
    public void SetCharacter(int index = 0, Dictionary<string, object> value = null)
    {
        var character = this.Character;
        character[index] = value;
        this.Character = character;
    }

    public void Save(string savename = default)
    {
        var savepath = Path.Combine(Application.streamingAssetsPath, $"{(string.IsNullOrEmpty(savename) ? "player" : savename)}_.json").Replace("\\", "/");
        if (!string.IsNullOrEmpty(savepath) && File.Exists(savepath))
            File.Delete(savepath);

        File.WriteAllText(savepath, JsonConvert.SerializeObject(this.data, Formatting.Indented), Encoding.UTF8);
        Debug.Log("Data has been Saved");
    }
    public void Load(string loadname = default)
    {
        Directory.CreateDirectory(Application.streamingAssetsPath);
        var loadpath = Path.Combine(Application.streamingAssetsPath, $"{(string.IsNullOrEmpty(loadname) ? "player" : loadname)}_.json").Replace("\\", "/");
        if (File.Exists(loadpath))
        {
            var json = File.ReadAllText(loadpath, Encoding.UTF8);
            var d  = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if(d != null)
            {
                SetAll(d);
                Debug.Log("data was loaded!");
            }
        }
    }

}