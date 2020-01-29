using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

internal class World
{
    /// <summary>
    /// Create a item in the world that can be picked up.
    /// </summary>
    /// <param name="value">The item that can be droped</param>
    /// <param name="position">The position where to drop the item</param>
    /// <param name="amount">The amout of the item to be dropped</param>
    /// <param name="despawn">How long it will take to despan the item</param>
    /// <param name="atPoint">Indicate to spane item at the position or a radious aroun the position</param>
    internal static void Create<T>(T value, Vector3 position = default, int amount = 1, float despawn = 300, bool atPoint = false)
    {
        if (typeof(T).Equals(typeof(ItemData)))
        {
            var wip = Resources.Load("Prefabs/World_Item") as GameObject;
            var item = (ItemData)(object)value;
            for (int i = 0; i < amount; i++)
            {
                var x = position.x + (atPoint ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));
                var z = position.z + (atPoint ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));

                var wi = Object.Instantiate(wip, new Vector3(x, position.y, z), Quaternion.identity).GetComponent<WorldItem>();
                wi.name = $"[WORLD ITEM]: {item.name}";
                wi.item = item;
                wi.despan = despawn;
                wi.transform.SetAsLastSibling();
            }
        }
        else if (typeof(T).Equals(typeof(LootData)))
        {
            var wip = Resources.Load("Prefabs/World_Item") as GameObject;
            var loot = (LootData)(object)value;
            Dictionary<int, ItemData> tabledata = new Dictionary<int, ItemData>();
            var curentry = loot.items;
            for (int i = 0; i < curentry.Count; i++)
            {
                var entry = curentry[i];
                tabledata.Add(entry.dropPercentage, Database.Load<ItemData>(entry.item_indicator));
            }

            foreach (var key in tabledata.Keys)
            {
                var roll = UnityEngine.Random.Range(0, 100);
                if (roll <= key)
                {
                    Create(tabledata[key], position, atPoint: true);
                }

            }
        }
        else if(typeof(T).Equals(typeof(PlayerData)))
        {
            var player = Resources.Load("Prefabs/Player") as PlayerInfo;
            var item = (PlayerData)(object)value;
            for (int i = 0; i < amount; i++)
            {
                var x = position.x + (atPoint ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));
                var z = position.z + (atPoint ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));

                var P = Object.Instantiate(player, new Vector3(x, position.y, z), Quaternion.identity).GetComponent<PlayerInfo>();
                P.name = $"[WORLD ITEM]: {item.Name}";
                P.data = item;
                P.transform.SetAsLastSibling();
            }
        }
    }

}