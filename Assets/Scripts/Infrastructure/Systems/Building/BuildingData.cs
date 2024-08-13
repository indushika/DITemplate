
using System;
using System.Collections.Generic;
using System.Numerics;

//MF Data
[Serializable]
public class BuildingData
{
    private BuildingTypeId buildingType;
    private int level;
    private List<int> occupiedNPCIds;
    private Vector3 gridPosition;

    public BuildingTypeId BuildingType { get => buildingType; }
    public int Level { get => level; set => level = value; }
    public List<int> OccupiedNPCIds { get => occupiedNPCIds; set => occupiedNPCIds = value; }
    public Vector3 GridPosition { get => gridPosition; set => gridPosition = value; }

    public BuildingData(BuildingTypeId buildingType)
    {
        this.buildingType = buildingType;
    }

}