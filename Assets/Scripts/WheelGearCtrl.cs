using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wheel
{
    public WheelCollider FR;
    public WheelCollider FL;
    public WheelCollider BR;
    public WheelCollider BL;

    public MeshRenderer F;
    public MeshRenderer B;
}

public class WheelGearCtrl : MonoBehaviour
{
    public Wheel Wheel;


    void Start()
    {
    }
    void FixedUpdate()
    {

    }
    void Update()
    {

    }

    void UpdateWheel(WheelCollider coll, MeshRenderer mesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorld
    }
}


