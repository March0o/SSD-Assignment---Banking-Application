using System;
using System.Security.Cryptography;
using System.Text;

public static class AesHelper
{
    private static string KeyName = "COOL-KEY-NAME-2"; // Persistent key name

    // ------------------ Encrypt ------------------
    public static (string CipherBase64, string IVBase64) Encrypt(string plaintext)
    {
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

        using Aes aes = SetupAES();
        aes.GenerateIV();  // Random 16-byte IV per encryption
        byte[] iv = aes.IV;

        Console.WriteLine($"[Encrypt] IV Length: {iv.Length} bytes");

        using ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] cipherBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

        Console.WriteLine($"[Encrypt] Plaintext Length: {plaintextBytes.Length} bytes");
        Console.WriteLine($"[Encrypt] Ciphertext Length: {cipherBytes.Length} bytes");

        string cipherBase64 = Convert.ToBase64String(cipherBytes);
        string ivBase64 = Convert.ToBase64String(iv);

        return (cipherBase64, ivBase64);
    }

    // ------------------ Decrypt ------------------
    public static string Decrypt(string cipherBase64, string ivBase64)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherBase64);
        byte[] ivBytes = Convert.FromBase64String(ivBase64);

        Console.WriteLine($"[Decrypt] IV Length: {ivBytes.Length} bytes");
        Console.WriteLine($"[Decrypt] Ciphertext Length: {cipherBytes.Length} bytes");

        using Aes aes = SetupAES();
        aes.IV = ivBytes;

        try
        {
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            Console.WriteLine($"[Decrypt] Decrypted Length: {decryptedBytes.Length} bytes");
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"[Decrypt] ERROR: {ex.Message}");
            Console.WriteLine("[Decrypt] Possible causes: wrong key, IV, corrupted ciphertext, or mode/padding mismatch.");
            throw;
        }
    }

    // ------------------ AES Setup ------------------
    private static Aes SetupAES()
    {
        CngProvider provider = CngProvider.MicrosoftSoftwareKeyStorageProvider;

        // Ensure the persistent key exists
        if (!CngKey.Exists(KeyName, provider))
        {
            Console.WriteLine("[SetupAES] Key does not exist. Creating a new persistent AES key...");
            var keyParams = new CngKeyCreationParameters
            {
                Provider = provider,
                KeyUsage = CngKeyUsages.AllUsages,
                Parameters = { new CngProperty("Length", BitConverter.GetBytes(256), CngPropertyOptions.None) }
            };
            CngKey.Create(new CngAlgorithm("AES"), KeyName, keyParams);
        }
        else
        {
            Console.WriteLine("[SetupAES] Persistent AES key exists. Using existing key.");
        }


        Aes aes = new AesCng(KeyName, provider);
        aes.KeySize = 256;                  //Set Cipher Key Size. This Is An AES Only Setting - Possible Values Include '128', '192' and '256'.
        aes.Mode = CipherMode.CBC;          //Set Block Cipher Mode - Possible Values Include CBC, CFB, CTS, ECB, and OFB.
        aes.Padding = PaddingMode.PKCS7;    //Set Block Cipher Padding Mode - Possible Values Include 'PKCS7', 'ANSI X923', and 'ISO10126'.
        return aes;
    }
}
