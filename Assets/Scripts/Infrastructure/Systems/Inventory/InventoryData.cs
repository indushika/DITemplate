using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MF Data
[GenerateNativeData]
public class InventoryData
{
    public Dictionary<ResourceTypeId, int> resourceAmountById;
    public Dictionary<ResourceTypeId, int> ResourceAmountById { get => resourceAmountById; }

    public InventoryData()
    {
        resourceAmountById = default;
    }


    #region API
    public void AddResource(ResourceTypeId resourceTypeId, int amount)
    {
        if (resourceAmountById.ContainsKey(resourceTypeId)) 
        {
            resourceAmountById[resourceTypeId] = amount; 
        }
        else
        {
            resourceAmountById.Add(resourceTypeId, amount);
        }
    }

    public bool TryGetResourceAmount(ResourceTypeId resourceTypeId, out int amount)
    {
        if (resourceAmountById.TryGetValue(resourceTypeId, out amount))
        {
            return true;
        }

        return false;
    }

    public void RemoveResource(ResourceTypeId resourceTypeId)
    {
        if (resourceAmountById.ContainsKey(resourceTypeId))
        {
            resourceAmountById.Remove(resourceTypeId);
        }
    }
    #endregion

}
