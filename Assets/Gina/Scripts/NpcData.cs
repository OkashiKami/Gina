
using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class NpcData : IDisposable
{
    public string name;
    public int level = 1;
    public float[] health = new float[] { 100, 100f };
    public float[] stamina = new float[] { 100f, 100f };
    public float[] mana = new float[] { 100f, 100f };
    public float[] exp = new float[] { 0f, 1000f };
    public float[] position = new float[3] { 0f, 0f, 0f };
    public float[] rotation = new float[3] { 0f, 0f, 0f };

    public ItemData[] inventory = new ItemData[30];
    public ItemData[] character = new ItemData[15];

    public LootData lootTable;

    [JsonIgnore] internal string file;
    [JsonIgnore] internal string id => $"{Application.productName}:{name}".Replace(" ", "_").ToLower();
    [JsonIgnore] public NpcData Copy => new NpcData(this);

    [JsonIgnore] public string Name { get => this.name; set { this.name = value; onNameChanged?.Invoke(value); } }
    [JsonIgnore] public int Level { get => this.level; set { this.level = value; onLevelChanged?.Invoke(value); } }
    [JsonIgnore] public float[] Health { get => this.health; set { this.health = value; onHealthChanged?.Invoke(value); } }
    [JsonIgnore] public float[] Stamina { get => this.stamina; set { this.stamina = value; onHealthChanged?.Invoke(value); } }
    [JsonIgnore] public float[] Mana { get => this.mana; set { this.mana = value; onManaChanged?.Invoke(value); } }
    [JsonIgnore] public float[] Exp { get => this.exp; set { this.exp = value; onExpChanged?.Invoke(value); } }
    [JsonIgnore] public Vector3 Position { get => new Vector3(position[0], position[1], position[2]); set { this.position = new float[] { value.x, value.y, value.z }; onPositionChanged?.Invoke(value); } }
    [JsonIgnore] public Vector3 Rotation { get => new Vector3(rotation[0], rotation[1], rotation[2]); set { this.rotation = new float[] { value.x, value.y, value.z }; onRotationChanged?.Invoke(value); } }



    public delegate void OnNameChanged(string name); public event OnNameChanged onNameChanged;
    public delegate void OnLevelChanged(int level); public event OnLevelChanged onLevelChanged;
    public delegate void OnHealthChanged(float[] health); public event OnHealthChanged onHealthChanged;
    public delegate void OnStaminaChanged(float[] stamina); public event OnStaminaChanged onStaminaChanged;
    public delegate void OnManaChanged(float[] mana); public event OnManaChanged onManaChanged;
    public delegate void OnExpChanged(float[] expe); public event OnExpChanged onExpChanged;
    public delegate void OnPositionChanged(Vector3 position); public event OnPositionChanged onPositionChanged;
    public delegate void OnRotationChanged(Vector3 rotation); public event OnPositionChanged onRotationChanged;
    public delegate void OnInventoryChanged(ItemData[] inventory); public event OnInventoryChanged onInventoryChanged;
    public delegate void OnCharacterChanged(ItemData[] character); public event OnCharacterChanged onCharacterChanged;


    public NpcData() { }
    public NpcData(NpcData value = null)
    {
        if (value == null)
            return;
        this.name = value.name;
        this.level = value.level;
        this.health = value.health;
        this.stamina = value.stamina;
        this.mana = value.mana;
        this.exp = value.exp;

        Array.Copy(value.position, this.position, 3);
        Array.Copy(value.rotation, this.rotation, 3);

        Array.Copy(value.inventory, this.inventory, 30);
        Array.Copy(value.character, this.character, 15);

        this.file = value.file;
    }
    public void Set(NpcData value = null)
    {
        if (value == null)
            return;
        this.Name = value.Name;
        this.Level = value.Level;
        this.Health = value.Health;
        this.Stamina = value.Stamina;
        this.Mana = value.Mana;
        this.Exp = value.Exp;
        this.Position = value.Position;
        this.Rotation = value.Rotation;

        Array.Copy(value.inventory, this.inventory, 30);
        Array.Copy(value.character, this.character, 15);

        onInventoryChanged?.Invoke(this.inventory);
        onCharacterChanged?.Invoke(this.character);

        this.file = value.file;
    }

    public void ModifyInvintory(int index = -1, ItemData value = null)
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
                    if (_itemdata.IsValid && _itemdata.id == value.id)
                    {
                        if (_itemdata.isStackable && _itemdata.curStack < _itemdata.maxStack)
                        {
                            _itemdata.curStack = _itemdata.curStack + value.curStack;
                            inventory[i] = _itemdata;
                            alreadyAdded = true;
                            break;
                        }
                    }
                }
            }


            if (!alreadyAdded)
            {
                // Add the item to the first empty slot
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] == null || !inventory[i].IsValid)
                    {
                        inventory[i] = value;
                        break;
                    }

                    var slot = inventory[i];
                    var item = value;
                    if (item.id == slot.id)
                    {
                        if (slot.isStackable)
                        {
                            if (slot.curStack < slot.maxStack)
                            {
                                slot.curStack = slot.curStack + 1;
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
    public void ModifyCharacter(int index = -1, ItemData value = null)
    {
        if (index >= 0) character[index] = value;
        else
        {
            bool alreadyAdded = false;
            // Check every slot to see if item is already there then add onto it if the item is stakable
            for (int i = 0; i < character.Length; i++)
            {
                if (value == null) break;
                var _itemdata = character[i];
                if (_itemdata != null)
                {
                    if (_itemdata.IsValid && _itemdata.id == value.id)
                    {
                        if (_itemdata.isStackable && _itemdata.curStack < _itemdata.maxStack)
                        {
                            _itemdata.curStack = _itemdata.curStack + value.curStack;
                            character[i] = _itemdata;
                            alreadyAdded = true;
                            break;
                        }
                    }
                }
            }


            if (!alreadyAdded)
            {
                // Add the item to the first empty slot
                for (int i = 0; i < character.Length; i++)
                {
                    if (character[i] == null || !character[i].IsValid)
                    {
                        character[i] = value;
                        break;
                    }

                    var slot = character[i];
                    var item = value;
                    if (item.id == slot.id)
                    {
                        if (slot.isStackable)
                        {
                            if (slot.curStack < slot.maxStack)
                            {
                                slot.curStack = slot.curStack + 1;
                                break;
                            }
                        }
                        else continue;
                    }
                }
            }
        }
        onCharacterChanged?.Invoke(character);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}