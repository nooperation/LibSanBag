using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.ResourceUtils
{
    public class LibUserPreferences
    {
        public enum APIResult
        {
            kResult_GeneralSuccess = 0,
            kResult_GeneralFailure = -1,
            kResult_BufferTooSmall = -2
        };

        [DllImport("LibUserPreferences.dll")]
        private static extern APIResult DecryptData(
            byte[] encrypted_buffer,
            ulong encrypted_buffer_size,
            string machine_guid,
            string salt_string,
            byte[] out_decrypted_buffer,
            ref ulong out_decrypted_buffer_size
        );

        [DllImport("LibUserPreferences.dll")]
        private static extern APIResult EncryptData(
            byte[] plaintext_data,
            ulong plaintext_data_size,
            string machine_guid,
            string salt_string,
            byte[] out_encrypted_buffer,
            ref ulong out_encrypted_buffer_size
        );

        public static string Read(string path, string guid, string salt)
        {
            using (var fileStream = File.OpenRead(path))
            {
                return Read(fileStream, guid, salt);
            }
        }

        public static string Read(Stream byteStream, string guid, string salt)
        {
            var encryptedBytes = new byte[byteStream.Length];
            byteStream.Read(encryptedBytes, 0, (int)byteStream.Length);

            ulong decryptedBytesSize = 0;
            var result = DecryptData(encryptedBytes, (ulong)encryptedBytes.Length, guid, salt, null, ref decryptedBytesSize);
            if (result != APIResult.kResult_BufferTooSmall)
            {
                throw new Exception("Failed to get decrypted buffer length");
            }

            var decryptedBytes = new byte[decryptedBytesSize];
            result = DecryptData(encryptedBytes, (ulong)encryptedBytes.Length, guid, salt, decryptedBytes, ref decryptedBytesSize);
            if (result != APIResult.kResult_GeneralSuccess)
            {
                throw new Exception("Failed to get decrypted buffer");
            }

            var decryptedString = Encoding.ASCII.GetString(decryptedBytes);
            return decryptedString;
        }

        public static void Write(Stream outStream, string data, string guid, string salt)
        {
            var decryptedBytes = Encoding.ASCII.GetBytes(data);

            ulong encryptedBytesSize = 0;
            var result = EncryptData(decryptedBytes, (ulong)decryptedBytes.Length, guid, salt, null, ref encryptedBytesSize);
            if (result != APIResult.kResult_BufferTooSmall)
            {
                throw new Exception("Failed to get encrypted buffer length");
            }

            var encryptedBytes = new byte[encryptedBytesSize];
            result = EncryptData(decryptedBytes, (ulong)decryptedBytes.Length, guid, salt, encryptedBytes, ref encryptedBytesSize);
            if (result != APIResult.kResult_GeneralSuccess)
            {
                throw new Exception("Failed to get encrypted buffer");
            }

            outStream.Write(encryptedBytes, 0, encryptedBytes.Length);
        }
    }
}
