using UnityEngine;

public class Unlock : MonoBehaviour
{
    [Tooltip("Inspector에서 드래그할 GreyBlockBan 오브젝트")]
    public GameObject ban;
    public GameObject button;

    // 버튼의 OnClick에 등록할 메서드
    public void UnlockButton()
    {
        if (ban != null)
        {
            Destroy(ban);
            Destroy(button);
            Debug.Log("GreyBlockBan 오브젝트를 삭제했습니다.");
        }
        else
        {
            Debug.LogWarning("greyBlockBan이 할당되지 않았습니다!");
        }
    }
}
