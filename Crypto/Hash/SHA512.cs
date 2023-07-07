using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto.Hash;

public class SHA512
{
    public static string Encode(byte[] s)
    {
        // Use the using statement to dispose the SHA512 object
        using (System.Security.Cryptography.SHA512 sha512 = System.Security.Cryptography.SHA512.Create())
        {
            // Compute the hash of the input bytes
            byte[] data = sha512.ComputeHash(s);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }

    public static string EncodeFile(string s)
    {
        // Use the using statement to dispose the SHA512 and FileStream objects
        using (System.Security.Cryptography.SHA512 sha512 = System.Security.Cryptography.SHA512.Create())
        using (FileStream fs = File.OpenRead(s))
        {
            // Compute the hash of the file bytes
            byte[] data = sha512.ComputeHash(fs);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}
