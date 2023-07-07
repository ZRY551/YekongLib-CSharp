using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto.Hash;

public class SHA256
{
    public static string Encode(byte[] s)
    {
        // Use the using statement to dispose the SHA256 object
        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
        {
            // Compute the hash of the input bytes
            byte[] data = sha256.ComputeHash(s);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }

    public static string EncodeFile(string s)
    {
        // Use the using statement to dispose the SHA256 and FileStream objects
        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
        using (FileStream fs = File.OpenRead(s))
        {
            // Compute the hash of the file bytes
            byte[] data = sha256.ComputeHash(fs);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}
