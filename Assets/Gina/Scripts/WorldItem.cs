using CodeMonkey.Utils;
using System;
using TMPro;
using UnityEngine;
using Invector.CharacterController;
#if UNITY_EDITOR
using UnityEditor;
#endif
internal class WorldItem : MonoBehaviour
{
    private SpriteRenderer icon;
    private TextMeshPro amount;
    public  Item item = null;

    internal static void Create(Item item, Vector3 position = default)
    {
        var wip = Resources.Load("Prefabs/World_Item") as GameObject;
        var x = position.x + UnityEngine.Random.Range(-3, 3);
        var z = position.z + UnityEngine.Random.Range(-3, 3);

        var wi = Instantiate(wip, new Vector3(x, position.y, z), Quaternion.identity).GetComponent<WorldItem>();
        wi.name = $"{item.Get<string>(paramname.name)}_WI";
        wi.item = item;
    }

    private void Awake()
    {
        icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
        amount = transform.Find("Amount").GetComponent<TextMeshPro>();

        FunctionPeriodic.Create(() =>
        {
            Destroy(this.gameObject);
        }, 300);

        FindObjectOfType<InputController>().onInteract += OnInteract;
    }

    private void OnInteract(Player player)
    {
        var dis = Vector3.Distance(player.transform.position, transform.position);
        if (dis < 0.6f)
        {
            player.player_data.SetInventory(value: item.data);
            Destroy(gameObject);
        }
        Debug.Log($"Interact {dis}");
    }

    private void Update()
    {
        if(icon)
        {
            icon.transform.Rotate(Vector3.up, 3, Space.World);

            if (item != null && item.IsValid)
                icon.sprite = item.Get<Sprite>(paramname.icon);
            else
                icon.sprite = null;
            icon.enabled = icon.sprite != null;
        }
        if (amount)
        {
            amount.transform.LookAt(Camera.main.transform, Vector3.up);
            if (item != null && item.IsValid)
                amount.text = item.Has(paramname.curStack) ? item.Get<int>(paramname.curStack).ToString() : string.Empty;
            else
                amount.text = string.Empty;
            amount.enabled = !string.IsNullOrEmpty(amount.text);
        }        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WorldItem))]
public class WorldItemEdtir : Editor
{
    public override void OnInspectorGUI()
    {
        Repaint();
        var tar = (WorldItem)target;
        if(tar.item.IsValid)
        {
            foreach (var item in tar.item.data)
            {
                EditorGUILayout.LabelField(new GUIContent(item.Key.ToString()),  new GUIContent(item.Value.ToString()));
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No Item has be set!", MessageType.Warning);
        }
        base.OnInspectorGUI();
    }
}
#endif