using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;


public static class FileHandler
{
    public static void SaveToJson<T>(List<T> toSave,string fileName)
    {
        Debug.Log(GetPath(fileName));
        string contnent = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(GetPath(fileName), contnent);
    }
    public static void SaveToJson<T>(T toSave, string fileName)
    {
        Debug.Log(GetPath(fileName));
        string contnent = JsonUtility.ToJson(toSave);
        WriteFile(GetPath(fileName), contnent);
    }
    public static List<T> ReadFromListJson<T>(string filename)
    {
        string contnet = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(contnet) || contnet == "{}")
        {
            return new List<T>();
        }

        List<T> res = JsonHelper.FromJson<T>(contnet).ToList();

        return res;
    }
    public static T ReadFromJson<T>(string filename)
    {
        string contnet = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(contnet) || contnet == "{}")
        {
            return default(T);
        }

        T res = JsonUtility.FromJson<T>(contnet);

        return res;
    }

    private static string GetPath(string filename)
    {
        return Application.dataPath+"/"+filename;
    }

    private static void WriteFile(string path,string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using(StreamReader reader=new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
