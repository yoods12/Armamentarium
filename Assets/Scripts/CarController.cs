using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
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

    private float horizontalInput, verticalInput, mouseInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    Rigidbody rb;
    Transform playerTransform;
    private float rotSpeed = 200f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;

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
        mouseInput = Input.GetAxis("Mouse X");
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
        bodyGear.Rotate(Vector3.up * rotSpeed * mouseInput * Time.deltaTime);

    }
}