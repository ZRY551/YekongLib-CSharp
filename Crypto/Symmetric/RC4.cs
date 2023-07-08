using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace YekongLib.Crypto.Symmetric;

public class RC4
{
    private const int BlockSize = 8; // RC4 operates on bytes

    private const int IVSize = 0; // RC4 does not use an IV

    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        // Create a new RC4 instance with the given key
        var rc4 = new RC4Internal(key);

        // Create a memory stream to store the encrypted data
        var ms = new MemoryStream();

        // Encrypt each byte of the data and write it to the stream
        foreach (var b in data)
        {
            ms.WriteByte(rc4.EncryptByte(b));
        }

        // Return the encrypted data as an array
        return ms.ToArray();
    }

    public static byte[] Decrypt(byte[] data, byte[] key)
    {
        // Create a new RC4 instance with the given key
        var rc4 = new RC4Internal(key);

        // Create a memory stream to store the decrypted data
        var ms = new MemoryStream();

        // Decrypt each byte of the data and write it to the stream
        foreach (var b in data)
        {
            ms.WriteByte(rc4.DecryptByte(b));
        }

        // Return the decrypted data as an array
        return ms.ToArray();
    }
}

// A helper class that implements the RC4 algorithm
internal class RC4Internal
{
    private readonly byte[] s; // The permutation array
    private int i; // The first index
    private int j; // The second index

    public RC4Internal(byte[] key)
    {
        // Initialize the permutation array with 0 to 255
        s = new byte[256];
        for (int k = 0; k < 256; k++)
        {
            s[k] = (byte)k;
        }

        // Shuffle the array using the key
        int l = key.Length;
        int t;
        for (int k = 0, m = 0; k < 256; k++)
        {
            m = (m + s[k] + key[k % l]) & 255;
            t = s[k];
            s[k] = s[m];
            s[m] = (byte)t;
        }

        // Initialize the indices
        i = 0;
        j = 0;
    }

    public byte EncryptByte(byte b)
    {
        // Generate a pseudo-random byte from the permutation array
        i = (i + 1) & 255;
        j = (j + s[i]) & 255;
        int t = s[i];
        s[i] = s[j];
        s[j] = (byte)t;
        int k = s[(s[i] + s[j]) & 255];

        // XOR it with the input byte to get the encrypted byte
        return (byte)(b ^ k);
    }

    public byte DecryptByte(byte b)
    {
        // Decrypting is the same as encrypting for RC4
        return EncryptByte(b);
    }
}

