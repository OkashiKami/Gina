using CodeMonkey.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData data;

    private List<Action> commands = new List<Action>();
    private Dictionary<Options, object> DefaultStats()
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

        stats.Set(Options.position, transform.position);
        stats.Set(Options.rotation, transform.eulerAngles);

        return stats.data;
    }

    // Start is called before the first frame update
    void Awake()
    {
        onStatsChanged += (a) => 
        {
            var pos = a.Get<Vector3>(Options.position);
            var rot = a.Get<Vector3>(Options.rotation);

            commands.Add(() =>
            {
                transform.position = pos;
                transform.eulerAngles = rot;
            });
        };
    }

    private void Start()
    {
        Load();
        FunctionPeriodic.Create(() => Save(), 30);
    }
    
    private void OnApplicationQuit()
    {
        Save(join: true);   
    }
    private void Update()
    {
        if(commands.Count > 0)
        {
            commands[0]?.Invoke();
            commands.RemoveAt(0);
        }
    }


    public void SetStat(Options option, object value = null)
    {
        var stats = new Item(data.stats);
        stats.Set(option, value);
        onStatsChanged?.Invoke(stats);
    }
    private delegate void OnFirstLoadComplete(Item data);
    private event OnFirstLoadComplete onStatsChanged;

    public void SetInventoryItem(int slot = -1, Item value = null)
    {
        if (slot < 0 || slot > data.inventory.Length - 1) return; 
        data.inventory[slot] = value != null ? value.data : null;
        onInventoryItemChaged?.Invoke(this.data.inventory.Select(x => new Item(x)).ToList());
    }
    public delegate void OnInventoryItemChaged(List<Item> items);
    public event OnInventoryItemChaged onInventoryItemChaged;

    public void SetActionbarItem(int slot = -1, Item value = null)
    {
        if (slot < 0 || slot > data.actionbar.Length - 1) return;
        data.actionbar[slot] = value != null ? value.data : null;
        onActionbarItemChaged?.Invoke(this.data.actionbar.Select(x => new Item(x)).ToList());
    }
    public delegate void OnActionbarItemChaged(List<Item> items);
    public event OnActionbarItemChaged onActionbarItemChaged;

    public void SetCharacterItem(int slot = -1, Item value = null)
    {
        if (slot < 0 || slot > data.character.Length - 1) return;
        data.character[slot] = value != null ? value.data : null;
        onCharacterItemChaged?.Invoke(this.data.character.Select(x => new Item(x)).ToList());
    }
    public delegate void OnCharacterItemChaged(List<Item> items);
    public event OnCharacterItemChaged onCharacterItemChaged;

    public void Save(bool join = false) 
    {
        var saveThread = new Thread(new ThreadStart(() =>
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            var savepath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(savepath, json, Encoding.UTF8);
            Debug.Log("Saved");
        }));
        saveThread.Name = "Save Thread";
        saveThread.IsBackground = true;
        saveThread.Start();
        if (join) saveThread.Join();
    }
    public void Load()
    {
        var loadThread = new Thread(new ThreadStart(() =>
        {
            PlayerData data = null;
            Directory.CreateDirectory(Application.streamingAssetsPath);
            var loadpath = Path.Combine(Application.streamingAssetsPath, "player_.json").Replace("\\", "/");
            if (File.Exists(loadpath))
            {
                var json = File.ReadAllText(loadpath, Encoding.UTF8);
                data = JsonConvert.DeserializeObject<PlayerData>(json);
            }

            if (data == null)
            {
                this.data.stats = DefaultStats();
                this.data.inventory = new Dictionary<Options, object>[30];
                this.data.actionbar = new Dictionary<Options, object>[12];
                this.data.character = new Dictionary<Options, object>[15];
            }
            else
            {
                this.data.stats = new Dictionary<Options, object>(data.stats);
                Array.Copy(data.inventory, this.data.inventory, 30);
                Array.Copy(data.actionbar, this.data.actionbar, 12);
                Array.Copy(data.character, this.data.character, 15);
            }

            commands.Add(() =>
            {
                onInventoryItemChaged?.Invoke(this.data.inventory.Select(x => new Item(x)).ToList());
                onActionbarItemChaged?.Invoke(this.data.actionbar.Select(x => new Item(x)).ToList());
                onCharacterItemChaged?.Invoke(this.data.character.Select(x => new Item(x)).ToList());
                onStatsChanged?.Invoke(new Item(this.data.stats));
            });
        }));
        loadThread.Name = "Load Thread";
        loadThread.IsBackground = true;
        loadThread.Start();
    }

}
