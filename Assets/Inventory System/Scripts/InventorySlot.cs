using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    //슬롯에 헬멧과 갑옷을 넣고싶다라고 하면 배열이 2개가 되어야해서 배열로 추가함
    public ItemType[] allowerItems = new ItemType[0];

    //parent는 초기화하면서 자동으로 넣어줘서 직렬화 하지않음, json으로 변환할때 제외됨
    //아이템 슬롯을 소유하고있는 부모를 지정하는 인벤토리 오브젝트
    [NonSerialized]
    public InventoryObject parent;

    //슬롯을 표시하는 UI 오브젝트 (프리팹 아님), 프리팹에서 인스턴스화 된 게임 오브젝트를 가지고 올 수 있도록 UI와 연동
    [NonSerialized]
    public GameObject slotUI;

    //슬롯에 아이템 갱신될때 호출할 이벤트
    [NonSerialized]
    public Action<InventorySlot> onPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> onPostUpdate;

    //실제 아이템 슬롯에 들어있는 아이템
    public Item item;
    public int amount;

    //슬롯에 있는 아이템의 데이터 가져오는거
    public ItemObject ItemObject
    {
        get
        {
            //db에서 아이템 찾아서 id 반환
            return item.id >= 0 ? parent.database.itemObjects[item.id] : null;
        }
    }
    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item item, int amount)
    {
        UpdateSlot(item, amount);
    }

    public void AddItem(Item item, int amount) => UpdateSlot(item, amount);
    public void RemoveItem() => UpdateSlot(new Item(), 0);
    public void AddAmount(int value) => UpdateSlot(item, amount += value);

    //슬롯 갱신
    public void UpdateSlot(Item item, int amount)
    {
        onPreUpdate?.Invoke(this);

        this.item = item;
        this.amount = amount;

        onPostUpdate?.Invoke(this);
    }
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        //슬롯에 아이템이 없으면 true
        if (allowerItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            return true;
        return false;
    }
}
