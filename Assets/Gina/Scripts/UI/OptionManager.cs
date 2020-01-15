using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionManager
{
    public static T Get<T>(Options option, Dictionary<Options, object> data)
    {
        if (typeof(T).Equals(typeof(string)))
            return data.ContainsKey(option) ? (T)data[option] : default(T);
        if (typeof(T).Equals(typeof(bool)))
            return data.ContainsKey(option) ? (T)data[option] : default(T);
        if (typeof(T).Equals(typeof(int)))
            return data.ContainsKey(option) ? (T)data[option] : default(T);
        if (typeof(T).Equals(typeof(float)))
            return data.ContainsKey(option) ? (T)data[option] : default(T);
        if (typeof(T).Equals(typeof(Vector2)))
        {
            if (!data.ContainsKey(option)) return default(T);
            string result = data[option].ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
            var parts = result.Split(',').ToList().Select(x => float.Parse(x)).ToList();
            return (T)(object)new Vector2(parts[0], parts[1]);
        }
        if (typeof(T).Equals(typeof(Vector3)))
        {
            if (!data.ContainsKey(option)) return default(T);
            string result = data[option].ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
            var parts = result.Split(',').ToList().Select(x => float.Parse(x)).ToList();
            return (T)(object)new Vector3(parts[0], parts[1], parts[2]);
        }
        if (typeof(T).Equals(typeof(Quaternion)))
        {
            if (!data.ContainsKey(option)) return default(T);
            string result = data[option].ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
            var parts = result.Split(',').ToList().Select(x => float.Parse(x)).ToList();
            return (T)(object)new Quaternion(parts[0], parts[1], parts[2], parts[2]);
        }

        return default(T);
    }
    public static void Set<T>(Options option, Dictionary<Options, object> data, T value = default)
    {
        if (typeof(T).Equals(typeof(string)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        if (typeof(T).Equals(typeof(bool)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        if (typeof(T).Equals(typeof(int)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        if (typeof(T).Equals(typeof(float)))
        {
            if (data.ContainsKey(option))
                data[option] = value;
            else
                data.Add(option, value);
        }
        if (typeof(T).Equals(typeof(Vector2)))
        {
            {
                if (data.ContainsKey(option))
                    data[option] = value.ToString();
                else
                    data.Add(option, value.ToString());
            }
        }
        if (typeof(T).Equals(typeof(Vector3)))
        {
            if (data.ContainsKey(option))
                data[option] = value.ToString();
            else
                data.Add(option, value.ToString());
        }
        if (typeof(T).Equals(typeof(Quaternion)))
        {
            if (data.ContainsKey(option))
                data[option] = value.ToString();
            else
                data.Add(option, value.ToString());
        }
    }
}
public enum Options
    {
        uuid,
        name,
        level,
        curHealth,
        maxHealth,
        curStamina,
        maxStamina,
        curMana,
        maxMana,
        curExp,
        maxExp,
        desc,
        icon,
        stackable,
        curStack,
        maxStack,
        position,
        rotation,
    }