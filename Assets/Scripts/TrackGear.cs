using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TrackGear : MonoBehaviour
{
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider FLWheelCol, FRWheelCol;
    [SerializeField] private WheelCollider BLWheelCol, BRWheelCol;

    private float h, v;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }
    void Update()
    {

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
        UpdateSingleWheel(FLWheelCol);
        UpdateSingleWheel(FRWheelCol);
        UpdateSingleWheel(BRWheelCol);
        UpdateSingleWheel(BLWheelCol);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
    }

}
