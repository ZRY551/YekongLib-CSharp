using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YekongLib.Crypto;

public class Base32
{
    // The base 32 alphabet
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    // The padding character
    private const char Padding = '=';

    public static byte[] Encode(string s)
    {
        // Convert the input string to bytes using UTF-8 encoding
        byte[] bytes = Encoding.UTF8.GetBytes(s);

        // Create a StringBuilder to store the output
        StringBuilder sb = new StringBuilder();

        // Loop through the input bytes in groups of 5
        for (int i = 0; i < bytes.Length; i += 5)
        {
            // Get the number of bytes in the current group
            int n = Math.Min(5, bytes.Length - i);

            // Create a 40-bit buffer to store the group
            ulong buffer = 0;

            // Copy the bytes to the buffer, left-aligned
            for (int j = 0; j < n; j++)
            {
                buffer |= ((ulong)bytes[i + j]) << (8 * (4 - j));
            }

            // Encode the buffer into 8 characters, using 5 bits per character
            for (int j = 0; j < 8; j++)
            {
                // Get the index of the character in the alphabet, using bitwise operations
                int index = (int)((buffer >> (35 - 5 * j)) & 0x1F);

                // Append the character to the output, or a padding if there are no more bits
                sb.Append(j < n * 8 / 5 ? Alphabet[index] : Padding);
            }
        }

        // Return the output as a byte array
        return Encoding.ASCII.GetBytes(sb.ToString());
    }

    public static string Decode(byte[] b)
    {
        // Convert the input bytes to a string using ASCII encoding
        string s = Encoding.ASCII.GetString(b);

        // Check if the input is valid (length is a multiple of 8 and contains only valid characters)
        if (s.Length % 8 != 0 || s.Any(c => c != Padding && !Alphabet.Contains(c)))
        {
            throw new ArgumentException("Invalid base 32 input");
        }

        // Create a MemoryStream to store the output bytes
        MemoryStream ms = new MemoryStream();

        // Loop through the input string in groups of 8 characters
        for (int i = 0; i < s.Length; i += 8)
        {
            // Get the number of padding characters in the current group
            int p = s.Substring(i, 8).Count(c => c == Padding);

            // Create a 40-bit buffer to store the group
            ulong buffer = 0;

            // Decode the group into the buffer, using 5 bits per character
            for (int j = 0; j < 8 - p; j++)
            {
                // Get the index of the character in the alphabet
                int index = Alphabet.IndexOf(s[i + j]);

                // Copy the bits to the buffer, left-aligned
                buffer |= ((ulong)index) << (35 - 5 * j);
            }

            // Write the buffer to the output stream, using 8 bits per byte and skipping any padding bits
            for (int j = 0; j < 5 - p; j++)
            {
                // Get the byte from the buffer, using bitwise operations
                byte b2 = (byte)((buffer >> (32 - 8 * j)) & 0xFF);

                // Write the byte to the output stream
                ms.WriteByte(b2);
            }
        }

        // Return the output as a string using UTF-8 encoding
        return Encoding.UTF8.GetString(ms.ToArray());
    }
}
