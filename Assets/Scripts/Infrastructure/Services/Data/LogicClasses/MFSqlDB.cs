using System.Collections.Generic;
using SQLite;
using Cysharp.Threading.Tasks;


namespace MonsterFactory.Services.DataManagement
{
    public interface IMFSerializedDBConnection
    {
        public UniTask Initialize();
        public UniTask<DataChunkMap> GetDataChunkById(string dataChunkId);

        public UniTask<int> WriteSingleDataChunkToId(string dataChunkId, byte[] dataBlob);
        
        public UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key);

        public UniTask<List<DataChunkMap>> GetAllDataFromTable();

        public UniTask<int> AddNewDataInstance(DataChunkMap data);

        public UniTask CloseDbConnection();
    }

    public class MFSqlDBConnection : IMFSerializedDBConnection
    {
        private readonly string dbPath;
        private SQLiteAsyncConnection dbConnection;

        public MFSqlDBConnection(string dbFilePath)
        {
            dbPath = dbFilePath;
        }

        public UniTask Initialize()
        {
            if (dbConnection != null)
            {
                return default;
            }
            dbConnection = new SQLiteAsyncConnection(dbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
            return CreateDataChunkTable();
        }

        public async UniTask<DataChunkMap> GetDataChunkById(string dataChunkId)
        {
            return await dbConnection.GetAsync<DataChunkMap>(dataChunkId);
        }

        public UniTask<int> WriteSingleDataChunkToId(string typeCode, byte[] dataBlob)
        {
            return dbConnection.InsertOrReplaceAsync(new DataChunkMap() { DataBlob = dataBlob, Id = typeCode },
                typeof(DataChunkMap));
        }

        public async UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key)
        {
            return await dbConnection.GetAsync<DataChunkMap>(key);
        }

        public async UniTask<List<DataChunkMap>> GetAllDataFromTable()
        {
            return await dbConnection.Table<DataChunkMap>().ToListAsync();
        }

        public UniTask<int> AddNewDataInstance(DataChunkMap data)
        {
            return dbConnection.InsertAsync(data, typeof(DataChunkMap));
        }

        public UniTask CloseDbConnection()
        {
            return dbConnection?.CloseAsync() ?? default;
        }
        
        private UniTask CreateDataChunkTable()
        {
            return dbConnection.CreateTablesAsync(CreateFlags.None, typeof(DataChunkMap));
        }
    }
}