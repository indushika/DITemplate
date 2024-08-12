using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AssignmentTypeData 
{
    private Dictionary<NPCStatType, float> preferredStatWeightsByType;
    public Dictionary<NPCStatType, float> PreferredStatWeightsByType { get => preferredStatWeightsByType; }

    public AssignmentTypeData(Dictionary<NPCStatType,float> preferredStatWeightsByType)
    {
        this.preferredStatWeightsByType = preferredStatWeightsByType;
    }

}

public enum AssignmentTypeId
{

}
