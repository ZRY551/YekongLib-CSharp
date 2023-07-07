using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace YekongLib.Crypto.Symmetric;

public class AES128
{
    // A constant key size of 128 bits for AES-128
    private const int KeySize = 128;

    // A constant block size of 128 bits for AES
    private const int BlockSize = 128;

    // A constant initialization vector size of 16 bytes for AES
    private const int IVSize = 16;

    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        // Check the input parameters for null values
        if (data == null || data.Length == 0)
            throw new ArgumentNullException(nameof(data));
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key));

        // Check the key size for AES-128
        if (key.Length * 8 != KeySize)
            throw new ArgumentException($"Invalid key size. Expected {KeySize} bits, got {key.Length * 8} bits.", nameof(key));

        // Create a new instance of the Aes class with the specified key and block size
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = key;

            // Generate a random initialization vector and assign it to the Aes instance
            aes.GenerateIV();
            byte[] iv = aes.IV;

            // Create a memory stream to store the encrypted data
            using (MemoryStream ms = new MemoryStream())
            {
                // Write the initialization vector to the memory stream
                ms.Write(iv, 0, iv.Length);

                // Create a crypto stream to perform encryption using the Aes instance
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
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

        // Check the key size for AES-128
        if (key.Length * 8 != KeySize)
            throw new ArgumentException($"Invalid key size. Expected {KeySize} bits, got {key.Length * 8} bits.", nameof(key));

        // Create a new instance of the Aes class with the specified key and block size
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = key;

            // Create a memory stream to read the encrypted data
            using (MemoryStream ms = new MemoryStream(data))
            {
                // Read the initialization vector from the memory stream
                byte[] iv = new byte[IVSize];
                ms.Read(iv, 0, iv.Length);

                // Assign the initialization vector to the Aes instance
                aes.IV = iv;

                // Create a crypto stream to perform decryption using the Aes instance
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
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
