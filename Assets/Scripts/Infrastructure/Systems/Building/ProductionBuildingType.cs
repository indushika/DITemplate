using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuildingType : BuildingTypeData
{
    private Dictionary<ResourceTypeId, int> productionResourceAmountById;

    public ProductionBuildingType(string name, int constructionTime, 
        Dictionary<ResourceTypeId, int> constructionResourceAmountById, int npcCapacity, (int, int) gridSize, 
        Dictionary<ResourceTypeId, int> productionResourceAmountById) :
        base(name, constructionTime, constructionResourceAmountById, npcCapacity, gridSize)
    {
        this.productionResourceAmountById = productionResourceAmountById;
    }
}
