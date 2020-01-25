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
    private Dictionary<string, object> data = new Dictionary<string, object>()
    {
        { "name", "entity" },
        { "level", 1 },

        { "curhealth", 1f }, { "maxhealth", 100f }, { "healthmulti", 0f },
        { "curstamina", 1f },{ "maxstamina", 100f }, { "staminamulti", 0f },
        { "curmana", 1f }, { "maxmana", 100f}, { "manamulti", 0f },
        { "curexp", 0f }, { "maxexp", 1000f }, { "expmulti", 0f },
       
        { "positionx", 0 }, { "positiony", 0 }, { "positionz", 0 },
        { "rotationx", 0 }, { "rotationy", 0 }, { "rotationz", 0 },

        { "inventory", new Dictionary<string, object>[30] },
        { "actionbar", new Dictionary<string, object>[12] },
        { "character", new Dictionary<string, object>[15] }
    };
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

        return (T)data[name];
    }
    //values
    internal void Init()
    {
        onInventoryChanged?.Invoke(Get<Dictionary<string, object>[]>("inventory"));
        onActionbarChanged?.Invoke(Get<Dictionary<string, object>[]>("actionbar"));
        onCharacterChanged?.Invoke(Get<Dictionary<string, object>[]>("character"));
    }

    // Events
    public delegate void OnNameChaged(string value); public event OnNameChaged onNameChaged;
    public delegate void OnLevelChaged(int value); public event OnLevelChaged onLevelChaged;
    public delegate void OnHealthChagned(float value); public event OnHealthChagned onHealthChagned;
    public delegate void OnStaminaChagned(float value); public event OnStaminaChagned onStaminaChagned;
    public delegate void OnManaChagned(float value); public event OnManaChagned onManaChagned;
    public delegate void OnExperianceChagned(float value); public event OnExperianceChagned onExperianceChagned;
    public delegate void OnPositionChanged(Vector3 value); public event OnPositionChanged onPositionChanged;
    public delegate void OnRotationChanged(Vector3 value); public event OnRotationChanged onRotationChanged;
    public delegate void OnInventoryChanged(Dictionary<string, object>[] value); public event OnInventoryChanged onInventoryChanged;
    public delegate void OnActionbarChanged(Dictionary<string, object>[] value); public event OnActionbarChanged onActionbarChanged;
    public delegate void OnCharacterChanged(Dictionary<string, object>[] value); public event OnCharacterChanged onCharacterChanged;


    public Data() { }

    public string CurName
    {
        get
        {
            if (!data.ContainsKey("name"))
                return default;
            return Get<string>("name");
        }
        set
        {
            Set("name", value);
            onNameChaged?.Invoke(value);
        }
    }
    public int CurLevel
    {
        get
        {
            if (!data.ContainsKey("level"))
                return default;
            return Get<int>("level");
        }
        set
        {
            Set("level", value);
            onLevelChaged?.Invoke(value);
        }
    }
    public float CurHealth
    {
        get
        {
            if (!data.ContainsKey("curhealth"))
                return default;
            return Get<float>("curhealth");
        }
        set
        {
            Set("curhealth", value);
            onHealthChagned?.Invoke(value);
        }
    }
    public float GetCurStamina
    {
        get
        {
            if (!data.ContainsKey("curstamina"))
                return default;
            return Get<float>("curstamina");
        }
        set
        {
            Set("curstamina", value);
            onStaminaChagned?.Invoke(value);
        }
    }
    public float CurMana
    {
        get
        {
            if (!data.ContainsKey("curmana"))
                return default;
            return Get<float>("curmana");
        }
        set
        {
            Set("curmana", value);
            onManaChagned?.Invoke(value);
        }
    }
    public float CurExp
    {
        get
        {
            if (!data.ContainsKey("curexp"))
                return default;
            return Get<float>("curexp");
        }
        set
        {
            Set("curexep", value);
            if (Get<float>("curexp") >= Get<float>("maxexp"))
            {
                Set("curexp", 0);
                CurLevel = CurLevel + 1;
                Set("expmulit", Get<float>("expmulti") + Get<int>("level") * .25f);
            }
            onExperianceChagned?.Invoke(value);
        }
    }
    public Vector3 CurPosition
    {
        get
        {
            if(!data.ContainsKey("positionx") && !data.ContainsKey("positiony") && !data.ContainsKey("positionz"))
                return default;

            var x = Get<float>("positionx");
            var y = Get<float>("positiony");
            var z = Get<float>("positionz");

            return new Vector3(x, y, z);

        }
        set
        {
            Set("positionx", value.x);
            Set("positiony", value.y);
            Set("positionz", value.z);

            onPositionChanged?.Invoke(value);
        }
    }
    public Vector3 CurRotation
    {
        get
        {
            if (!data.ContainsKey("rotationx") && !data.ContainsKey("rotationy") && !data.ContainsKey("rotationz"))
                return default;

            var x = Get<float>("rotationx");
            var y = Get<float>("rotationy");
            var z = Get<float>("rotationz");

            return new Vector3(x, y, z);

        }
        set
        {
            Set("rotationx", value.x);
            Set("rotationy", value.y);
            Set("rotationz", value.z);

            onRotationChanged?.Invoke(value);
        }
    }

    public Dictionary<string, object>[] inventory
    {
        get
        {
            if (!data.ContainsKey("inventory"))
                return default;
            return Get<Dictionary<string, object>[]>("inventory");
        }
        set
        {
            Set("inventory", value);
            onInventoryChanged?.Invoke(value);
        }
    }
    public Dictionary<string, object>[] actionbar
    {
        get
        {
            if (!data.ContainsKey("actionbar"))
                return default;
            return Get<Dictionary<string, object>[]>("actionbar");
        }
        set
        {
            Set("actionbar", value);
            onActionbarChanged?.Invoke(value);
        }
    }
    public Dictionary<string, object>[] character
    {
        get
        {
            if (!data.ContainsKey("character"))
                return default;
            return Get<Dictionary<string, object>[]>("character");
        }
        set
        {
            Set("character", value);
            onActionbarChanged?.Invoke(value);
        }
    }

    public void SetInventory(int index = -1, Dictionary<string, object> value = null)
    {
        var inventory = this.inventory;
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
                        if(_item.IsStackable && _item.Get<int>(paramname.curStack) < _item.Get<int>(paramname.maxStack))
                        {
                            _item.Set(paramname.curStack, _item.Get<int>(paramname.curStack) + new Item(value).Get<int>(paramname.curStack));
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
                            if (slot.Get<int>(paramname.curStack) < slot.Get<int>(paramname.maxStack))
                            {
                                slot.Set(paramname.curStack, slot.Get<int>(paramname.curStack) + 1);
                                break;
                            }
                        }
                        else continue;
                    }
                }
            }





        }
        this.inventory = inventory;
    }
    public void SetActionbar(int index = 0, Dictionary<string, object> value = null)
    {
        var actionbar = this.actionbar;
        actionbar[index] = value;
        this.actionbar = actionbar;
    }
    public void SetCharacter(int index = 0, Dictionary<string, object> value = null)
    {
        var character = this.character;
        character[index] = value;
        this.character = character;
    }

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
        Data data = null;
        Directory.CreateDirectory(Application.streamingAssetsPath);
        var loadpath = Path.Combine(Application.streamingAssetsPath, $"{(string.IsNullOrEmpty(loadname) ? "player" : loadname)}_.json").Replace("\\", "/");
        if (File.Exists(loadpath))
        {
            var json = File.ReadAllText(loadpath, Encoding.UTF8);
            data = JsonConvert.DeserializeObject<Data>(json);
        }
        if(data != null)
        {
            this.CurName = data.CurName;
            this.CurLevel = data.CurLevel;
            this.CurHealth = data.CurHealth;
            this.GetCurStamina = data.GetCurStamina;
            this.CurMana = data.CurMana;
            this.CurExp = data.CurExp;
            this.CurPosition = data.CurPosition;
            this.CurRotation = data.CurRotation;
            this.inventory = data.inventory;
            this.actionbar = data.actionbar;
            this.character = data.character;
            Debug.Log("data was loaded!");
        }
    }

}