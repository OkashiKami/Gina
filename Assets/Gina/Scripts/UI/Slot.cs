using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

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