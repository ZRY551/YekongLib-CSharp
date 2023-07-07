using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto;

public class Base16
{
    // The base 16 alphabet
    private const string Alphabet = "0123456789ABCDEF";

    public static byte[] Decode(string s)
    {
        // Check if the input is valid
        if (s == null) throw new ArgumentNullException(nameof(s));
        if (s.Length % 2 != 0) throw new ArgumentException("Invalid length", nameof(s));
        if (s.Any(c => !Alphabet.Contains(c))) throw new ArgumentException("Invalid character", nameof(s));

        // Create a byte array to store the result
        byte[] result = new byte[s.Length / 2];

        // Loop through the input string in pairs of two characters
        for (int i = 0; i < s.Length; i += 2)
        {
            // Convert each pair of characters to a byte value
            result[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
        }

        // Return the result
        return result;
    }

    public static string Encode(byte[] b)
    {
        // Check if the input is valid
        if (b == null) throw new ArgumentNullException(nameof(b));

        // Create a string builder to store the result
        StringBuilder sb = new StringBuilder(b.Length * 2);

        // Loop through the input byte array
        foreach (byte x in b)
        {
            // Convert each byte value to a pair of characters
            sb.Append(Alphabet[x >> 4]); // Get the high nibble
            sb.Append(Alphabet[x & 0x0F]); // Get the low nibble
        }

        // Return the result
        return sb.ToString();
    }
}
