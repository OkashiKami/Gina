using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class PlaceholderSlot : MonoBehaviour
{
    public Item item;
    private Image icon;
    private TextMeshProUGUI count;

    public void Awake()
    {
        if (!icon) icon = transform.Find("Icon").GetComponent<Image>();
        if (!count) count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {

        if (item != null && item.IsValid)
        {
            if (item.IsStackable)
            {
                if (item.Get<int>(pname.curStack) > 1)
                    count.text = item.Get<int>(pname.curStack).ToString();
                else
                    count.text = string.Empty;
            }
            else count.text = string.Empty;

            if (!string.IsNullOrEmpty(item.Get<string>(pname.icon)))
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

}