using Unity.Collections;
using Unity.Mathematics;
using System;

public struct NativeAssignment
{
    public NativeHashMap<int,Single> preferredStatWeightsByType;
    public Int32 id;
    public Single progress;

    public NativeAssignment(Assignment instance)
    {
        preferredStatWeightsByType = new NativeHashMap<int,Single>(instance.preferredStatWeightsByType.Count, Allocator.Persistent);

        foreach (var element in instance.preferredStatWeightsByType)
            preferredStatWeightsByType.Add((int)element.Key, element.Value);

        id = instance.id;

        progress = instance.progress;

    }

    public void Dispose()
    {
        if (preferredStatWeightsByType.IsCreated)
            preferredStatWeightsByType.Dispose();
    }
}
