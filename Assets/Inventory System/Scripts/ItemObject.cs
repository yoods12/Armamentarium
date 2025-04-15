using System;
using UnityEngine;

public enum ItemType: int
{
    Block = 0,
    WeaponGear = 1,
    PowerGear = 2
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public bool stackable; //여러개 겹칠 수 있는지

    public Sprite icon; //아이템 이미지
    public GameObject modelprefab; //캐릭터에 부착되어야하는 3d모델 프리팹

    public Item data = new Item();

    [TextArea(15, 20)]
    public string description; //설명

    //아이템을 생성해서 반환하는 함수
    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

}
