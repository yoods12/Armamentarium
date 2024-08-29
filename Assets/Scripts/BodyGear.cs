using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGear : MonoBehaviour
{
    private Transform tr;
    private float rotSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseRotate();
    }

    private void MouseRotate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        float y = tr.localEulerAngles.y + mouseDelta.x * rotSpeed * Time.deltaTime;
        float x = tr.localEulerAngles.x - mouseDelta.y * rotSpeed * Time.deltaTime;

        if (x > 180f)
        {
            x -= 360f; // 180도를 초과하면 음수로 변환
        }
        if (y > 180f)
        {
            y -= 360f; // 180도를 초과하면 음수로 변환
        }
        x = Mathf.Clamp(x, -30, 30);
        y = Mathf.Clamp(y, -50, 50);

        tr.localRotation = Quaternion.Euler(x, y, 0);

    }
}
