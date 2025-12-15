using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SSD_Assignment___Banking_Application
{
    public class AES_DLE
    {
        public byte[] Encrypt(byte[] plaintext_data)
        {
            Aes aes = Setup_AES();
            byte[] ciphertext_data;//Byte Array Where Result Of Encryption Operation Will Be Stored.

            ICryptoTransform encryptor = aes.CreateEncryptor();//Object That Contains The AES Encryption Algorithm (Using The Key and IV Value Specified In The AES Object). 

            MemoryStream msEncrypt = new MemoryStream();//MemoryStream That Will Store The Output Of The Encryption Operation.

            /*
                Calling The Write() Method On The CryptoStream Object Declared Below Will Store/Write 
                The Result Of The Encryption Operation To The Memory Stream Object Specified In The Constructor.
            */
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(plaintext_data, 0, plaintext_data.Length);//Writes All Data Contained In plaintext_data Byte Array To CryptoStream (Which Then Gets Encrypted And Gets Written to the msEncrypt MemoryStream).
            csEncrypt.Dispose();//Closes CryptoStream

            ciphertext_data = msEncrypt.ToArray();//Output Result Of Encryption Operation In Byte Array Form.
            msEncrypt.Dispose();//Closes MemoryStream

            return ciphertext_data;

        }

        public byte[] Decrypt(byte[] ciphertext_data)
        {
            Aes aes = Setup_AES();
            byte[] plaintext_data;//Byte Array Where Result Of Decryption Operation Will Be Stored.

            ICryptoTransform decryptor = aes.CreateDecryptor();//Object That Contains The AES Decryption Algorithm (Using The Key and IV Value Specified In The AES Object). 

            MemoryStream msDecrypt = new MemoryStream();//MemoryStream That Will Store The Output Of The Decryption Operation.

            /*
                Calling The Write() Method On The CryptoStream Object Declared Below Will Store/Write 
                The Result Of The Decryption Operation To The Memory Stream Object Specified In The Constructor.
            */
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write);//Writes All Data Contained In Byte Array To CryptoStream (Which Then Gets Decrypted).
            csDecrypt.Write(ciphertext_data, 0, ciphertext_data.Length);//Writes All Data Contained In ciphertext_data Byte Array To CryptoStream (Which Then Gets Decrypted And Gets Written to the msDecrypt MemoryStream).
            csDecrypt.Dispose();//Closes CryptoStream

            plaintext_data = msDecrypt.ToArray();//Output Result Of Decryption Operation In Byte Array Form.
            msDecrypt.Dispose();//Closes MemoryStream

            return plaintext_data;

        }

        public Aes Setup_AES()
        {
            string key_string = "oM4BmmHcux7FBnCnk7vI80qUWF/4zfYpFyJnvyYFG3A=";
            string iv_string = "m3UiRuxNAWhD7ul5tDdwBA==";

            byte[] key = Convert.FromBase64String(key_string); // 32 bytes for AES-256
            byte[] iv = Convert.FromBase64String(iv_string);   // 16 bytes for AES block

            Aes aes = Aes.Create();
            aes.KeySize = 256;                  //Set Cipher Key Size. This Is An AES Only Setting - Possible Values Include '128', '192' and '256'.
            aes.Key = key;                      //Set Key Value (Declared And Populated Above)           
            aes.Mode = CipherMode.CBC;          //Set Block Cipher Mode - Possible Values Include CBC, CFB, CTS, ECB, and OFB.
            aes.IV = iv;                        //Set IV Value (Declared And Populated Above) - Only Used In Non-ECB Block Cipher Modes 
            aes.Padding = PaddingMode.PKCS7;    //Set Block Cipher Padding Mode - Possible Values Include 'PKCS7', 'ANSI X923', and 'ISO10126'.
            return aes;
        }
    }
}
