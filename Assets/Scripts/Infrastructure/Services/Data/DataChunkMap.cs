using SQLite;

namespace MonsterFactory.Services.DataManagement
{
    public class DataChunkMap
    {
        [PrimaryKey, Unique, MaxLength(64)] public string Id { get; set; }
        
        public byte[] DataBlob { get; set; }
    }
}