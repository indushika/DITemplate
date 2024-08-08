using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace MonsterFactory.Services.DataManagement
{
    public static class MFDataExtensions
    {
        static bool serializerRegistered = false;

        public static MFData ExtractDataObjectOfType<T>(this DataChunkMap dataChunk) where T : MFData
        {
            return dataChunk.DataBlob != null
                ? MessagePackSerializer.Deserialize<MFData>(dataChunk.DataBlob)
                : default;
        }

        public static byte[] SerializeDataToBytes<T>(this T data) where T : MFData
        {
            return MessagePackSerializer.Serialize<MFData>(data);
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