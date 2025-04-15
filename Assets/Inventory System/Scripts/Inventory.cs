using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    public void Clear()
    {
        foreach(InventorySlot slot in slots)
        {
            //slot.UpdateSlot(new Item(), 0);
            slot.RemoveItem();
        }
    }
    //어떠한 아이템이 포함되어있는가
    public bool IsContain(ItemObject itemObject)
    {
        //슬롯에 있는 아이템의 아이디와 아이템 오브젝트의 id를 비교해서 null 이 아니면 동일한 종류의 아이템이 있다는 뜻
        //return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
        return Iscontain(itemObject.data.id);
    }
    public bool Iscontain(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
}
