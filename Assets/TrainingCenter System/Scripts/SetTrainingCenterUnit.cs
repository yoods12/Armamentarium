using UnityEngine;
using UnityEngine.SceneManagement;
public class SetTrainingCenterUnit : MonoBehaviour
{
    public GameObject playerUnit; // 위치를 초기화 할 유닛
    public Vector3 unitPosition;
    public Quaternion unitRotation = Quaternion.identity;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "TrainingCenter")
        {
            SetPosition(); // 훈련장에서 위치 초기화
            //SetPhysics(); // 물리 초기화
        }
    }
    // 위치 초기화
    public void SetPosition()
    {
        if (playerUnit != null)
        {
            playerUnit.transform.position = unitPosition;
            playerUnit.transform.rotation = unitRotation;
        }
    }

    // 물리 초기화
    //public void SetPhysics()
    //{
       // if (playerUnit != null)
        //{
            //rb.isKinematic = false;
            //rb.useGravity = true;
        //}
    //}
}
