using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject playerInventory; // 인벤토리 창(패널)
    [SerializeField] private GameObject playerTechTree; // 기술 트리 창(패널)
    [SerializeField] private GameObject ban; // 인벤토리 창(패널)
    [SerializeField] private GameObject aim; // 에임

    void Awake()
    {
        Singleton();
        // 초기 상태: 인벤토리 창은 꺼져있고 에임은 켜진 상태로 설정
        playerInventory.SetActive(false);
        playerTechTree.SetActive(false);
        ban.SetActive(false);

        aim.SetActive(true);
        // 초기 상태에서 마우스 커서를 숨기고 잠급니다.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MouseOnOff();
        TrainingCenterMouseOff();
    }
    void Singleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InventoryButton() // 인벤토리 버튼 클릭 시 인벤토리 껏다켰다 하는 함수
    {    
        // 현재 인벤토리 창의 활성 상태를 가져옴
        bool isActive = playerInventory.activeSelf; //true는 인벤토리가 꺼져있을때

        // 인벤토리 창 상태를 반대로 전환 (비활성 -> 활성, 활성 -> 비활성)
        playerInventory.SetActive(!isActive);
        ban.SetActive(!isActive);
        // aim은 인벤토리 창과 반대로 활성화
        aim.SetActive(isActive);

        // 인벤토리 창이 꺼지면 마우스 보이게함
        if(isActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void TechTree()
    {
        bool isActive = playerTechTree.activeSelf; //true는 인벤토리가 꺼져있을때

        // 인벤토리 창 상태를 반대로 전환 (비활성 -> 활성, 활성 -> 비활성)
        playerTechTree.SetActive(!isActive);
        // aim은 인벤토리 창과 반대로 활성화
        aim.SetActive(isActive);

        // 인벤토리 창이 꺼지면 마우스 보이게함
        if (isActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void TrainingCenterButton()
    {
        SceneManager.LoadScene("TrainingCenter");
    }

    void MouseOnOff()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && SceneManager.GetActiveScene().name == "Factory")
        {
            // 현재 커서 보임 상태를 확인합니다.
            bool isVisible = Cursor.visible;

            // 토글: 현재 상태의 반대로 전환
            Cursor.visible = !isVisible;

            // 커서가 보이면 잠금 해제, 숨겨지면 잠금
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void TrainingCenterMouseOff()
    {
        if(SceneManager.GetActiveScene().name == "TrainingCenter")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
