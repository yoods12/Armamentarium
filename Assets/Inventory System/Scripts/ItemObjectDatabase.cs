using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemObjectDatabase : ScriptableObject
{
    public ItemObject[] itemObjects;

    // 예: PowerGear: 0~999, WeaponGear: 1000~1999, Block: 2000~2999 (원하는 값으로 조정)
    private static readonly Dictionary<ItemType, int> baseIds = new Dictionary<ItemType, int>()
    {
        { ItemType.PowerGear, 5 },
        { ItemType.WeaponGear, 3 },
        { ItemType.Block, 0 }
    };

    //public void OnValidate()
    //{
    //    if (itemObjects == null)
    //        return;

    //    // 각 아이템 타입별 카운터를 초기화합니다.
    //    Dictionary<ItemType, int> counters = new Dictionary<ItemType, int>();
    //    foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
    //    {
    //        counters[type] = 0;
    //    }

    //    for (int i = 0; i < itemObjects.Length; i++)
    //    {
    //        if (itemObjects[i] != null && itemObjects[i].data != null)
    //        {
    //            ItemType type = itemObjects[i].type;
    //            int baseId = baseIds[type];
    //            itemObjects[i].data.id = baseId + counters[type];
    //            counters[type]++;
    //        }
    //    }
    //}
}
