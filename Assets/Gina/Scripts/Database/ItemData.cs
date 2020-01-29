using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class ItemData : IDisposable
{
    public string icon;
    public string name;
    public string desc;
    public bool isEquipment;
    public EquiptmentType equipmentType;
    public bool isStackable;
    public int curStack = 1, maxStack = 16;
    public float healthBonus;
    public float staminaBonus;
    public float manaBonus;
    public float expBonus;
    public float strengthBonus;
    public float agilityBonus;
    public float dexterityBonus;
    public float worth;
    public string prefab;

    [JsonIgnore] public string file;
    [JsonIgnore] public Texture2D _texture = null;
    [JsonIgnore] public Sprite _sprite = null;
    [JsonIgnore] public Object _object = null;

    public ItemData() { }
    public ItemData(ItemData value = null)
    {
        if (value == null)
            return;
        this.icon = value.icon;
        this.name = value.name;
        this.desc = value.desc;
        this.isEquipment = value.isEquipment;
        this.equipmentType = value.equipmentType;
        this.isStackable = value.isStackable;
        this.curStack = value.curStack;
        this.maxStack = value.maxStack;
        this.healthBonus = value.healthBonus;
        this.staminaBonus = value.staminaBonus;
        this.manaBonus = value.manaBonus;
        this.expBonus = value.expBonus;
        this.strengthBonus = value.strengthBonus;
        this.agilityBonus = value.agilityBonus;
        this.dexterityBonus = value.dexterityBonus;
        this.worth = value.worth;
        this.prefab = value.prefab;
        this.file = value.file;
    }

    public void Set (ItemData value = null)
    {
        if (value == null)
            return;
        this.icon = value.icon;
        this.name = value.name;
        this.desc = value.desc;
        this.isEquipment = value.isEquipment;
        this.equipmentType = value.equipmentType;
        this.isStackable = value.isStackable;
        this.curStack = value.curStack;
        this.maxStack = value.maxStack;
        this.healthBonus = value.healthBonus;
        this.staminaBonus = value.staminaBonus;
        this.manaBonus = value.manaBonus;
        this.expBonus = value.expBonus;
        this.strengthBonus = value.strengthBonus;
        this.agilityBonus = value.agilityBonus;
        this.dexterityBonus = value.dexterityBonus;
        this.worth = value.worth;
        this.prefab = value.prefab;
        this.file = value.file;
    }
    [JsonIgnore] public ItemData Copy => new ItemData(this);
    [JsonIgnore] public string id
    {
        get
        {
            return $"{Application.productName.ToLower()}:{name.Replace(" ", "_").ToLower()}";
        }
    }
    [JsonIgnore] public bool IsValid
    {
        get
        {
            bool valid = true;
            if (string.IsNullOrEmpty(name)) valid = false;
            return valid;
        }
    }
    [JsonIgnore] public Texture2D Texture
    {
        get
        {
            if (_texture != null) return _texture;
            _texture = new Texture2D(1, 1);
            // Load from Resource folder
            var v = icon;
            var r = v.Split('.')[0];
            r = r.Replace($"{Application.productName}/", string.Empty);
            r = r.Replace("Resources/", string.Empty);
            _texture = Resources.Load<Texture2D>(r);
            if (_texture != null)
                return _texture;
            if (File.Exists(v))
            {
                var image_data = File.ReadAllBytes(v);
                _texture = new Texture2D(1, 1);
                _texture.LoadImage(image_data);
                return _texture;
            }

            return default;
        }
    }
    [JsonIgnore] public Sprite Sprite
    {
        get
        {
            if (_sprite != null) return _sprite;
            _texture = new Texture2D(1, 1);
            // Load from Resource folder
            var v = icon;
            var r = v.Split('.')[0];
            r = r.Replace($"{Application.productName}/", string.Empty);
            r = r.Replace("Resources/", string.Empty);
            _texture = Resources.Load<Texture2D>(r);
            if (_texture != null)
            {
                _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one * 0.5f);
                return _sprite;
            }
            if (File.Exists(v))
            {
                var image_data = File.ReadAllBytes(v);
                _texture = new Texture2D(1, 1);
                _texture.LoadImage(image_data);
                _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one * 0.5f);
                return _sprite;
            }

            return default;
        }
    }
    [JsonIgnore] public Object Object
    {
        get
        {
            if (this._object != null) return  this._object;
            // Load from Resource folder
            var v = prefab;
            //v = v.Split('.')[0];
            if (!string.IsNullOrEmpty(v))
            {
#if UNITY_EDITOR
                _object = UnityEditor.AssetDatabase.LoadAssetAtPath(v, typeof(Object));
#else
                    _object = Resources.Load(v);
#endif
                if (_object != null)
                    return _object;
            }

            return default;
        }
    }

    public void Dispose()
    {
        _texture = null;
        _sprite = null;
        _object = null;

        GC.SuppressFinalize(this);
    }
}