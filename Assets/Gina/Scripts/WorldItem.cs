using CodeMonkey.Utils;
using System;
using TMPro;
using UnityEngine;
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
        wi.name = $"{item.Get<string>(Options.name)}_WI";
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
    }

    private void Update()
    {
        if(icon)
        {
            icon.transform.Rotate(Vector3.up, 3, Space.World);

            if (item != null && item.IsValid())
                icon.sprite = item.Get<Sprite>(Options.icon);
            else
                icon.sprite = null;
            icon.enabled = icon.sprite != null;
        }
        if (amount)
        {
            amount.transform.LookAt(Camera.main.transform, Vector3.up);
            if (item != null && item.IsValid())
                amount.text = item.Has(Options.curStack) ? item.Get<int>(Options.curStack).ToString() : string.Empty;
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
        if(tar.item.IsValid())
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