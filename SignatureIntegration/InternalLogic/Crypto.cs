using SignatureIntegration.InternalLogic.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SignatureIntegration.InternalLogic
{
    internal class Crypto : ICrypto
    {
        private string[] keywords = { "ToBeOrNot", "Ophelia", "Elsinore", "Claudius", "Polonius", "Ghost", "Denmark", "Laertes", "Horatio", "Revenge" };

        public Crypto()
        { }

        public string Encode(string userId)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, keywords.Length);

            using (var aes = Aes.Create())
            {
                aes.Key = GetAesKey(userId, index);
                aes.IV = new byte[16];

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                var forencode = userId + "CHECK";

                var plainBytes = Encoding.UTF8.GetBytes(forencode);

                var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                return Convert.ToBase64String(cipherBytes);
            }
        }

        public string Decode(string userId, string cipherText)
        {
            for (int index = 0; index < keywords.Length; index++)
            {
                try
                {
                    using (var aes = Aes.Create())
                    {
                        aes.Key = GetAesKey(userId, index);
                        aes.IV = new byte[16];

                        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                        var cipherBytes = Convert.FromBase64String(cipherText);

                        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                        var decoded = Encoding.UTF8.GetString(plainBytes);

                        if (decoded.EndsWith("CHECK"))
                        {
                            return decoded.Substring(0, decoded.Length - "CHECK".Length);
                        }
                    }
                }
                catch { }
            }

            throw new Exception("The passkey was incorrect.");
        }

        private byte[] GetAesKey(string userId, int index)
        {
            string comKey = $"{keywords[index]}{userId}{DateTime.UtcNow.ToString("yyyyMMdd")}";

            using (var sha256 = SHA256.Create()) 
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(comKey));
            }
        }
    }
}
