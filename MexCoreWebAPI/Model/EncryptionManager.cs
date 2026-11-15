using System;
using System.Security.Cryptography;
using System.Text;

namespace MexCoreWebAPI.Model
{
    public class EncryptionManager
    {
        private byte[] Key = { 89, 85, 45, 236, 140, 228, 180, 79, 209, 164, 211, 131, 28, 7, 110, 73, 140, 235, 118, 52, 225, 46, 202, 118 };
        private byte[] IV = { 161, 200, 187, 221, 66, 92, 119, 227 };
        public string Encrypt(string inputString)
        {
            //Step #4
            byte[] buffer = Encoding.ASCII.GetBytes(inputString);

            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {
                Key = Key,
                IV = IV
            };
            //Step #4,5,6
            ICryptoTransform ITransform = tripleDes.CreateEncryptor();
            return Convert.ToBase64String(ITransform.TransformFinalBlock(buffer, 0, buffer.Length));
        }
        public string Decrypt(string inputString)
        {
            byte[] buffer = Convert.FromBase64String(inputString);
            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider()
            {
                Key = Key,
                IV = IV
            };
            ICryptoTransform ITransform = tripleDes.CreateDecryptor();
            return Encoding.ASCII.GetString(ITransform.TransformFinalBlock(buffer, 0, buffer.Length));
        }
    }
}
