using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider FLWheelCol, FRWheelCol;
    [SerializeField] private WheelCollider BLWheelCol, BRWheelCol;

    // Wheels
    [SerializeField] private Transform FLWheelTr, FRWheelTr;
    [SerializeField] private Transform BLWheelTr, BRWheelTr;

    [SerializeField] private Transform bodyGear, coreGear;
    [SerializeField] private Transform rightWeaponGear, leftWeaponGear;

    private float h, v, x, y;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    Rigidbody rb;
    Transform playerTransform;
    private float rotSpeed = 200f;
    private float currentRot;

    // 회전 제한 변수
    //private float maxBodyGearRotationX = 60f; // 최대 상하 회전 각도
    //private float maxBodyGearRotationY = 90f; // 최대 좌우 회전 각도
    //private Vector3 currentBodyGearRotation;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        //currentBodyGearRotation = bodyGear.localEulerAngles; // 초기 회전 값 저장
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        UpdateGearLocation();
    }
    void Update()
    {
        MouseRotate();
    }
    private void GetInput()
    {
        // Steering Input
        h = Input.GetAxisRaw("Horizontal");

        // Acceleration Input
        v = Input.GetAxisRaw("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);

    }

    private void HandleMotor()
    {
        FLWheelCol.motorTorque = v * motorForce;
        FRWheelCol.motorTorque = v * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        FRWheelCol.brakeTorque = currentbreakForce;
        FLWheelCol.brakeTorque = currentbreakForce;
        BLWheelCol.brakeTorque = currentbreakForce;
        BRWheelCol.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * h;
        FLWheelCol.steerAngle = currentSteerAngle;
        FRWheelCol.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(FLWheelCol, FLWheelTr);
        UpdateSingleWheel(FRWheelCol, FRWheelTr);
        UpdateSingleWheel(BRWheelCol, BRWheelTr);
        UpdateSingleWheel(BLWheelCol, BLWheelTr);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot); 

        wheelTransform.position = Vector3.Lerp(wheelTransform.position, pos, Time.deltaTime * 10f); // 10f는 보간 속도
        wheelTransform.rotation = Quaternion.Slerp(wheelTransform.rotation, rot, Time.deltaTime * 10f);
    }
    private void UpdateGearLocation()
    {
        bodyGear.localPosition = new Vector3(0, 1.58f, 0);
        
        //coreGear.localPosition = new Vector3(0, 0, 0);
        rightWeaponGear.localPosition = new Vector3(0.41f, 0.41f, 0);
        leftWeaponGear.localPosition = new Vector3(-0.41f, 0.41f, 0);
    }
    private void MouseRotate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        float y = bodyGear.localEulerAngles.y + mouseDelta.x * rotSpeed * Time.deltaTime;
        float x = bodyGear.localEulerAngles.x - mouseDelta.y * rotSpeed * Time.deltaTime;

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

        bodyGear.localRotation = Quaternion.Euler(x, y, 0);

    }
}