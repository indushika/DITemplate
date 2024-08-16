using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeBuildingData
{
    public NativeArray<Int32> occupiedNPCIds;
    public float3 gridPosition;
    public BuildingTypeId buildingType;
    public Int32 level;

    public NativeBuildingData(BuildingData instance)
    {
        occupiedNPCIds = new NativeArray<Int32>(instance.occupiedNPCIds.Count, Allocator.Persistent);

        for (int i = 0; i < instance.occupiedNPCIds.Count; i++)
            occupiedNPCIds[i] = instance.occupiedNPCIds[i];

        gridPosition = new float3(instance.gridPosition.x, instance.gridPosition.y, instance.gridPosition.z);

        buildingType = instance.buildingType;

        level = instance.level;

    }

    public void Dispose()
    {
        if (occupiedNPCIds.IsCreated)
            occupiedNPCIds.Dispose();
    }
}
