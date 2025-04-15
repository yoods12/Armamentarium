using UnityEngine;

public class TestItem : MonoBehaviour
{
    public InventoryObject inventoryObject;
    public ItemObjectDatabase itemObjectDatabase;
    public Inventory inventory;

    public void AddItem()
    {
        if(itemObjectDatabase.itemObjects.Length > 0)
        {
            ItemObject newItemobejct = itemObjectDatabase.itemObjects[Random.Range(0, itemObjectDatabase.itemObjects.Length)];
            Item newItem = new Item(newItemobejct);
            inventoryObject.AddItem(newItem, 1);
        }
    }

    public void RemoveItem()
    {
        if (inventory.slots.Length > 0)
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                inventoryObject.slots[i].RemoveItem();
            }
        }
    }
}
