
using System.Collections.Generic;
using UnityEngine;

//MF Data
[GenerateNativeData]
public class BuildingData
{
    public BuildingTypeId buildingType;
    public int level;
    public List<int> occupiedNPCIds;
    public Vector3 gridPosition;

    public BuildingTypeId BuildingType { get => buildingType; }
    public int Level { get => level; set => level = value; }
    public List<int> OccupiedNPCIds { get => occupiedNPCIds; set => occupiedNPCIds = value; }
    public Vector3 GridPosition { get => gridPosition; set => gridPosition = value; }

    public BuildingData(BuildingTypeId buildingType)
    {
        this.buildingType = buildingType;
    }

}