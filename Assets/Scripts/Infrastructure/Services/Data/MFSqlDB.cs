using SQLite;
using Cysharp.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using SQLiteNetExtensions.Attributes;
using UnityEngine;


namespace MonsterFactory.Services.DataManagement
{
    public interface IMFLocalDBService
    {
        public UniTask Initialize();
        public UniTask<DataChunkMap> GetDataChunkById(string dataChunkId);

        public UniTask<int> WriteDataChunkToId(string dataChunkId, byte[] dataBlob);
        public UniTask<DataChunkMap> GetChunkUniqueDataFromKey(string key);

        public UniTask<int> AddNewDataInstance(DataChunkMap data);

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

        private UniTask InitializeGameTables()
        {
            return dbConnection.CreateTablesAsync(CreateFlags.None, typeof(DataChunkMap), typeof(DataChunkMap));
        }


        public UniTask Initialize()
        {
            dbConnection = new SQLiteAsyncConnection(dbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
            return InitializeGameTables();
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

        public async void AddDataChunkMap(string typeString)
        {
            await dbConnection.InsertOrReplaceAsync(new DataChunkMap()
            {
                Id = typeString
            });
        }
    }

    
    public class DataChunkMap
    {
        [PrimaryKey, Unique, MaxLength(64)] public string Id { get; set; }
        
        public byte[] DataBlob { get; set; }
    }

    public static class MFDataExtensions
    {
        static bool serializerRegistered = false;

        public static IMFData ExtractDataObjectOfType<T>(this DataChunkMap dataChunk) where T : IMFData
        {
            return dataChunk.DataBlob != null
                ? MessagePackSerializer.Deserialize<IMFData>(dataChunk.DataBlob)
                : default;
        }

        public static byte[] SerializeDataToBytes<T>(this T data) where T : IMFData
        {
            return MessagePackSerializer.Serialize<IMFData>(data);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (!serializerRegistered)
            {
                StaticCompositeResolver.Instance.Register(
                    MessagePack.Resolvers.GeneratedResolver.Instance,
                    MessagePack.Resolvers.StandardResolver.Instance
                );

                var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

                MessagePackSerializer.DefaultOptions = option;
                serializerRegistered = true;
            }
        }
        
#if UNITY_EDITOR


        [UnityEditor.InitializeOnLoadMethod]
        static void EditorInitialize()
        {
            Initialize();
        }

#endif
    }
    
}