using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ActionbarSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    
    public EquiptmentType requireType;
    public Item item = null;

    public Image icon;
    private Image cooldown;
    private TextMeshProUGUI cooldowntext;
    private Image castup;
    private TextMeshProUGUI count;

    public void Awake()
    {
        if (!icon) icon = transform.Find("Icon").GetComponent<Image>();
        if (!cooldown) cooldown = transform.Find("Cooldown").GetComponent<Image>();
        if (!castup) castup = transform.Find("Castup").GetComponent<Image>();
        if (!cooldowntext) cooldowntext = transform.Find("CooldownText").GetComponent<TextMeshProUGUI>();
        if (!count) count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
    }
    public void Update()
    {
        if(item != null)
        {
            if (item.IsStackable)
            {
                if (item.Get<int>(pname.curStack) > 1)
                    count.text = item.Get<int>(pname.curStack).ToString();
                else
                    count.text = string.Empty;
            }
            else count.text = string.Empty;

            if(!string.IsNullOrEmpty(item.Get<string>(pname.icon)))
            {
                icon.sprite = item.Get<Sprite>(pname.icon);
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
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        var player = FindObjectOfType<Player>().player_data;
        var _item = item == null || !item.IsValid ? null : item.Copy;

        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<InventorySlot>())
            {
                if (requireType == EquiptmentType.None || requireType == (EquiptmentType)eventData.pointerDrag.GetComponent<InventorySlot>().item.Get<int>(pname.equipmentType))
                {
                    Set(eventData.pointerDrag.GetComponent<InventorySlot>().item);
                    eventData.pointerDrag.GetComponent<InventorySlot>().Set(_item);
                }
            }
            else if (eventData.pointerDrag.GetComponent<ActionbarSlot>())
            {
                if (requireType == EquiptmentType.None || requireType == (EquiptmentType)eventData.pointerDrag.GetComponent<ActionbarSlot>().item.Get<int>(pname.equipmentType))
                {
                    Set(eventData.pointerDrag.GetComponent<ActionbarSlot>().item);
                    eventData.pointerDrag.GetComponent<ActionbarSlot>().Set(_item);
                }
            }
            else if (eventData.pointerDrag.GetComponent<CharacterSlot>())
            {
                if (requireType == EquiptmentType.None || requireType == (EquiptmentType)eventData.pointerDrag.GetComponent<CharacterSlot>().item.Get<int>(pname.equipmentType))
                {
                    Set(eventData.pointerDrag.GetComponent<CharacterSlot>().item);
                    eventData.pointerDrag.GetComponent<CharacterSlot>().Set(_item);
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
        var myindex = transform.parent.GetComponentsInChildren<ActionbarSlot>().ToList().IndexOf(this);
        var player = FindObjectOfType<Player>().player_data;
        player.SetActionbar(myindex, value != null ? value.data : null);
    }
}
