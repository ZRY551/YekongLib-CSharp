using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto.Hash;

public class SHA1
{
    public static string Encode(byte[] s)
    {
        using (System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
        {
            // Compute the hash of the input bytes
            byte[] data = sha1.ComputeHash(s);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }

    public static string EncodeFile(string s)
    {
        using (System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
        using (FileStream fs = File.OpenRead(s))
        {
            // Compute the hash of the file bytes
            byte[] data = sha1.ComputeHash(fs);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}
