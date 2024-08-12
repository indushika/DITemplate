using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MF Data
public class InventoryData
{
    private Dictionary<ResourceTypeId, int> resourceAmountById = default;

    public Dictionary<ResourceTypeId, int> ResourceAmountById { get => resourceAmountById; }

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
