using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int count;
}

[CreateAssetMenu(menuName ="Data/Item Container")]
public class ItemContainer : ScriptableObject
{
   public List<ItemSlot> slots;

   public void AddItem(Item item , int count = 1)
   {
        if(item.stackable == true)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == item);
            if(itemSlot != null)
            {
                itemSlot.count += count;
            }

            else
            {
                itemSlot = slots.Find(x => x.item == null);
                if(itemSlot != null)
                {
                    itemSlot.item = item;
                    itemSlot.count = count;
                }
            }
        }
        
        else    // 한개로 제한 되는 아이템
        {
            ItemSlot itemSlot = slots.Find(X=>X.item == null);
            if(itemSlot != null)
            {
                itemSlot.item = item;
            }
        }
   }
}
