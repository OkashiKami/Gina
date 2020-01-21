using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InventorySlot : Slot
{
    private Image icon;
    private TextMeshProUGUI count;

    public override void Awake()
    {
        if (!icon) icon = transform.Find("Icon").GetComponent<Image>();
        if (!count) count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
    }
    public override void Update()
    {
        if(item != null &&  item.IsValid)
        {
            if (item.IsStackable)
            {
                if (item.Get<int>(paramname.curStack) > 1)
                    count.text = item.Get<int>(paramname.curStack).ToString();
                else
                    count.text = string.Empty;
            }
            else count.text = string.Empty;

            if(!string.IsNullOrEmpty(item.Get<string>(paramname.icon)))
            {
                icon.sprite = item.Get<Sprite>(paramname.icon);
                icon.enabled = true;
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


    RectTransform _rt;
    Canvas _c;
    public override void OnDrop(PointerEventData eventData)
    {
        
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

    }

    public override void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log("OnBeginDrag");
        GetComponent<CanvasGroup>().alpha = 0.6f;
        _rt = Instantiate(this.gameObject).GetComponent<RectTransform>();
        _rt.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        GetComponent<CanvasGroup>().alpha = 1;

        Destroy(_rt.gameObject);
    }
}
