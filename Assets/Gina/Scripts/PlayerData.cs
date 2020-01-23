using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class PlayerData
{
    //values
    public string name = "Player";
    public int level = 1;
    public SerializableCurMax health = new SerializableCurMax(1, 100);
    public SerializableCurMax stamina = new SerializableCurMax(1, 100);
    public SerializableCurMax mana = new SerializableCurMax(0, 100);
    public SerializableCurMax experiance = new SerializableCurMax(0, 1000);
    public SerializableVector position = Vector3.zero;
    public SerializableVector rotation = Vector3.zero;

    internal void Init()
    {
        onInventoryChanged?.Invoke(inventory);
        onActionbarChanged?.Invoke(actionbar);
        onCharacterChanged?.Invoke(character);
    }

    public Dictionary<paramname, object>[] inventory = new Dictionary<paramname, object>[30];
    public Dictionary<paramname, object>[] actionbar = new Dictionary<paramname, object>[12];
    public Dictionary<paramname, object>[] character = new Dictionary<paramname, object>[15];
    // Events
    public delegate void OnNameChaged(string value); public event OnNameChaged onNameChaged;
    public delegate void OnLevelChaged(int value); public event OnLevelChaged onLevelChaged;
    public delegate void OnHealthChagned(SerializableCurMax value); public event OnHealthChagned onHealthChagned;
    public delegate void OnStaminaChagned(SerializableCurMax value); public event OnStaminaChagned onStaminaChagned;
    public delegate void OnManaChagned(SerializableCurMax value); public event OnManaChagned onManaChagned;
    public delegate void OnExperianceChagned(SerializableCurMax value); public event OnExperianceChagned onExperianceChagned;
    public delegate void OnPositionChanged(Vector3 value); public event OnPositionChanged onPositionChanged;
    public delegate void OnRotationChanged(Vector3 value); public event OnRotationChanged onRotationChanged;
    public delegate void OnInventoryChanged(Dictionary<paramname, object>[] value); public event OnInventoryChanged onInventoryChanged;
    public delegate void OnActionbarChanged(Dictionary<paramname, object>[] value); public event OnActionbarChanged onActionbarChanged;
    public delegate void OnCharacterChanged(Dictionary<paramname, object>[] value); public event OnCharacterChanged onCharacterChanged;


    public PlayerData() { }

    public void SetName(string value)
    {
        name = value;
        onNameChaged?.Invoke(name);
    }
    public void SetLevel(int value)
    {
        level = value;
        onLevelChaged?.Invoke(level);
    }
    public void SetHealth(float value)
    {
        health.cur = value;
        onHealthChagned?.Invoke(health);
    }
    public void SetStamina(float value)
    {
        stamina.cur = value;
        onStaminaChagned?.Invoke(stamina);
    }
    public void SetMana(float value)
    {
        mana.cur = value;
        onManaChagned?.Invoke(mana);
    }
    public void SetExperiance(float value)
    {
        experiance.cur = value;
        if (experiance.cur >= experiance.max)
        {
            experiance.cur = 0;
            experiance.multiplier += level * .25f;
            SetLevel(level++);
        }
        onExperianceChagned?.Invoke(experiance);

    }
    public void SetPotion(Vector3 value)
    {
        position = value;
        onPositionChanged?.Invoke(position);
    }
    public void SetRotation(Vector3 value)
    {
        rotation = value;
        onRotationChanged?.Invoke(rotation);
    }
    public void SetInventory(int index = -1, Dictionary<paramname, object> value = null)
    {

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
        onInventoryChanged?.Invoke(inventory);
    }
    public void SetActionbar(int index = 0, Dictionary<paramname, object> value = null)
    {
        actionbar[index] = value;
        onActionbarChanged?.Invoke(actionbar);
    }
    public void SetCharacter(int index = 0, Dictionary<paramname, object> value = null)
    {
        character[index] = value;
        onCharacterChanged?.Invoke(character);
    }

    public void Save()
    {
        var savepath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
        if (!string.IsNullOrEmpty(savepath) && File.Exists(savepath))
            File.Delete(savepath);

        File.WriteAllText(savepath, JsonConvert.SerializeObject(this, Formatting.Indented), Encoding.UTF8);
        Debug.Log("Player Data has been Saved");
    }
    public void Load()
    {
        PlayerData data = null;
        Directory.CreateDirectory(Application.streamingAssetsPath);
        var loadpath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
        if (File.Exists(loadpath))
        {
            var json = File.ReadAllText(loadpath, Encoding.UTF8);
            data = JsonConvert.DeserializeObject<PlayerData>(json);
        }
        if(data != null)
        {
            this.name = data.name;
            this.level = data.level;
            this.health = new SerializableCurMax(data.health);
            this.stamina = new SerializableCurMax(data.stamina);
            this.mana = new SerializableCurMax(data.mana);
            this.experiance = new SerializableCurMax(data.experiance);
            this.position = new SerializableVector(data.position);
            this.rotation = new SerializableVector(data.rotation);
            Array.Copy(data.inventory, this.inventory, data.inventory.Length);
            Array.Copy(data.actionbar, this.actionbar, data.actionbar.Length);
            Array.Copy(data.character, this.character, data.character.Length);

            onNameChaged?.Invoke(name);
            onLevelChaged?.Invoke(level);
            onHealthChagned?.Invoke(health);
            onStaminaChagned?.Invoke(stamina);
            onManaChagned?.Invoke(mana);
            onExperianceChagned?.Invoke(experiance);
            onPositionChanged?.Invoke(position);
            onRotationChanged?.Invoke(rotation);
            onInventoryChanged?.Invoke(inventory);
            onActionbarChanged?.Invoke(actionbar);
            onCharacterChanged?.Invoke(character);
            Debug.Log("Player data was loaded!");
        }
    }

}