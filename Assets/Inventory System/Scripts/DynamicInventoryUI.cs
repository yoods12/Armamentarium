using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField] protected GameObject slotPrefab;
    [SerializeField] protected Vector2 start;
    [SerializeField] protected Vector2 size;
    [SerializeField] protected Vector2 space;
    [Min(1), SerializeField] protected int numberOfColumns = 8;
    [SerializeField] private Transform inventoryPanel;

    public override void CreateSlotUIs()
    {
        if (inventoryObject == null || inventoryObject.slots == null)
        {
            Debug.LogError("InventoryObject or its slots are not assigned!");
            return;
        }

        // 기존 키 비우고
        slotsUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            var slotData = inventoryObject.slots[i];

            // 1) 슬롯 생성 및 위치 지정
            GameObject go = Instantiate(slotPrefab, inventoryPanel);
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt != null)
                rt.anchoredPosition = CalculatePosition(i);

            // 2) 슬롯-데이터 연결 + 연구 잠금 처리
            SetupSlotUI(go, slotData);

            // 3) 슬롯UI ↔ 슬롯 데이터 매핑
            slotData.slotUI = go;
            slotsUIs.Add(go, slotData);
        }
    }

    // 위치 계산은 기존 그대로
    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColumns));
        float y = start.y - ((size.y + space.y) * (i / numberOfColumns));
        return new Vector3(x, y, 0f);
    }
}
