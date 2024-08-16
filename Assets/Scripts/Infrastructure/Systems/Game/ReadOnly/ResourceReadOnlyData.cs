using MonsterFactory.Services.DataManagement;
using System.Collections.Generic;

[AutoLoadDbObjects(uniqueId: "ResourceReadOnlyData")]
[GenerateNativeData]
public class ResourceReadOnlyData : MFData
{
    public IReadOnlyDictionary<ResourceTypeId, ResourceTypeData> ResourceTypeDataById;
}

