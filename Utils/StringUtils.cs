using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Utils;

public class StringUtils
{
    public static byte[] StringToByteArray(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string ByteArrayToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    public static byte[] StringToByteArray(string str, Encoding encoding)
    {
        return encoding.GetBytes(str);
    }

    public static string ByteArrayToString(byte[] bytes, Encoding encoding)
    {
        return encoding.GetString(bytes);
    }


    public static string StringToHex(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    public static byte[] HexToByteArray(string hex)
    {
        int length = hex.Length / 2;
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

    public static string StringToHex(string str, Encoding encoding)
    {
        byte[] bytes = encoding.GetBytes(str);
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    public static byte[] HexToByteArray(string hex, Encoding encoding)
    {
        int length = hex.Length / 2;
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return encoding.GetBytes(encoding.GetString(bytes));
    }


}
