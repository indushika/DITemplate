using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NPCIdProvider
{
    private List<int> npcIds;
    private List<int> bufferedNPCIds;

    private const int bufferSize = 5;
    private static readonly Random random = new Random();

    //pass the NPC List here instead of persistent game data 
    public NPCIdProvider(List<int> npcIds)
    {
        this.npcIds = npcIds;
        CreateNPCIdBufferData();
    }

    #region API
    public int GetNPCId()
    {
        int id = GetNPCIdFromBuffer();

        return id;
    }
    #endregion

    #region Implementation
    private void CreateNPCIdBufferData()
    {
        bufferedNPCIds = new List<int>();
        for (int i = 0; i < bufferSize; i++)
        {
            bufferedNPCIds.Add(GetNewNPCIdToBuffer());
        }
    }
    private int GetNewNPCIdToBuffer()
    {
        if (bufferedNPCIds == null || npcIds == null) 
        {
            throw new NullReferenceException("NPCCreator: GetNPCId: NPC Id List not initialized.");
        }

        int id;

        do
        {
            id = random.Next(1000, 9999);
        }
        while (npcIds.Contains(id) || bufferedNPCIds.Contains(id));

        return id;
    }

    private int GetNPCIdFromBuffer()
    {
        int id = bufferedNPCIds.First();

        bufferedNPCIds.Remove(id);

        bufferedNPCIds.Add(GetNewNPCIdToBuffer());

        return id;
    }
    #endregion
}
