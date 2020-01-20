using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CharacterSlot : Slot
{
    private Image icon;
    private TextMeshProUGUI count;
    private Image decal;
    public EquiptmentType requireType;
    public override void Awake()
    {
        if (!icon) icon = transform.Find("Icon").GetComponent<Image>();
        if (!count) count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
        if (!decal) decal = transform.Find("Decal").GetComponent<Image>();
    }

    private void OnValidate()
    {
        a:
        if(decal)
        {
            switch(requireType)
            {
                case EquiptmentType.Helmet: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Helmet-Decal"); break;
                case EquiptmentType.Chestplate: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Chestplate-Decal"); break;
                case EquiptmentType.Leggings: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Leggings-Decal"); break;
                case EquiptmentType.Boots: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Boots-Decal"); break;
                case EquiptmentType.Glovs: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Glovs-Decal"); break;
                case EquiptmentType.Necklace: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Necklace-Decal"); break;
                case EquiptmentType.Ring: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Ring-Decal"); break;
                default: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Add-Decal"); break;
            }
        }
        else
        {
            Awake();
            goto a;
        }
    }

    public override void Update()
    {
        if(item != null)
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
