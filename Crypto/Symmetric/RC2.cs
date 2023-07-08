using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto.Symmetric;

public class RC2
{
    // A constant block size of 64 bits for RC2
    private const int BlockSize = 64;

    // A constant initialization vector size of 8 bytes for RC2
    private const int IVSize = 8;

    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        // Check the input parameters for null values
        if (data == null || data.Length == 0)
            throw new ArgumentNullException(nameof(data));
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key));

        // Check the key size for RC2
        if (key.Length < 1 || key.Length > 128)
            throw new ArgumentException($"Invalid key size. Expected 8-1024 bits, got {key.Length * 8} bits.", nameof(key));

        // Create a new instance of the RC2 class with the specified key and block size
        using (System.Security.Cryptography.RC2 rc2 = System.Security.Cryptography.RC2.Create())
        {
            rc2.BlockSize = BlockSize;
            rc2.Key = key;

            // Generate a random initialization vector and assign it to the RC2 instance
            rc2.GenerateIV();
            byte[] iv = rc2.IV;

            // Create a memory stream to store the encrypted data
            using (MemoryStream ms = new MemoryStream())
            {
                // Write the initialization vector to the memory stream
                ms.Write(iv, 0, iv.Length);

                // Create a crypto stream to perform encryption using the RC2 instance
                using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, rc2.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                {
                    // Write the plain data to the crypto stream
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();

                    // Return the encrypted data from the memory stream as a byte array
                    return ms.ToArray();
                }
            }
        }
    }

    public static byte[] Decrypt(byte[] data, byte[] key)
    {
        // Check the input parameters for null values
        if (data == null || data.Length == 0)
            throw new ArgumentNullException(nameof(data));
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key));

        // Check the key size for RC2
        if (key.Length < 1 || key.Length > 128)
            throw new ArgumentException($"Invalid key size. Expected 8-1024 bits, got {key.Length * 8} bits.", nameof(key));

        // Create a new instance of the RC2 class with the specified key and block size
        using (System.Security.Cryptography.RC2 rc2 = System.Security.Cryptography.RC2.Create())
        {
            rc2.BlockSize = BlockSize;
            rc2.Key = key;

            // Create a memory stream to read the encrypted data
            using (MemoryStream ms = new MemoryStream(data))
            {
                // Read the initialization vector from the memory stream
                byte[] iv = new byte[IVSize];
                ms.Read(iv, 0, iv.Length);

                // Assign the initialization vector to the RC2 instance
                rc2.IV = iv;

                // Create a crypto stream to perform decryption using the RC2 instance
                using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, rc2.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Read))
                {
                    // Create a buffer to store the decrypted data
                    byte[] buffer = new byte[data.Length - iv.Length];

                    // Read the decrypted data from the crypto stream
                    int bytesRead = cs.Read(buffer, 0, buffer.Length);

                    // Resize the buffer to match the actual decrypted data size
                    Array.Resize(ref buffer, bytesRead);

                    // Return the decrypted data as a byte array
                    return buffer;
                }
            }
        }
    }
}
