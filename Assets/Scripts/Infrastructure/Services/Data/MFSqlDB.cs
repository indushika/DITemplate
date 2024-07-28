using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MessagePack;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions.TextBlob;
using UnityEngine;

namespace MonsterFactory.Services.DataManagement
{
    public interface IMFLocalDBService
    {
        public void Initialize();
        public UniTask<DataChunk> GetDataChunkById(int dataChunkId);
        public UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key);

        public UniTask<int> AddNewDataInstance(DataChunk data);

        public void AddDataChunkMap(string typeString, int chunkId);
    }
    
    public class MFSqlDB : IMFLocalDBService
    {
        private readonly string dbPath;
        private SQLiteAsyncConnection dbConnection;
        
        public MFSqlDB(string dbFilePath)
        {
            dbPath = dbFilePath;
        }

        private void InitializeGameTables()
        {
            dbConnection.CreateTablesAsync(CreateFlags.None, typeof(DataChunk),typeof(DataChunkMap));
        }


        public void Initialize()
        {
            dbConnection = new SQLiteAsyncConnection(dbPath,SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
            //TODO : Data from existing tables when db paths change here. 
            InitializeGameTables();
        }

        public async UniTask<DataChunk> GetDataChunkById(int dataChunkId)
        {
            return await dbConnection.GetAsync<DataChunk>(dataChunkId);
        }
        
        public async UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key)
        {
            return await dbConnection.GetAsync<DataChunkMap>(key);
        }

        public UniTask<int> AddNewDataInstance(DataChunk data)
        {
           return dbConnection.InsertAsync(data, typeof(DataChunk));
        }

        public void AddDataChunkMap(string typeString, int chunkId)
        {
            dbConnection.InsertOrReplaceAsync(new DataChunkMap()
            {
                DataChunkId = chunkId,
                Id = typeString
            });
        }
        

    }
    
    public class DataChunk
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int DataChunkId { get; set; }
        public byte[] DataBlob { get; set; }
    }
    
    public class DataChunkMap
    {
        [PrimaryKey, Unique, MaxLength(64)]
        public string Id { get; set; }
        [Indexed, ForeignKey(typeof(DataChunk))]
        public int DataChunkId { get; set; }
    }

    public static class MFDataExtensions
    {
        public static T ExtractDataObjectOfType<T>(this DataChunk dataChunk)
        {
            return dataChunk.DataBlob != null ? MessagePackSerializer.Deserialize<T>(dataChunk.DataBlob) : default;
        }
    }
}