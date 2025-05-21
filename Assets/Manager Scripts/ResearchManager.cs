using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public static ResearchManager instance;
    public List<ResearchNode> allNodes;
    public int currentExp;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public bool CanUnlock(ResearchNode node)
    {
        if (node.isUnlocked || currentExp < node.experienceCost) return false;

        foreach (var pre in node.prerequisites)
        {
            if (!pre.isUnlocked) return false;
        }

        return true;
    }

    public bool Unlock(ResearchNode node)
    {
        if (!CanUnlock(node)) return false;

        currentExp -= node.experienceCost;
        node.isUnlocked = true;
        return true;
    }
}
