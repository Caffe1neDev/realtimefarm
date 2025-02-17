using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] ItemContainer inventory;
    [SerializeField] List<InventorySlot> slots;
    private void Start() 
    {
        SetIndex();
        ShowItem();
    }

 

    private void SetIndex()
    {
        for(int i = 0; i<inventory.slots.Count; i++){
            slots[i].SetIndex(i);
        }
    }

    private void ShowItem()
    {
        for(int i = 0; i< inventory.slots.Count; i++)
        {
            if(inventory.slots[i].item == null)
            {
                slots[i].itemClean();
            }
            
            else
            {
                slots[i].itemSet(inventory.slots[i]);
            }
        }
    }
}
