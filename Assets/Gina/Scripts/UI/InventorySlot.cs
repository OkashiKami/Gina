using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{

    public Image icon;
    public TextMeshProUGUI count;
    private Dictionary<Options, object> data = null;

    public void Update()
    {
        if(data != null)
        {
            if (OptionManager.Get<bool>(Options.stackable, data))
            {
                if (OptionManager.Get<int>(Options.curStack, data) > 1)
                    count.text = OptionManager.Get<int>(Options.curStack, data).ToString();
                else
                    count.text = string.Empty;
            }
            else count.text = string.Empty;

            if(!string.IsNullOrEmpty(OptionManager.Get<string>(Options.icon, data)))
            {

            }
            else
            {
                icon.enabled = false;
                icon.sprite = null;
            }
        }
        else
        {
            count.text = string.Empty;
            icon.enabled = false;
            icon.sprite = null;
        }
        
    }

    public void Set(Dictionary<Options, object> data = null) => this.data = data;

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
