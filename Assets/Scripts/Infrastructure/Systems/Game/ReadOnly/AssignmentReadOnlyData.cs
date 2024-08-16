using MonsterFactory.Services.DataManagement;
using System.Collections.Generic;

[AutoLoadDbObjects(uniqueId: "AssignmentReadOnlyData")]
[GenerateNativeData]    
public class AssignmentReadOnlyData : MFData
{
    public IReadOnlyDictionary<AssignmentTypeId, AssignmentTypeData> AssignmentTypeDataById;
}

