using System;
using System.Collections.Generic;
using UnityEngine;

//MF Data
[Serializable]
public class RuntimeNPCData
{
    private Dictionary<NPCStatType, int> runtimeValueByStat;
    private Vector3 gridPosition;
    private int level;
    public Dictionary<NPCStatType, int> RuntimeValueByStat { get => runtimeValueByStat; set => runtimeValueByStat = value; }
    public Vector3 GridPosition { get => gridPosition; set => gridPosition = value; }
    public int Level { get => level; set => level = value; }
}