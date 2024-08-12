using Cysharp.Threading.Tasks;
using MonsterFactory.Services;
using MonsterFactory.Services.DataManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public interface IBuildingManager
{
    bool TryCreateNewBuilding(BuildingTypeId buildingType, out BuildingData buildingData);
    bool AreResourcesAvailableForBuild(BuildingTypeId buildingType);
}
public class BuildingManager : IMFService, IBuildingManager
{
    private ReadOnlyGameData readOnlyGameData;
    private RuntimeGameData runtimeGameData;
    private readonly MFRuntimeDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider;

    private BuildingGenerator generator;

    private Dictionary<int, BuildingData> buildingDataByInstanceId;
    private Dictionary<BuildingTypeId, BuildingTypeData> buildingTypeDataById;

    [Inject]
    public BuildingManager(MFRuntimeDataInstanceProvider<RuntimeGameData> runtimeDataInstanceProvider, ReadOnlyGameData readOnlyGameData)
    {
        this.runtimeDataInstanceProvider = runtimeDataInstanceProvider;

        this.readOnlyGameData = readOnlyGameData;

        runtimeGameData = runtimeDataInstanceProvider.DataInstance;

        Initialize();
    }

    #region API
    public UniTask[] GetInitializeTasks()
    {
        return default;
    }

    public bool TryCreateNewBuilding(BuildingTypeId buildingType, out BuildingData buildingData)
    {
        if (AreResourcesAvailableForBuild(buildingType))
        {
            buildingData = generator.GetNewBuildingData(buildingType);

            var instanceId = InstanceIdProvider.GetInstanceId(buildingDataByInstanceId.Keys.ToList());

            buildingDataByInstanceId.Add(instanceId, buildingData);
            runtimeGameData.BuildingData.Add(buildingData);

            if (!buildingTypeDataById.TryGetValue(buildingType, out BuildingTypeData typeData))
            {
                if (readOnlyGameData.BuildingTypeDataById.TryGetValue(buildingType, out typeData))
                {
                    buildingTypeDataById.TryAdd(buildingType, typeData);
                }
            }

            return true;
        }

        buildingData = null;
        return false;
    }

    public bool AreResourcesAvailableForBuild(BuildingTypeId buildingType)
    {
        var inventoryData = runtimeGameData.InventoryData;

        if (inventoryData == null)
        {
            throw new System.NullReferenceException("BuildingManager: AreResourcesAvailableForBuild: Inventory Data is not found.");
        }

        if (!buildingTypeDataById.TryGetValue(buildingType, out BuildingTypeData data))
        {
            if (!readOnlyGameData.BuildingTypeDataById.TryGetValue(buildingType, out data))
            {
                throw new System.NullReferenceException($"BuildingManager: AreResourcesAvailableForBuild: " +
                    $"Building Type Data not found for Building Type : {buildingType}.");
            }
        }

        var constructionResources = data.ConstructionResourceAmountById;

        foreach (var resource in constructionResources)
        {
            if (!inventoryData.TryGetResourceAmount(resource.Key, out int amount) || amount < resource.Value)
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    #region Implementation
    private void Initialize()
    {
        generator = new BuildingGenerator();
        buildingDataByInstanceId = new Dictionary<int, BuildingData>();

        var buildingData = runtimeGameData.BuildingData;

        if (buildingData == null)
        {
            runtimeGameData.BuildingData = new List<BuildingData>();
            return;
        }

        foreach (var data in buildingData)
        {
            var instanceId = InstanceIdProvider.GetInstanceId(buildingDataByInstanceId.Keys.ToList());
            buildingDataByInstanceId.Add(instanceId, data);
        }
    }


    #endregion

    #region Notes: Remove Later

    //Check if there's building data from previous sessions and generate them 

    //Check if the building can be created; resources it's locked by etc 

    //check and create building

    //Create Building by Building Type Id, this returns a new instance of this building type 
    //once a building is created the instance is returned, this is saved in the Building Manager cache, List<Buildings> 

    //Building Generator: generates the data for the building 
    //Update Grid Manager to 

    //level up a building 
    #endregion

}
