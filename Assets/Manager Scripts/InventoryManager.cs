// InventoryManager.cs (&#8203;:contentReference[oaicite:0]{index=0})
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryObject inventory;

    // 추가: 현재 선택된 슬롯
    public InventorySlot selectedSlot;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
