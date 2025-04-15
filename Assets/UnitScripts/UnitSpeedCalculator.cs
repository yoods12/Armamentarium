using UnityEngine;

public class UnitSpeedCalculator : MonoBehaviour
{
    public float CalculateSpeed()
    {
        Wheel[] wheels = GetComponentsInChildren<Wheel>();
        if(wheels.Length == 0)
        {
            Debug.LogWarning("No wheels found on the unit."); // 유닛에 바퀴 없으면
            return 0f;
        }
        float totalSpeed = 0f;
        foreach(Wheel wheel in wheels)
        {
            totalSpeed += wheel.speed;
        }
        return totalSpeed / wheels.Length;  // 유닛이 가진 바퀴의 평균 속도
    }
}
