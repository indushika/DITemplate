public class BuildingGenerator
{
    #region API
    public BuildingData GetNewBuildingData(BuildingTypeId buildingType)
    {
        return new BuildingData(buildingType);
    }
    #endregion

}