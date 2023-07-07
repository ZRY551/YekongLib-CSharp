using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto.Hash;

public class MD5
{
    public static string Encode(byte[] s)
    {
        // Use the using statement to dispose the MD5 object
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            // Compute the hash of the input bytes
            byte[] data = md5.ComputeHash(s);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }

    public static string EncodeFile(string s)
    {
        // Use the using statement to dispose the MD5 and FileStream objects
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        using (FileStream fs = File.OpenRead(s))
        {
            // Compute the hash of the file bytes
            byte[] data = md5.ComputeHash(fs);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}
