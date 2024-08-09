using SQLite;
using Cysharp.Threading.Tasks;


namespace MonsterFactory.Services.DataManagement
{
    public interface IMFSerializedDB
    {
        public UniTask Initialize();
        public UniTask<DataChunkMap> GetDataChunkById(string dataChunkId);

        public UniTask<int> WriteSingleDataChunkToId(string dataChunkId, byte[] dataBlob);
        
        public UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key);

        public UniTask<int> AddNewDataInstance(DataChunkMap data);

        public UniTask CloseDbConnection();
    }

    public class MFSqlDB : IMFSerializedDB
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

        public UniTask<int> WriteSingleDataChunkToId(string typeCode, byte[] dataBlob)
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
    }
}