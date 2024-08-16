using System;
using System.Collections.Generic;
using UnityEngine;

//MF Data
[GenerateNativeData]
public class RuntimeNPCData
{
    public Dictionary<NPCStatType, int> runtimeValueByStat;
    public Vector3 gridPosition;
    public int level;

    public Dictionary<NPCStatType, int> RuntimeValueByStat { get => runtimeValueByStat; set => runtimeValueByStat = value; }
    public Vector3 GridPosition { get => gridPosition; set => gridPosition = value; }
    public int Level { get => level; set => level = value; }

    public RuntimeNPCData()
    {
    }

}