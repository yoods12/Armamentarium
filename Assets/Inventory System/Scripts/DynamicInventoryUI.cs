using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject slotPrefab;

    [SerializeField]
    protected Vector2 start;

    [SerializeField]
    protected Vector2 size;

    [SerializeField]
    protected Vector2 space; // 슬롯간 간격

    [Min(1), SerializeField]
    protected int numberOfColumns = 8;

    // 슬롯 UI 모두 한 부모에 생성 (두 개의 패널로 분리하지 않음)
    [SerializeField] private Transform inventoryPanel;

    public override void CreateSlotUIs()
    {
        if (inventoryObject == null || inventoryObject.slots == null)
        {
            Debug.LogError("InventoryObject or its slots are not assigned!");
            return;
        }

        slotsUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, inventoryPanel);
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = CalculatePosition(i);
            }
            else
            {
                Debug.LogWarning("Slot prefab is missing RectTransform.");
            }

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });

            inventoryObject.slots[i].slotUI = go;
            slotsUIs.Add(go, inventoryObject.slots[i]);
        }
    }
    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColumns));
        float y = start.y - ((size.y + space.y) * (i / numberOfColumns));
        return new Vector3(x, y, 0f);
    }

}
