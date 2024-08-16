using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GenerateNativeData]
public class BuildingTypeData 
{
    public string name;
    public int constructionTime;
    public Dictionary<ResourceTypeId, int> constructionResourceAmountById;
    public int npcCapacity;
    public (int, int) gridSize;

    public string Name { get => name; }
    public int ConstructionTime { get => constructionTime; }
    public Dictionary<ResourceTypeId, int> ConstructionResourceAmountById { get => constructionResourceAmountById; }
    public int NpcCapacity { get => npcCapacity; }
    public (int, int) GridSize { get => gridSize; }


    public BuildingTypeData(string name, int constructionTime,
        Dictionary<ResourceTypeId, int> constructionResourceAmountById, int npcCapacity, (int, int) gridSize)
    {
        this.name = name;
        this.constructionTime = constructionTime;
        this.constructionResourceAmountById = constructionResourceAmountById;
        this.npcCapacity = npcCapacity;
        this.gridSize = gridSize;
    }

}

public enum BuildingTypeId
{

}