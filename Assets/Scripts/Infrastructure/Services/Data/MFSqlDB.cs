﻿using SQLite;
using Cysharp.Threading.Tasks;
using SQLiteNetExtensions.Attributes;


namespace MonsterFactory.Services.DataManagement
{
    public interface IMFLocalDBService
    {
        public UniTask Initialize();
        public UniTask<DataChunkMap> GetDataChunkById(string dataChunkId);

        public UniTask<int> WriteDataChunkToId(string dataChunkId, byte[] dataBlob);
        public UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key);

        public UniTask<int> AddNewDataInstance(DataChunkMap data);

        public UniTask CloseDbConnection();

        public void AddDataChunkMap(string typeString);
    }

    public class MFSqlDB : IMFLocalDBService
    {
        private readonly string dbPath;
        private SQLiteAsyncConnection dbConnection;

        public MFSqlDB(string dbFilePath)
        {
            dbPath = dbFilePath;
        }

        private UniTask CreateDataChunkTable()
        {
            return dbConnection.CreateTablesAsync(CreateFlags.None, typeof(DataChunkMap));
        }


        public UniTask Initialize()
        {
            dbConnection = new SQLiteAsyncConnection(dbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
            return CreateDataChunkTable();
        }

        public async UniTask<DataChunkMap> GetDataChunkById(string dataChunkId)
        {
            return await dbConnection.GetAsync<DataChunkMap>(dataChunkId);
        }

        public UniTask<int> WriteDataChunkToId(string typeCode, byte[] dataBlob)
        {
            return dbConnection.InsertOrReplaceAsync(new DataChunkMap() { DataBlob = dataBlob, Id = typeCode },
                typeof(DataChunkMap));
        }

        public async UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key)
        {
            return await dbConnection.GetAsync<DataChunkMap>(key);
        }

        public UniTask<int> AddNewDataInstance(DataChunkMap data)
        {
            return dbConnection.InsertAsync(data, typeof(DataChunkMap));
        }

        public UniTask CloseDbConnection()
        {
            if (dbConnection == null)
            {
                return default;
            }
            return dbConnection.CloseAsync();
        }

        public async void AddDataChunkMap(string typeString)
        {
            await dbConnection.InsertOrReplaceAsync(new DataChunkMap()
            {
                Id = typeString
            });
        }
    }
}