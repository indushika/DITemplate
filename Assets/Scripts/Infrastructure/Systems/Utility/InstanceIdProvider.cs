using System.Collections.Generic;
using System;

public static class InstanceIdProvider
{
    private static readonly Random random = new Random();

    public static int GetInstanceId(List<int> ids)
    {
        if (ids == null)
        {
            throw new ArgumentNullException("InstanceIdProvider: Pre-existing Ids not initialized");
        }

        int id;

        do
        {
            id = random.Next(10000);
        }
        while (ids.Contains(id));


        return id;
    }
}