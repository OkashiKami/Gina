using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Item
{
    public Dictionary<string, object> data = new Dictionary<string, object>();

    [NonSerialized] public string file;
    [NonSerialized] public Texture2D _texture = null;
    [NonSerialized] public Sprite _sprite = null;
    [NonSerialized] public Object _object = null;


    public Item() { }
    public Item(Dictionary<string, object> item_data, string file = default)
    {
        this.data = item_data;
        if (!string.IsNullOrEmpty(file)) this.file = file;
    }
    public Item Copy => new Item(data, file);
    internal string GetID
    {
        get
        {
            var _namespace = Application.productName.ToLower();
            var _name = Get<string>(paramname.name).Replace(" ", "_").ToLower();
            return $"{_namespace}:{_name}";
        }
    }
    public bool IsValid
    {
        get
        {
            bool valid = true;
            if (data == null) return false;
            try
            {
                if (!data.ContainsKey(paramname.name) || string.IsNullOrEmpty(Get<string>(paramname.name))) valid = false;
            }
            catch
            {
                return false;
            }
            return valid;
        }
    }
    public bool IsStackable
    {
        get
        {
            if (data == null || data.Count <= 0) 
                return false;

            var curstack = Has(paramname.curStack);
            var maxstack = Has(paramname.maxStack);

            return curstack && maxstack;
        }
    }

    internal T Get<T>(string option)
    {
#if UNITY_EDITOR
        // Skip this part
#else
        if (!IsValid)
        return default;
#endif
        if (data == null || !data.ContainsKey(option))
            return default(T);

        if (typeof(T).Equals(typeof(string)))
        {
            if (data.ContainsKey(option))
                return (T)data[option];
        }
        else if (typeof(T).Equals(typeof(bool)))
        {
            if (data.ContainsKey(option))
                return (T)(object)bool.Parse(data[option].ToString());
        }
        else if (typeof(T).Equals(typeof(int)))
        {
            if (data.ContainsKey(option))
                return (T)(object)int.Parse(data[option].ToString());
        }
        else if (typeof(T).Equals(typeof(float)))
        {
            if (data.ContainsKey(option))
                return (T)(object)float.Parse(data[option].ToString());
        }
        else if (typeof(T).Equals(typeof(Vector2)))
        {
            if (data.ContainsKey(option))
            {
                string result = data[option].ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
                var parts = result.Split(',').ToList().Select(x => float.Parse(x)).ToList();
                return (T)(object)new Vector2(parts[0], parts[1]);
            }
        }
        else if (typeof(T).Equals(typeof(Vector3)))
        {
            if (data.ContainsKey(option))
            {
                string result = data[option].ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
                var parts = result.Split(',').ToList().Select(x => float.Parse(x)).ToList();
                return (T)(object)new Vector3(parts[0], parts[1], parts[2]);
            }
        }
        else if (typeof(T).Equals(typeof(Quaternion)))
        {
            if (data.ContainsKey(option))
            {
                string result = data[option].ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
                var parts = result.Split(',').ToList().Select(x => float.Parse(x)).ToList();
                return (T)(object)new Quaternion(parts[0], parts[1], parts[2], parts[2]);
            }
        }
        else if (typeof(T).Equals(typeof(Texture2D)))
        {
            if (this._texture != null) return (T)(object)this._texture;
            _texture = new Texture2D(1, 1);
            // Load from Resource folder
            if (data.ContainsKey(option))
            {
                var v = Get<string>(option);
                var r = v.Split('.')[0];
                r = r.Replace($"{Application.productName}/", string.Empty);
                r = r.Replace("Resources/", string.Empty);
                _texture = Resources.Load<Texture2D>(r);
                if (_texture != null)
                    return (T)(object)_texture;
                if (File.Exists(v))
                {
                    var image_data = File.ReadAllBytes(v);
                    _texture = new Texture2D(1, 1);
                    _texture.LoadImage(image_data);
                    return (T)(object)_texture;
                }
            }
        }
        else if (typeof(T).Equals(typeof(Sprite)))
        {
            if (_sprite != null) return (T)(object)_sprite;
            _texture = new Texture2D(1, 1);
            // Load from Resource folder
            if (data.ContainsKey(option))
            {
                var v = Get<string>(option);
                var r = v.Split('.')[0];
                r = r.Replace($"{Application.productName}/", string.Empty);
                r = r.Replace("Resources/", string.Empty);
                _texture = Resources.Load<Texture2D>(r);
                if (_texture != null)
                {
                    _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one * 0.5f);
                    return (T)(object)_sprite;
                }
                if (File.Exists(v))
                {
                    var image_data = File.ReadAllBytes(v);
                    _texture = new Texture2D(1, 1);
                    _texture.LoadImage(image_data);
                    _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one * 0.5f);
                    return (T)(object)_sprite;
                }
            }
        }
        else if (typeof(T).Equals(typeof(Object)))
        {
            if (this._object != null) return (T)(object)this._object;
            // Load from Resource folder
            if (data.ContainsKey(option))
            {
                var v = Get<string>(option);
                //v = v.Split('.')[0];
                if (!string.IsNullOrEmpty(v))
                {
#if UNITY_EDITOR
                    _object = UnityEditor.AssetDatabase.LoadAssetAtPath(v, typeof(Object));
#else
                    _object = Resources.Load(v);
#endif
                    if (_object != null)
                        return (T)(object)_object;
                }
            }
        }
        else
        {
            return (T)data[option];
        }

        return default(T);
    }
    public void Set<T>(string option, T value = default)
    {
        if (typeof(T).Equals(typeof(string)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        else if (typeof(T).Equals(typeof(bool)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        else if (typeof(T).Equals(typeof(int)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        else if (typeof(T).Equals(typeof(float)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        else if (typeof(T).Equals(typeof(Vector2)))
        {
            {
                if (data.ContainsKey(option))
                    data[option] = value.ToString();
                else
                    data.Add(option, value.ToString());
            }
        }
        else if (typeof(T).Equals(typeof(Vector3)))
        {
            if (data.ContainsKey(option))
                data[option] = value.ToString();
            else
                data.Add(option, value.ToString());
        }
        else if (typeof(T).Equals(typeof(Quaternion)))
        {
            if (data.ContainsKey(option))
                data[option] = value.ToString();
            else
                data.Add(option, value.ToString());
        }
        else if (typeof(T).Equals(typeof(Object)))
        {
            var assetPath = string.Empty;
            if (value != null)
            {
                assetPath = UnityEditor.AssetDatabase.GetAssetPath((Object)(object)value);
                Set(option, assetPath);
            }
        }
        else
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }

    }
    public void Remove(string option)
    {
        if (data.ContainsKey(option))
            data.Remove(option);
    }
    public bool Has(string option)
    {
        if (data == null) return false;
        else return data.ContainsKey(option);
    }
}