namespace WOT_Common;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class CryptoFunctions
{
    private const string KEY = "b14ca5a4e4WOT133bbce2ewot15a1916";
    private static readonly Encoding Encoder = Encoding.UTF8;

    public static async Task<string> CreateMD5(string _input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(_input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return await Task.FromResult(Convert.ToHexString(hashBytes)); 
        }
    }

    public static async Task<string> Encryption(string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(KEY);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    array = memoryStream.ToArray();
                }
            }
        }
        return await Task.FromResult(Convert.ToBase64String(array));
    }

    public static async Task<string> Decryption(string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(KEY);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return await Task.FromResult(streamReader.ReadToEnd());
                    }
                }
            }
        }
    }
}

