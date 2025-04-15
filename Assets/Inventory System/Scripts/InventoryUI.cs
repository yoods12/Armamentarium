using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UI = UnityEngine.UI;

public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver;
    public static GameObject slotHoveredOver;
    public static GameObject tempItemBeingDragged;
}

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject inventoryObject;
    private InventoryObject previousInventoryObejct;

    public Dictionary<GameObject, InventorySlot> slotsUIs = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlotUIs(); // 슬롯 UI 생성

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            inventoryObject.slots[i].parent = inventoryObject;
            inventoryObject.slots[i].onPostUpdate = OnPostUpdate;
        }
        previousInventoryObejct = inventoryObject;

        // 인벤토리 창에 마우스가 들어오거나 나갈 때 이벤트 처리
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

        // 추가: 각 슬롯 UI에 PointerClick 이벤트 등록하여 슬롯 선택 기능 구현
        foreach (GameObject slotUI in new List<GameObject>(slotsUIs.Keys))
        {
            AddEvent(slotUI, EventTriggerType.PointerClick, (data) => OnSlotClicked(slotUI));
        }
    }

    protected virtual void Start()
    {
        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            inventoryObject.slots[i].UpdateSlot(previousInventoryObejct.slots[i].item, previousInventoryObejct.slots[i].amount);
        }
    }

    public abstract void CreateSlotUIs();

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // EventTrigger 컴포넌트가 없다면 추가
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = go.AddComponent<EventTrigger>();
        }
        // triggers 리스트가 null인 경우 초기화
        if (trigger.triggers == null)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        // 이벤트 엔트리 생성 및 추가
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(0).GetComponent<UI.Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<UI.Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString("n0"));
    }
    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }
    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnEnterSlot(GameObject go)
    {
        MouseData.slotHoveredOver = go;
    }
    public void OnExitSlot(GameObject go)
    {
        MouseData.slotHoveredOver = null;
    }
    //드래그 시작
    public void OnStartDrag(GameObject go)
    {
        MouseData.tempItemBeingDragged = CreateDragImage(go);
    }
    //드래그 이미지 생성
    private GameObject CreateDragImage(GameObject go)
    {
        if (slotsUIs[go].item.id < 0)
        {
            return null;
        }
        GameObject dragImageGo = new GameObject();
        RectTransform rectTransform = dragImageGo.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        dragImageGo.transform.SetParent(transform.parent);
        UI.Image image = dragImageGo.AddComponent<UI.Image>();
        image.sprite = slotsUIs[go].ItemObject.icon;
        image.raycastTarget = false;

        dragImageGo.name = "DragImage";
        return dragImageGo;
    }
    //이미지가 마우스 따라가게 함
    public void OnDrag(GameObject go)
    {
        if (MouseData.tempItemBeingDragged == null)
        {
            return;
        }
        MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }
    //드래그 끝
    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsUIs[go].RemoveItem(); //아이템을 인터페이스(UI) 밖으로 드래그 하면 아이템 삭제
        }
        else if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsUIs[MouseData.slotHoveredOver];
            inventoryObject.SwapItems(slotsUIs[go], mouseHoverSlotData); //아이템 교체
        }
    }

    // 새로 추가: 슬롯 클릭 시 호출되어 InventoryManager에 선택된 슬롯을 등록하는 메서드
    protected void OnSlotClicked(GameObject slotUI)
    {
        // InventoryManager 인스턴스의 selectedSlot 변수에 선택된 슬롯을 저장
        InventoryManager.instance.selectedSlot = slotsUIs[slotUI];
        // 예시로, 선택된 슬롯의 정보를 로그로 출력하거나 UI 하이라이트 처리 가능
        Debug.Log("Selected slot with item: " + slotsUIs[slotUI].item.name);
    }
}
