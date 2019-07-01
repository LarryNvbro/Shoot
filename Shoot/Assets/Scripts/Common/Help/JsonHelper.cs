using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;


public class JsonHelper
{
    static List<string> errors = new List<string>();

    public static T DeserializeObject<T>(string json)
    {
        Debug.Log("Parse Json " + typeof(T));

        if (Application.isEditor)
        {
            Debug.Log(json.SubstringFirst(10000));
        }

        errors.Clear();

        T result;
        try
        {
            result = JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception e)
        {
            result = default(T);
            Debug.LogException(e);
        }

        return result;
    }

    public static T DeserializeObjectWithCompression<T>(string json)
    {
        Debug.Log("Parse Json");

        errors.Clear();

        T result;
        try
        {
            byte[] buffer = Convert.FromBase64String(json);

            byte[] decompressed = CLZF2.Decompress(buffer);

            Debug.Log("size : " + decompressed.Length);

            string stringdecompressed = Encoding.UTF8.GetString(decompressed);
            if (Application.isEditor)
            {
                //Debug.Log(stringdecompressed);
            }
            Debug.Log(stringdecompressed);
            result = JsonConvert.DeserializeObject<T>(stringdecompressed);
            Debug.Log("Success Parse Json");
        }
        catch (Exception e)
        {
            result = default(T);
            Debug.Log("Fail Parse Json");
            Debug.LogException(e);

            throw e;
        }

        return result;
    }

    public static string SerializeObject<T>(object obj)
    {
        string result = null;
        try
        {
            result = JsonConvert.SerializeObject(obj);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return result;
    }

    public static string SerializeObjectWithCompression<T>(object obj)
    {
        string result = null;
        try
        {
            result = JsonConvert.SerializeObject(obj);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        Debug.Log("json : " + result);

        byte[] buffer = Encoding.UTF8.GetBytes(result);

        Debug.Log("size : " + buffer.Length);

        byte[] compressed = CLZF2.Compress(buffer);

        Debug.Log("compressed size : " + compressed.Length);

        return Convert.ToBase64String(compressed);
    }
}
