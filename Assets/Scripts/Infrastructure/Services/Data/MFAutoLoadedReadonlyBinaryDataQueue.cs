using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MonsterFactory.Services.DataManagement
{
    public class MFReadOnlyBinaryDataQueue : Dictionary<string, byte[]>
    {
        public bool TryDeque(string id, out byte[] bytes)
        {
            if (TryGetValue(id, out bytes))
            {
                Remove(id);
                return true;
            }
            return false;
        }
    }


    public class MFReadOnlyDbDataCache : Dictionary<string, MFReadOnlyBinaryDataQueue>
    {
        public async UniTask TryQueue(string dbFileName)
        {
            try
            {
                if (ContainsKey(dbFileName))
                {
                    return;
                }
                var conn = new MFSqlDBConnection(DataManagerDirectoryHelper.StreamingDataObjectPath(dbFileName));
                await conn.Initialize();
                var list = await conn.GetAllDataFromTable();
                TryQueueData(dbFileName, list);
                await conn.CloseDbConnection();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            
        }
        public void TryQueueData(string dbFileName, List<DataChunkMap> rawData)
        {
            MFReadOnlyBinaryDataQueue dataQueue = new MFReadOnlyBinaryDataQueue();
            
            foreach (DataChunkMap variable in rawData)
            {
                dataQueue.Add(variable.Id, variable.DataBlob);
            }
            TryAdd(dbFileName, dataQueue);
        }
        
    }
}