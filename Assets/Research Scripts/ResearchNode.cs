using UnityEngine;

[CreateAssetMenu(fileName = "NewResearchNode", menuName = "Research/Node")]
public class ResearchNode : ScriptableObject
{
    public string nodeName;
    public GameObject modelPrefab;
    public Sprite icon;
    public int experienceCost;
    public ResearchNode[] prerequisites;
    public bool isUnlocked;
}
