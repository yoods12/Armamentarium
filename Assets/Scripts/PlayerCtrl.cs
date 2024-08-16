using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [SerializeField] private Transform bodyGear, coreGear;
    [SerializeField] private Transform rightWeaponGear, leftWeaponGear;

    private float horizontalInput, verticalInput, mouseXInput, mouseYInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    Rigidbody rb;
    Transform playerTransform;
    private float rotSpeed = 200f;
    private float currentRot;

    // 회전 제한 변수
    //private float maxBodyGearRotationX = 60f; // 최대 상하 회전 각도
    //private float maxBodyGearRotationY = 90f; // 최대 좌우 회전 각도
    private Vector3 currentBodyGearRotation;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        currentBodyGearRotation = bodyGear.localEulerAngles; // 초기 회전 값 저장
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        MouseRotate();
        UpdateGearLocation();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxisRaw("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);

        // 마우스 좌우 회전
        mouseXInput = Input.GetAxis("Mouse X");
        // 마우스 상하 회전
        mouseYInput = Input.GetAxis("Mouse Y");
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
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

        //mouseXInput = Math.Clamp(mouseXInput, -maxBodyGearRotationX, maxBodyGearRotationX);
        //mouseYInput = Math.Clamp(mouseYInput, -maxBodyGearRotationY, maxBodyGearRotationY);

        //bodyGear.Rotate(Vector3.down, -mouseXInput, Space.Self);
        //bodyGear.Rotate(Vector3.right, -mouseYInput, Space.Self);

        // 마우스 입력에 따른 회전 값 계산
        //float newRotationX = currentBodyGearRotation.x - (rotSpeed * mouseYInput * Time.deltaTime);
        //float newRotationY = currentBodyGearRotation.y + (rotSpeed * mouseXInput * Time.deltaTime);

        // 회전 각도 제한
        //newRotationX = Mathf.Clamp(newRotationX, -maxBodyGearRotationX, maxBodyGearRotationX);
        //newRotationY = Mathf.Clamp(newRotationY, -maxBodyGearRotationY, maxBodyGearRotationY);

        //currentBodyGearRotation = new Vector3(newRotationX, newRotationY, 0); // Z축 회전은 필요에 따라 조정

        // 회전 적용
        //bodyGear.localRotation = Quaternion.Euler(currentBodyGearRotation);

        bodyGear.rotation = new Quaternion(Math.Clamp(bodyGear.transform.rotation.x * mouseXInput * Time.deltaTime * rotSpeed, -30f, 50f),
            0f,Math.Clamp(bodyGear.transform.rotation.z * mouseXInput * Time.deltaTime * rotSpeed, -60f, 60f), 0f);
        //bodyGear.Rotate(Vector3.left * rotSpeed * mouseYInput);
    }
}