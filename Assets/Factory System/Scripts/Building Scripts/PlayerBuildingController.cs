using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingController : MonoBehaviour
{

    [SerializeField] private GameObject inventoryUI; // 인벤토리 창(패널)

    [Header("Block Settings")]
    // 기본 블럭 프리팹 (인벤토리 선택이 없을 경우 사용)
    public GameObject prefab;

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; // 초당 회전 속도 (도 단위)

    public Transform playerUnit; // 플레이어 유닛의 Transform (부모)

    public GameObject buildingAim; // 플레이어 빌딩 에임 오브젝트

    private GameObject preview;  // 미리보기 ghost 인스턴스
    private Camera mainCamera;

    // 현재 사용할 프리팹. 인벤토리에서 선택된 아이템의 modelprefab가 있으면 해당 프리팹을 사용
    private GameObject currentPrefab;

    void Start()
    {
        mainCamera = Camera.main;
        // 초기엔 기본 프리팹으로 설정
        currentPrefab = prefab;
        CreatePreview();
    }

    void Update()
    {
        bool isMouseVisible = Cursor.visible;
        bool isAimActive = buildingAim.activeSelf;

        // 인벤토리에서 선택된 슬롯이 존재하고, 해당 슬롯에 아이템이 등록되어 있으며 modelprefab가 있다면 currentPrefab 업데이트
        if (InventoryManager.instance != null &&
            InventoryManager.instance.selectedSlot != null &&
            InventoryManager.instance.selectedSlot.ItemObject != null &&
            InventoryManager.instance.selectedSlot.ItemObject.modelprefab != null)
        {
            currentPrefab = InventoryManager.instance.selectedSlot.ItemObject.modelprefab;
        }
        else
        {
            currentPrefab = prefab;
        }

        // 미리보기 오브젝트가 없거나 현재 프리팹과 다른 경우 새로 생성
        if (preview == null || preview.name != currentPrefab.name + "(Clone)")
        {
            CreatePreview();
        }
        if (isAimActive && !isMouseVisible)
        {
            UpdatePreview();
            HandleRotation();
        }
        if (Input.GetMouseButtonDown(0) && isAimActive && !isMouseVisible)
        {
            PlaceBlock();
        }
        if (Input.GetMouseButtonDown(1) && isAimActive && !isMouseVisible)
        {
            DestroyBlock();
        }
    }

    // 미리보기 인스턴스를 생성하고 ghost 상태로 설정
    void CreatePreview()
    {
        if (currentPrefab == null)
            return;

        if (preview != null)
            Destroy(preview);

        preview = Instantiate(currentPrefab);
        MakePreview(preview);
        preview.SetActive(false);
    }

    // 미리보기 오브젝트를 ghost 상태로 설정 (Collider 비활성화, 머티리얼 반투명 처리)
    void MakePreview(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<BoxCollider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            Material mat = rend.material;
            mat.color = new Color(0f, 1f, 0f, 0.5f);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
    }

    // 화면 중앙(플레이어 에임)을 기준으로 레이캐스트하여 미리보기 위치를 업데이트
    void UpdatePreview()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Block") && !hitInfo.collider.CompareTag("Power Gear") &&
                !hitInfo.collider.CompareTag("Weapon") && currentPrefab.CompareTag("Block")) // 블럭에 블럭 설치
            {
                Vector3 spawnSpot = hitInfo.collider.transform.position + hitInfo.normal;
                preview.transform.position = spawnSpot;
                preview.transform.rotation = currentPrefab.transform.rotation;
                preview.SetActive(true);
            }
            else if (hitInfo.collider.CompareTag("Block") && currentPrefab.CompareTag("Weapon")) // 블럭에 무기 설치
            {
                Vector3 spawnSpot = hitInfo.collider.transform.position + hitInfo.normal;
                preview.transform.position = spawnSpot;
                preview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                preview.SetActive(true);
            }
            else if (hitInfo.collider.CompareTag("Block") && currentPrefab.CompareTag("Power Gear")) // 블럭에 파워기어 설치
            {
                Vector3 spawnSpot = hitInfo.collider.transform.position + hitInfo.normal;
                preview.transform.position = spawnSpot;
                preview.transform.rotation = Quaternion.FromToRotation(-Vector3.right, hitInfo.normal);
                preview.SetActive(true);
            }

            else
            {
                preview.SetActive(false);
            }
        }
        else
        {
            preview.SetActive(false);
        }
    }

    // Q/E 키를 통해 미리보기 오브젝트 회전 처리 (Y축 기준)
    void HandleRotation()
    {
        if (preview == null || !preview.activeSelf)
            return;

        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q))
            rotationInput = -1f;
        if (Input.GetKey(KeyCode.E))
            rotationInput = 1f;

        if (rotationInput != 0f)
        {
            preview.transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    // 좌클릭 시 미리보기 위치에서 실제 블럭 설치
    void PlaceBlock()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;

        bool isInventoryActive = inventoryUI.activeSelf; //인벤토리 켜져있을 때 true

        if (Physics.Raycast(ray, out hitInfo) && !isInventoryActive)
        {
            if (hitInfo.collider.CompareTag("Block") && !hitInfo.collider.CompareTag("Power Gear") &&
                !hitInfo.collider.CompareTag("Weapon") && currentPrefab.CompareTag("Block")) // 블럭에 블럭 설치
            {
                Vector3 spawnSpot = hitInfo.collider.transform.position + hitInfo.normal;
                Quaternion spawnRot = currentPrefab.transform.rotation;
                Instantiate(currentPrefab, spawnSpot, spawnRot, playerUnit);
            }
            else if (hitInfo.collider.CompareTag("Block") && currentPrefab.CompareTag("Weapon")) // 블럭에 무기 설치
            {
                Vector3 spawnSpot = hitInfo.collider.transform.position + hitInfo.normal;
                Quaternion spawnRot = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                Instantiate(currentPrefab, spawnSpot, spawnRot, playerUnit);
            }
            else if (hitInfo.collider.CompareTag("Block") && currentPrefab.CompareTag("Power Gear")) // 블럭에 파워기어 설치
            {
                Vector3 spawnSpot = hitInfo.collider.transform.position + hitInfo.normal;
                Quaternion spawnRot = Quaternion.FromToRotation(-Vector3.right, hitInfo.normal);
                Instantiate(currentPrefab, spawnSpot, spawnRot, playerUnit);
            }
        }
    }
    void DestroyBlock()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;

        bool isInventoryActive = inventoryUI.activeSelf; //인벤토리 켜져있을때 true

        if (Physics.Raycast(ray, out hitInfo) && !isInventoryActive)
        {
            Debug.Log("t");
            string objectName = hitInfo.collider.gameObject.name;
            if (hitInfo.collider.CompareTag("Block") && !objectName.Contains("Core"))
            {
                Destroy(hitInfo.collider.gameObject);
            }
            else if (hitInfo.collider.CompareTag("Power Gear") && !objectName.Contains("Core"))
            {
                Destroy(hitInfo.collider.gameObject);
            }
            else if (hitInfo.collider.CompareTag("Weapon") && !objectName.Contains("Core"))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
    }
}
