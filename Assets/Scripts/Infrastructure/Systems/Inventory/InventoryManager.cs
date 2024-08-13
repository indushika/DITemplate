using Cysharp.Threading.Tasks;
using MonsterFactory.Services;
using MonsterFactory.Services.DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public interface IInventoryManager
{
    void AddResource(ResourceTypeId resourceTypeId, int amount);
    void RemoveResource(ResourceTypeId resourceTypeId);
    void RemoveResourceAmount(ResourceTypeId resourceTypeId, int amount);
    bool TryGetResourcesWithAmount<T>(out Dictionary<T, int> resources) where T : ResourceTypeData;
}
public class InventoryManager : IMFService, IInventoryManager
{
    private RuntimeGameData runtimeGameData;
    private ReadOnlyGameData readOnlyGameData;
    private readonly MFLocallyStoredDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider;


    private InventoryData inventoryData;
    private Dictionary<ResourceTypeId, ResourceTypeData> resourceTypeDataById;

    [Inject]
    public InventoryManager(MFLocallyStoredDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider, ReadOnlyGameData readOnlyGameData)
    {
        this.runtimeDataInstanceProvider = runtimeDataInstanceProvider;

        this.readOnlyGameData = readOnlyGameData;

        runtimeGameData = runtimeDataInstanceProvider.DataInstance;

        resourceTypeDataById = default;

        if (runtimeGameData == null || readOnlyGameData == null)
        {
            throw new NullReferenceException("InventoryManager: Game Data is missing.");
        }

        inventoryData = runtimeGameData.InventoryData;

        Initialize();
    }

    #region API
    public void AddResource(ResourceTypeId resourceTypeId, int amount)
    {
        inventoryData.AddResource(resourceTypeId, amount);

        if (!resourceTypeDataById.ContainsKey(resourceTypeId))
        {
            if (readOnlyGameData.ResourceTypeDataById.TryGetValue(resourceTypeId, out ResourceTypeData data))
            {
                resourceTypeDataById.Add(resourceTypeId, data);
            }
        }
    }

    public void RemoveResource(ResourceTypeId resourceTypeId)
    {
        inventoryData.RemoveResource(resourceTypeId);

        if (resourceTypeDataById.ContainsKey(resourceTypeId))
        {
            resourceTypeDataById.Remove(resourceTypeId);
        }
    }

    public bool TryGetResourcesWithAmount<T>(out Dictionary<T, int> resources) where T : ResourceTypeData 
    {
        resources = new Dictionary<T, int>();
        bool isResourceAvailable = false;

        var resourceAmountById = inventoryData.ResourceAmountById;

        foreach (var item in resourceAmountById)
        {
            if (resourceTypeDataById.TryGetValue(item.Key, out ResourceTypeData data))
            {
                if (data is T consumable)
                {
                    resources.Add(consumable, item.Value);
                    isResourceAvailable = true;
                }
            }
        }

        return isResourceAvailable;
    }

    public void RemoveResourceAmount(ResourceTypeId resourceTypeId, int amount)
    {

    }

    public UniTask[] GetInitializeTasks()
    {
        return default;
    }
    #endregion

    #region Implementation
    private void Initialize()
    {
        if (inventoryData == null)
        {
            inventoryData = new InventoryData();
        }

        var resourceAmountById = inventoryData.ResourceAmountById;

        var resourceDataById = readOnlyGameData.ResourceTypeDataById;

        foreach (var item in resourceAmountById)
        {
            if (resourceDataById.TryGetValue(item.Key, out ResourceTypeData data))
            {
                resourceTypeDataById.Add(item.Key, data);
            }
        }
    }

    #endregion

}
