using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ActionbarSlot : Slot
{
    private Image icon;
    private Image cooldown;
    private TextMeshProUGUI cooldowntext;
    private Image castup;
    private TextMeshProUGUI count;

    public override void Awake()
    {
        if (!icon) icon = transform.Find("Icon").GetComponent<Image>();
        if (!cooldown) cooldown = transform.Find("Cooldown").GetComponent<Image>();
        if (!castup) castup = transform.Find("Castup").GetComponent<Image>();
        if (!cooldowntext) cooldowntext = transform.Find("CooldownText").GetComponent<TextMeshProUGUI>();
        if (!count) count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
    }
    public override void Update()
    {
        if(item != null)
        {
            if (item.Get<bool>(Options.stackable))
            {
                if (item.Get<int>(Options.curStack) > 1)
                    count.text = item.Get<int>(Options.curStack).ToString();
                else
                    count.text = string.Empty;
            }
            else count.text = string.Empty;

            if(!string.IsNullOrEmpty(item.Get<string>(Options.icon)))
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

    public override void OnDrop(PointerEventData eventData)
    {
        
    }

    public override void OnDrag(PointerEventData eventData)
    {
        
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
