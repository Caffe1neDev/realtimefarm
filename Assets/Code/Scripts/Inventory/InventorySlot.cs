using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text text;

    int ItemIndex;

    public void SetIndex(int index)
    {
        ItemIndex = index;
    }

    public void itemSet(ItemSlot slot)
    {
        icon.sprite = slot.item.icon;

        if(slot.item.stackable == true)
        {
            text.gameObject.SetActive(true);
            text.text = slot.count.ToString();
        }

        else
        {
            text.gameObject.SetActive(false);
        }
    }

    public void itemClean()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
