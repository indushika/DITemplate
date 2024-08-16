using MonsterFactory.Services.DataManagement;
using System.Collections.Generic;

[AutoLoadDbObjects(uniqueId: "BuildingReadOnlyData")]
[GenerateNativeData]
public class BuildingReadOnlyData : MFData
{
    public IReadOnlyDictionary<BuildingTypeId, BuildingTypeData> BuildingTypeDataById;
}

