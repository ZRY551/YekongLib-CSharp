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

    
    public static byte[] Decode(string s)
    {
        // Check if the input is valid
        if (s == null) throw new ArgumentNullException(nameof(s));
        if (s.Length % 8 != 0) throw new ArgumentException("Invalid length", nameof(s));
        if (s.Any(c => !Alphabet.Contains(c) && c != Padding)) throw new ArgumentException("Invalid character", nameof(s));

        // Remove any padding characters from the end of the input
        int padCount = s.Count(c => c == Padding);
        s = s.TrimEnd(Padding);

        // Create a byte array to store the result
        byte[] result = new byte[s.Length * 5 / 8 - padCount];

        // Loop through the input string in groups of eight characters
        for (int i = 0; i < s.Length; i += 8)
        {
            // Convert each group of eight characters to a 40-bit value
            ulong bits = 0;
            for (int j = 0; j < 8; j++)
            {
                bits = (bits << 5) | (ulong)Alphabet.IndexOf(s[i + j]);
            }

            // Extract five bytes from the 40-bit value
            for (int k = 0; k < 5; k++)
            {
                // Check if we have reached the end of the result array
                if (i * 5 / 8 + k >= result.Length) break;

                // Get the byte value from the high end of the bits
                result[i * 5 / 8 + k] = (byte)(bits >> (32 - k * 8));
            }
        }

        // Return the result
        return result;
    }

    public static string Encode(byte[] b)
    {
        // Check if the input is valid
        if (b == null) throw new ArgumentNullException(nameof(b));

        // Create a string builder to store the result
        StringBuilder sb = new StringBuilder(b.Length * 8 / 5);

        // Loop through the input byte array in groups of five bytes
        for (int i = 0; i < b.Length; i += 5)
        {
            // Convert each group of five bytes to a 40-bit value
            ulong bits = 0;
            for (int j = 0; j < 5; j++)
            {
                // Check if we have reached the end of the input array
                if (i + j >= b.Length) break;

                // Get the byte value from the low end of the bits
                bits = (bits << 8) | b[i + j];
            }

            // Extract eight characters from the 40-bit value
            for (int k = 0; k < 8; k++)
            {
                // Check if we have reached the end of the output length
                if (i * 8 / 5 + k >= b.Length * 8 / 5) break;

                // Get the character index from the low end of the bits
                int index = (int)(bits >> (35 - k * 5)) & 0x1F;

                // Append the character to the result
                sb.Append(Alphabet[index]);
            }
        }

        // Add padding characters if necessary to make the output length a multiple of eight
        while (sb.Length % 8 != 0)
        {
            sb.Append(Padding);
        }

        // Return the result
        return sb.ToString();
    }
}
