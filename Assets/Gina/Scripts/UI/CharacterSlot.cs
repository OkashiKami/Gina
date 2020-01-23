using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CharacterSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public EquiptmentType requireType;
    public Item item = null;

    private Image icon;
    private TextMeshProUGUI count;
    private Image decal;



    public void Awake()
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
                case EquiptmentType.Weapon: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Weapon-Decal"); break;
                default: decal.sprite = Resources.Load<Sprite>("Textures/Armor-Add-Decal"); break;
            } 
        }
        else
        {
            Awake();
            goto a;
        }
    }
    public void Update()
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
                icon.sprite = item.Get<Sprite>(paramname.icon);
                icon.enabled = true;
                decal.enabled = false;
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
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        var player = FindObjectOfType<Player>().player_data;

        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<InventorySlot>())
            {
                if (requireType == EquiptmentType.None || requireType == (EquiptmentType)eventData.pointerDrag.GetComponent<InventorySlot>().item.Get<int>(paramname.equipmentType))
                {
                    Set(eventData.pointerDrag.GetComponent<InventorySlot>().item);
                    eventData.pointerDrag.GetComponent<InventorySlot>().Set();
                }
            }
            else if (eventData.pointerDrag.GetComponent<ActionbarSlot>())
            {
                if (requireType == EquiptmentType.None || requireType == (EquiptmentType)eventData.pointerDrag.GetComponent<ActionbarSlot>().item.Get<int>(paramname.equipmentType))
                {
                    Set(eventData.pointerDrag.GetComponent<ActionbarSlot>().item);
                    eventData.pointerDrag.GetComponent<ActionbarSlot>().Set();
                }
            }
            else if (eventData.pointerDrag.GetComponent<CharacterSlot>())
            {
                if (requireType == EquiptmentType.None || requireType == (EquiptmentType)eventData.pointerDrag.GetComponent<CharacterSlot>().item.Get<int>(paramname.equipmentType))
                {
                    Set(eventData.pointerDrag.GetComponent<CharacterSlot>().item);
                    eventData.pointerDrag.GetComponent<CharacterSlot>().Set();
                }
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_rt == null) return;
        Debug.Log("OnDrag");
        _rt.anchoredPosition += eventData.delta / _c.scaleFactor;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null || !item.IsValid) return;
        _c = GetComponentInParent<Canvas>();
        Debug.Log("OnBeginDrag");
        _rt = Instantiate(this.gameObject, _c.transform).GetComponent<RectTransform>();
        _rt.gameObject.name = "Slot [PLACEHOLDER]";
        _rt.SetAsLastSibling();
        _rt.position = GetComponent<RectTransform>().position;
        _rt.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _rt.sizeDelta = Vector2.one * 45f;

        if (_rt.GetComponent<InventorySlot>())
            Destroy(_rt.GetComponent<InventorySlot>());
        if (_rt.GetComponent<ActionbarSlot>())
            Destroy(_rt.GetComponent<ActionbarSlot>());
        if (_rt.GetComponent<CharacterSlot>())
            Destroy(_rt.GetComponent<CharacterSlot>());

        _rt.gameObject.AddComponent<PlaceholderSlot>().item = item;
        GetComponent<CanvasGroup>().alpha = 0.6f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_rt == null) return;
        Debug.Log("OnEndDrag");
        GetComponent<CanvasGroup>().alpha = 1;
        if (_rt != null)
            Destroy(_rt.gameObject);
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }


    public void Set(Item value = null)
    {
        var myindex = transform.parent.GetComponentsInChildren<CharacterSlot>().ToList().IndexOf(this);
        var player = FindObjectOfType<Player>().player_data;
        player.SetCharacter(myindex, value != null ? value.data : null);
    }
}
