using System;

[Serializable]
public class LootTableEntry
{
    public string item = "entry";
    public int dropchance = 0;

    public LootTableEntry()
    {
        item = string.Empty;
        dropchance = 0;
    }
}