using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var player = FindObjectOfType<Player>();

        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<InventorySlot>())
            {
                WorldItem.Create(eventData.pointerDrag.GetComponent<InventorySlot>().item, player.transform.position);
                eventData.pointerDrag.GetComponent<InventorySlot>().Set();
            }
            else if (eventData.pointerDrag.GetComponent<ActionbarSlot>())
            {
                WorldItem.Create(eventData.pointerDrag.GetComponent<ActionbarSlot>().item, player.transform.position);
                eventData.pointerDrag.GetComponent<ActionbarSlot>().Set();
            }
            else if (eventData.pointerDrag.GetComponent<CharacterSlot>())
            {
                WorldItem.Create(eventData.pointerDrag.GetComponent<CharacterSlot>().item, player.transform.position);
                eventData.pointerDrag.GetComponent<CharacterSlot>().Set();
            }
        }
    }
}
