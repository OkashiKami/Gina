using CodeMonkey.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData data;

    private List<Action> commands = new List<Action>();
    private Dictionary<Options, object> DefaultStats()
    {
        Dictionary<Options, object> stats = new Dictionary<Options, object>();
        OptionManager.Set(Options.name, stats, "Player");
        OptionManager.Set(Options.level, stats, 1);

        OptionManager.Set(Options.curHealth, stats, 1f);
        OptionManager.Set(Options.curStamina, stats, 1f);
        OptionManager.Set(Options.curMana, stats, 1f);
        OptionManager.Set(Options.curExp, stats, 0f);

        OptionManager.Set(Options.maxHealth, stats, 100f);
        OptionManager.Set(Options.maxStamina, stats, 100f);
        OptionManager.Set(Options.maxMana, stats, 100f);
        OptionManager.Set(Options.maxExp, stats, 1000f);

        OptionManager.Set(Options.position, stats, transform.position);
        OptionManager.Set(Options.rotation, stats, transform.eulerAngles);

        return stats;
    }

    // Start is called before the first frame update
    void Awake()
    {
        onStatsChanged += (a) => 
        {
            var pos = OptionManager.Get<Vector3>(Options.position, a);
            var rot = OptionManager.Get<Vector3>(Options.rotation, a);

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
        OptionManager.Set(option, data.stats, value);
        onStatsChanged?.Invoke(data.stats);
    }
    private delegate void OnFirstLoadComplete(Dictionary<Options, object> data);
    private event OnFirstLoadComplete onStatsChanged;

    public void SetInventoryItem(int slot = -1, Dictionary<Options, object> data = null)
    {

        onInventoryItemChaged?.Invoke(this.data.inventory);
    }
    public delegate void OnInventoryItemChaged(Dictionary<Options, object>[] items);
    public event OnInventoryItemChaged onInventoryItemChaged;

    public void SetActionbarItem(int slot = -1, Dictionary<Options, object> data = null)
    {

        onActionbarItemChaged?.Invoke(this.data.actionbar);
    }
    public delegate void OnActionbarItemChaged(Dictionary<Options, object>[] items);
    public event OnActionbarItemChaged onActionbarItemChaged;

    public void SetCharacterItem(int slot = -1, Dictionary<Options, object> data = null)
    {

        onCharacterItemChaged?.Invoke(this.data.character);
    }
    public delegate void OnCharacterItemChaged(Dictionary<Options, object>[] items);
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
                onInventoryItemChaged?.Invoke(this.data.inventory);
                onActionbarItemChaged?.Invoke(this.data.actionbar);
                onCharacterItemChaged?.Invoke(this.data.character);
                onStatsChanged?.Invoke(this.data.stats);
            });
        }));
        loadThread.Name = "Load Thread";
        loadThread.IsBackground = true;
        loadThread.Start();
    }

}
