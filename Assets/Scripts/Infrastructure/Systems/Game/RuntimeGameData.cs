using MessagePack;
using MonsterFactory.Services.DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MessagePackObject]
[MFDataObject("RuntimeGameData", true, true)]
public class RuntimeGameData : IMFData
{
    [Key(1)]
    public List<int> npcIds = default;
    [Key(2)]
    public Dictionary<int, RuntimeNPCData> runtimeNPCDataById;
    [Key(3)]
    public Dictionary<int, Assignment> assignmentsById = default;
    [Key(4)]
    public InventoryData inventoryData;
    [Key(5)]
    public List<BuildingData> buildingData = default;

    [IgnoreMember]
    public List<int> NPCIds { get => npcIds; }
    [IgnoreMember]
    public Dictionary<int, Assignment> AssignmentsById { get => assignmentsById; }
    [IgnoreMember]
    public InventoryData InventoryData { get => inventoryData; set => inventoryData = value; }
    [IgnoreMember]
    public List<BuildingData> BuildingData { get => buildingData; set => buildingData = value; }
    [IgnoreMember]
    public Dictionary<int, RuntimeNPCData> RuntimeNPCDataById { get => runtimeNPCDataById; }

    public void AddNewNPCId(int npcId)
    {
        npcIds.Add(npcId);
    }
    public void UpdateAssignmentsByIDCollection(Dictionary<int, Assignment> assignmentsById)
    {
        this.assignmentsById.Clear();
        this.assignmentsById = assignmentsById;
    }
    public void AddNewNPC(int npcId, RuntimeNPCData runtimeData)
    {
        runtimeNPCDataById.TryAdd(npcId, runtimeData);
    }
}
