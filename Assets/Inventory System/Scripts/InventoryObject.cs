using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public enum InterfaceType
{
    Inventory,
    QuickSlot,
    Box
}
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemObjectDatabase database;
    public InterfaceType type;

    [SerializeField]
    private Inventory container = new Inventory();
    public InventorySlot[] slots => container.slots;

    public int EmptySlot
    {
        get
        {
            int counter = 0;
            foreach(InventorySlot slot in slots)
            {
                if(slot.item.id <0 )
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    public bool AddItem(Item item, int amount)
    {
        // 빈 슬롯이 없으면 추가 불가
        if (EmptySlot <= 0)
        {
            return false;
        }

        // item.id 값이 유효한지 확인 (0 이상이며 데이터베이스 범위 내인지)
        if (item.id < 0 || item.id >= database.itemObjects.Length)
        {
            Debug.LogError("Invalid item id: " + item.id + ". Cannot add item to inventory.");
            return false;
        }

        InventorySlot slot = FindItemInInventory(item);

        // 만약 아이템이 겹칠 수 없는 아이템이거나 같은 종류의 아이템이 슬롯에 없으면 빈 슬롯에 추가
        if (!database.itemObjects[item.id].stackable || slot == null)
        {
            GetEmptySlot().AddItem(item, amount);
        }
        else // 겹칠 수 있는 아이템이면 수량 증가
        {
            slot.AddAmount(amount);
        }
        return true;
    }


    public InventorySlot FindItemInInventory(Item item)
    {
        return slots.FirstOrDefault(i => i.item.id == item.id);
    }
    public InventorySlot GetEmptySlot()
    {
        return slots.FirstOrDefault(i => i.item.id < 0);
    }
    public bool IsContainItem(ItemObject itemObject)
    {
        return slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
    }

    //슬롯간 아이템 교환
    public void SwapItems(InventorySlot itemSlotA, InventorySlot itemSlotB)
    {
        if (itemSlotA == itemSlotB) return;
        if (itemSlotB.CanPlaceInSlot(itemSlotA.ItemObject) && itemSlotA.CanPlaceInSlot(itemSlotB.ItemObject))
        {
            InventorySlot tempSlot = new InventorySlot(itemSlotB.item, itemSlotB.amount);
            itemSlotB.UpdateSlot(itemSlotA.item, itemSlotA.amount);
            itemSlotA.UpdateSlot(tempSlot.item, tempSlot.amount);
        }
    }
}