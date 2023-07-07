using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto.Hash;

public class SHA384
{
    public static string Encode(byte[] s)
    {
        // Use the using statement to dispose the SHA384 object
        using (System.Security.Cryptography.SHA384 sha384 = System.Security.Cryptography.SHA384.Create())
        {
            // Compute the hash of the input bytes
            byte[] data = sha384.ComputeHash(s);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }

    public static string EncodeFile(string s)
    {
        // Use the using statement to dispose the SHA384 and FileStream objects
        using (System.Security.Cryptography.SHA384 sha384 = System.Security.Cryptography.SHA384.Create())
        using (FileStream fs = File.OpenRead(s))
        {
            // Compute the hash of the file bytes
            byte[] data = sha384.ComputeHash(fs);

            // Convert the byte array to a hexadecimal string
            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}
