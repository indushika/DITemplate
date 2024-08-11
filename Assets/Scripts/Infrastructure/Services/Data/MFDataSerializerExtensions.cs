using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace MonsterFactory.Services.DataManagement
{
    public static class MFDataSerializerExtensions
    {
        static bool _serializerRegistered = false;

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

        public static MFDataObject GetDataAttribute<T>(out string name)
        {
            var type = typeof(T);
            name = type.Name;
            object[] attributes = type.GetCustomAttributes(typeof(MFDataObject), true);
            foreach (var attribute in attributes)
            {
                if (attribute is MFDataObject dataObjectAttribute)
                {
                    return dataObjectAttribute;
                }
            }

            return null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (!_serializerRegistered)
            {
                StaticCompositeResolver.Instance.Register(
                    GeneratedResolver.Instance,
                    StandardResolver.Instance
                );

                var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

                MessagePackSerializer.DefaultOptions = option;
                _serializerRegistered = true;
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