using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace YekongLib.Crypto.Asymmetric;

public class RSA
{
    // Generate a RSA key pair of the specified size in bits and return it as a tuple of byte arrays
    public static (byte[] publicKey, byte[] privateKey) GenerateKeyPair(int keySize)
    {
        using (var rsa = new RSACryptoServiceProvider(keySize))
        {
            return (rsa.ExportRSAPublicKey(), rsa.ExportRSAPrivateKey());
        }
    }

    // Encrypt the data with the given public key using the specified RSA algorithm and return the encrypted bytes
    // If no padding is specified, use the default OAEP padding
    public static byte[] Encrypt(byte[] data, byte[] publicKey, RSAEncryptionPadding padding = null)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportRSAPublicKey(publicKey, out _);
            return rsa.Encrypt(data, padding ?? RSAEncryptionPadding.OaepSHA1);
        }
    }

    // Decrypt the data with the given private key using the specified RSA algorithm and return the decrypted bytes
    // If no padding is specified, use the default OAEP padding
    public static byte[] Decrypt(byte[] data, byte[] privateKey, RSAEncryptionPadding padding = null)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportRSAPrivateKey(privateKey, out _);
            return rsa.Decrypt(data, padding ?? RSAEncryptionPadding.OaepSHA1);
        }
    }

    // Sign the data with the given private key using the specified RSA algorithm and hash algorithm and return the signature bytes
    // If no hash algorithm or padding is specified, use the default SHA256 and PSS padding
    public static byte[] Sign(byte[] data, byte[] privateKey, HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportRSAPrivateKey(privateKey, out _);
            return rsa.SignData(data, hashAlgorithm == default ? HashAlgorithmName.SHA256 : hashAlgorithm, padding ?? RSASignaturePadding.Pss);
        }
    }

    // Verify the signature of the data with the given public key using the specified RSA algorithm and hash algorithm and return true if valid, false otherwise
    // If no hash algorithm or padding is specified, use the default SHA256 and PSS padding
    public static bool Verify(byte[] data, byte[] signature, byte[] publicKey, HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportRSAPublicKey(publicKey, out _);
            return rsa.VerifyData(data, signature, hashAlgorithm == default ? HashAlgorithmName.SHA256 : hashAlgorithm, padding ?? RSASignaturePadding.Pss);
        }
    }
}
