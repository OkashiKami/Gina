using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public Dictionary<Options, object> stats = new Dictionary<Options, object>();
    public Dictionary<Options, object>[] inventory = new Dictionary<Options, object>[30];
    public Dictionary<Options, object>[] actionbar = new Dictionary<Options, object>[12];
    public Dictionary<Options, object>[] character = new Dictionary<Options, object>[15];
}