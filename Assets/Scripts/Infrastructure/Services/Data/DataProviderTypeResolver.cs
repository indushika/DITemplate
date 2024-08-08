using System;
using System.Security.Cryptography;
using System.Text;

namespace MonsterFactory.Services.DataManagement
{
    public static class DataProviderTypeResolver
    {
        private static SHA256Managed _sha256Managed;

        public static void ResolveTypeInfo(ref string uniqueId, ref bool autoFetch, ref string typeCode, ref MFDataObject dataObject)
        {
            uniqueId = dataObject.UniqueId;
            autoFetch = dataObject.AutoFetch;
            if (!string.IsNullOrEmpty(uniqueId))
            {
                _sha256Managed ??= new SHA256Managed();
                byte[] hashedBytes = _sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(uniqueId));
                string hashedString = Convert.ToBase64String(hashedBytes);
                typeCode = hashedString.Length > 64 ? hashedString.Substring(0, 64) : hashedString;
            }
        }
    }
}