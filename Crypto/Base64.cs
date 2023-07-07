using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto;

public class Base64
{
    public static byte[] Decode(string s)
    {
        return System.Convert.FromBase64String(s);
    }

    public static string Encode(byte[] b)
    {
        return System.Convert.ToBase64String(b);
    }



}
