using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    internal Item item = null;

    public abstract void Awake();
    public abstract void Update();

    public abstract void OnBeginDrag(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnDrop(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);
    public void Set(Item value = null) => this.item = value;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{name} was {eventData.button} clicked!");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Slot), editorForChildClasses: true)]
public class SlotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tar = (Slot)target;

        if (tar.item == null || !tar.item.IsValid())
            EditorGUILayout.HelpBox($"No Item in {tar.name}", MessageType.Warning);
        else
        {
            foreach (var key in tar.item.data.Keys)
            {
                EditorGUILayout.LabelField(new GUIContent(key.ToString()), new GUIContent(tar.item.data[key].ToString()));
            }
        }
    }
}
#endif