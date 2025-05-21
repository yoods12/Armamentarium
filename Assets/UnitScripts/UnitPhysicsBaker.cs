using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitPhysicsBaker : MonoBehaviour
{
    public class BlockData : MonoBehaviour
    {
        public float mass = 1f;
    }

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void BakerMassAndCOM()
    {
        var blocks = GetComponentsInChildren<BlockData>();
        if (blocks.Length == 0)
        {
            Debug.LogWarning("No BlockData found in children.");
            return;
        }

        float totalMass = 0f;
        Vector3 weightedLocalPosSum = Vector3.zero;

        foreach(var bd in blocks)
        {
            float m = bd.mass;
            totalMass += bd.mass;
           
            Vector3 localPos = transform.InverseTransformPoint(bd.transform.position);
            weightedLocalPosSum += localPos * m;
        }

        rb.mass = totalMass;

        Vector3 localCOM = weightedLocalPosSum / totalMass;
        rb.centerOfMass = localCOM;
    }
}
