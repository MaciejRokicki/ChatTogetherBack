using System;
using System.Security.Cryptography;
using System.Text;

namespace ChatTogether.Commons.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string salt = "ASFEsGewTR@#rewdgA3$#TRegw%4e^&$W3tERG23tEWGDSJ&YY9r575rwyhreah#@tewTGsdfh6%8e^rutYrdhBfdJG";

        public string EncryptionSHA256(string value)
        {
            if (value == string.Empty)
                return null;

            value = new StringBuilder(value)
                .Append(salt)
                .ToString();

            SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            sha256Hash.Dispose();

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public bool VerifySHA256(string valueString, string hash)
        {
            if (valueString == string.Empty)
                return false;

            if (hash == string.Empty)
                return false;

            string encryptedString = EncryptionSHA256(valueString);

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            if (stringComparer.Compare(encryptedString, hash) == 0)
                return true;

            return false;
        }
    }
}
