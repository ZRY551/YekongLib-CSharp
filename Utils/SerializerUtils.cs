using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace YekongLib.Utils;

public class SerializerUtils
{
    // Converts an object to an XML string
    public static string ToXml<T>(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var sw = new StringWriter();
        serializer.Serialize(sw, obj);
        return sw.ToString();
    }

    // Converts an XML string to an object
    public static T FromXml<T>(string xml)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var sr = new StringReader(xml);
        return (T)serializer.Deserialize(sr);
    }

    // Converts an object to a JSON string
    public static string ToJson<T>(T obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    // Converts a JSON string to an object
    public static T FromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    // Converts an object to a binary array
    public static byte[] ToBinary<T>(T obj)
    {
        var formatter = new BinaryFormatter();
        using var ms = new MemoryStream();
        formatter.Serialize(ms, obj);
        return ms.ToArray();
    }

    // Converts a binary array to an object
    public static T FromBinary<T>(byte[] bytes)
    {
        var formatter = new BinaryFormatter();
        using var ms = new MemoryStream(bytes);
        return (T)formatter.Deserialize(ms);
    }
}

