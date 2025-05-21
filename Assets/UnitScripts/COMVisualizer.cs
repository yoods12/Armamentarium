using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UnitPhysicsBaker))]
public class COMVisualizer : MonoBehaviour
{
    [Header("마커 설정")]
    public float markerSize = 0.2f;
    public Color markerColor = Color.red;
    public float markerHeightOffset = 0.1f;

    private Transform marker;
    private Material markerMat;

    void Awake()
    {
        // 구체 생성
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker = go.transform;
        marker.name = "COM_Marker";
        // collider 제거
        Destroy(go.GetComponent<Collider>());

        // 부모에 종속
        marker.SetParent(transform, false);
        // 크기 설정
        marker.localScale = Vector3.one * markerSize;

        // Unlit/Color 셰이더로 매터리얼 생성
        markerMat = new Material(Shader.Find("Unlit/Color"));
        markerMat.color = markerColor;
        // 항상 앞에 렌더
        markerMat.SetInt("_ZWrite", 0);
        markerMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        markerMat.renderQueue = 5000;

        var rend = go.GetComponent<MeshRenderer>();
        rend.material = markerMat;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rend.receiveShadows = false;

        // 초기 로컬 위치는 0
        marker.localPosition = Vector3.up * markerHeightOffset;
    }

    void Update()
    {
        var blocks = GetComponentsInChildren<UnitPhysicsBaker.BlockData>();
        if (blocks.Length == 0) return;

        // 질량중심 로컬로 계산
        float totalMass = 0f;
        Vector3 weightedSum = Vector3.zero;
        foreach (var bd in blocks)
        {
            float m = bd.mass;
            totalMass += m;
            Vector3 localPos = transform.InverseTransformPoint(bd.transform.position);
            weightedSum += localPos * m;
        }
        Vector3 localCOM = weightedSum / totalMass;

        // 로컬 위치 + 높이 오프셋
        marker.localPosition = localCOM + Vector3.up * markerHeightOffset;
    }
}
    