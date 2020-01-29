using CodeMonkey.Utils;
using System;
using TMPro;
using UnityEngine;
using Invector.CharacterController;
using System.Collections.Generic;
using System.Linq;

internal class WorldItem : MonoBehaviour
{
    public static void Create<T>(T value = default, Vector3 position = default, int amount = 1, float despawn = 300, bool atPoint = false)
    {
        if (value == null) return;
        var wip = Resources.Load("Prefabs/World_Item") as GameObject;

        if (typeof(T).Equals(typeof(ItemData)))
        {
            var item = (ItemData)(object)value; 

            for (int i = 0; i < amount; i++)
            {
                var x = position.x + (atPoint ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));
                var z = position.z + (atPoint ? UnityEngine.Random.Range(-.5f, .5f) : UnityEngine.Random.Range(-3, 3));

                var wi = Instantiate(wip, new Vector3(x, position.y, z), Quaternion.identity).GetComponent<WorldItem>();
                wi.name = $"[WORLD ITEM]: {item.name}";
                wi.item = item;
                wi.despan = despawn;
                wi.transform.SetAsLastSibling();
            }
        }
        else if(typeof(T).Equals(typeof(LootData)))
        {
            var lootT = (LootData)(object)value;

            Dictionary<int, ItemData> tabledata = new Dictionary<int, ItemData>();
            for (int i = 0; i < lootT.items.Count; i++)
            {
                var entry = lootT.items[i];
                tabledata.Add(entry.dropPercentage, Database.Load<ItemData>(entry.item_indicator));
            }

            foreach (var key in tabledata.Keys)
            {
                var roll = UnityEngine.Random.Range(0, 100);
                if (roll <= key)
                {
                    WorldItem.Create(tabledata[key], position, atPoint: true);
                }

            }
        }
    }


    private SpriteRenderer icon;
    private TextMeshPro amount;
    public  ItemData item = null;
    public float despan;

    private void Awake()
    {
        icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
        amount = transform.Find("Amount").GetComponent<TextMeshPro>();
        FindObjectOfType<InputController>().onInteract += OnInteract;
    }

    

    private void OnInteract(PlayerInfo player)
    {
        if (player == null) return;
        var dis = Vector3.Distance(player.transform.position, transform.position);
        if (dis < 1.03f)
        {
            player.data.ModifyInvintory(value: item);
            FindObjectOfType<InputController>().onInteract -= OnInteract;
            Destroy(gameObject);
        }
        Debug.Log($"Interact {dis}");
    }

    private void Start()
    {
    }

    private void Update()
    {
        if(despan > 0)
        {

            despan -= 1 * Time.deltaTime;
            if(despan <= 0)
            {
                Destroy(gameObject);
            }
            name = $"[WI][{despan.ToString("n0")}s]: {item.name}";
        }

        if(icon)
        {
            icon.transform.Rotate(Vector3.up, 3, Space.World);

            if (item != null && item.IsValid)
                icon.sprite = item.Sprite;
            else
                icon.sprite = null;
            icon.enabled = icon.sprite != null;
        }
        if (amount)
        {
            amount.transform.LookAt(Camera.main.transform, Vector3.up);
            if (item != null && item.IsValid && item.isStackable)
                amount.text = item.curStack > 1 ? item.curStack.ToString() : string.Empty;
            else
                amount.text = string.Empty;
            amount.enabled = !string.IsNullOrEmpty(amount.text);
        }        


    }
}