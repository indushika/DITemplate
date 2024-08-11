using System.Collections.Generic;

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
}