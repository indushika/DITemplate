using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTypeData 
{
    private string name;
    private int constructionTime;
    private Dictionary<ResourceTypeId, int> constructionResourceAmountById;
    private int npcCapacity;
    private (int, int) gridSize;

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