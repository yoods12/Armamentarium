using UnityEngine;

public class PlayerMovermentController : MonoBehaviour
{
    public GameObject buildingAim;

    private CharacterController controller;

    private float moveSpeed = 4;
    private float rotationSpeed = 200f;
    private float mx = 0;
    private float my = 0;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        playerKeyMove();
        playerMouseMove();
    }

    void playerKeyMove()
    {
        bool isMouseVisible = Cursor.visible;
        bool isAimActive = buildingAim.activeSelf;

        if (!isMouseVisible && isAimActive)
        {

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 horizontalMove = transform.right * h + transform.forward * v;

            // 수직 이동: 스페이스바 -> 위, 쉬프트 -> 아래
            float verticalInput = 0;
            if (Input.GetKey(KeyCode.Space))
                verticalInput = 0.5f;
            if (Input.GetKey(KeyCode.LeftShift))
                verticalInput = -0.5f;
            Vector3 verticalMove = Vector3.up * verticalInput;

            // 최종 이동 벡터
            Vector3 move = (horizontalMove + verticalMove) * moveSpeed;

            controller.Move(move * moveSpeed * Time.deltaTime);
        }
    }

    void playerMouseMove()
    {
        bool isMouseVisible = Cursor.visible;
        bool isAimActive = buildingAim.activeSelf;

        if (!isMouseVisible && isAimActive)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            mx += mouseX * rotationSpeed * Time.deltaTime;
            my += mouseY * rotationSpeed * Time.deltaTime;

            my = Mathf.Clamp(my, -90f, 90f);

            transform.eulerAngles = new Vector3(-my, mx, 0);
        }
    }
}
