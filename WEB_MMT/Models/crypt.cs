using System.Security.Cryptography;
using System.Text;

namespace WEB_MMT.Models
{
    public class crypt
    {
        private const string key = "doanmangmaytinhquizzapplication";

        public static string Encrypt(string input, bool useHashing)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateAesKey(); // Sử dụng khóa mới tạo
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(input);
                        }
                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        private static byte[] GenerateAesKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 128; // 128-bit key size
                aes.GenerateKey();
                return aes.Key;
            }
        }
    }
}
