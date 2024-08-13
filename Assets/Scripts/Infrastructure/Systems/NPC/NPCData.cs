using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct NPCData 
{
    private readonly int npcID;
    private readonly string name;

    private readonly Dictionary<NPCStatType, int> baseValueByStat;
    private readonly List<NPCAttributeType> attributeTypes;

    private RuntimeNPCData runtimeData;

    public int NpcID { get => npcID; }
    public string Name { get => name;}
    public List<NPCAttributeType> AttributesList => attributeTypes;

    public Dictionary<NPCStatType, int> BaseValueByStat => baseValueByStat;

    public RuntimeNPCData RuntimeData { get => runtimeData; }

    public NPCData(int npcID, string name, List<NPCAttributeType> attributeTypes,
       Dictionary<NPCStatType, int> baseValueByStat, RuntimeNPCData runtimeData)
    {
        this.npcID = npcID;
        this.name = name;
        this.baseValueByStat = baseValueByStat; 
        this.attributeTypes = attributeTypes;
        this.runtimeData = runtimeData;
    }


    //Runtime Data -> to be used by other systems if necessary 

    //set runtime stat values if the NPC already existed (NPC Manager will handle setting) 
    //last known position, other runtime data needed; equipment, etc.

}
