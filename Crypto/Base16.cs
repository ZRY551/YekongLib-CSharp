using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto;

public class Base16
{
    // The base 16 alphabet
    private const string Alphabet = "0123456789ABCDEF";

    public static byte[] Encode(string s)
    {
        // Convert the input string to bytes using UTF-8 encoding
        byte[] bytes = Encoding.UTF8.GetBytes(s);

        // Create a StringBuilder to store the output
        StringBuilder sb = new StringBuilder();

        // Loop through the input bytes
        foreach (byte b in bytes)
        {
            // Encode the byte into 2 characters, using 4 bits per character
            sb.Append(Alphabet[b >> 4]); // Get the high 4 bits and append the corresponding character
            sb.Append(Alphabet[b & 0x0F]); // Get the low 4 bits and append the corresponding character
        }

        // Return the output as a byte array
        return Encoding.ASCII.GetBytes(sb.ToString());
    }

    public static string Decode(byte[] b)
    {
        // Convert the input bytes to a string using ASCII encoding
        string s = Encoding.ASCII.GetString(b);

        // Check if the input is valid (length is even and contains only valid characters)
        if (s.Length % 2 != 0 || s.Any(c => !Alphabet.Contains(c)))
        {
            throw new ArgumentException("Invalid base 16 input");
        }

        // Create a MemoryStream to store the output bytes
        MemoryStream ms = new MemoryStream();

        // Loop through the input string in groups of 2 characters
        for (int i = 0; i < s.Length; i += 2)
        {
            // Decode the group into a byte, using 4 bits per character
            byte b2 = 0;

            // Get the index of the first character in the alphabet and copy its bits to the high 4 bits of the byte
            int index = Alphabet.IndexOf(s[i]);
            b2 |= (byte)(index << 4);

            // Get the index of the second character in the alphabet and copy its bits to the low 4 bits of the byte
            index = Alphabet.IndexOf(s[i + 1]);
            b2 |= (byte)index;

            // Write the byte to the output stream
            ms.WriteByte(b2);
        }

        // Return the output as a string using UTF-8 encoding
        return Encoding.UTF8.GetString(ms.ToArray());
    }
}
