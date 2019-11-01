using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad<T> where T : class
{
    public static void Save(T data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Path.Combine(Application.dataPath, $"{typeof(T)}.save"), FileMode.Create);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static T Load()
    {
        if (File.Exists(Path.Combine(Application.dataPath, $"{typeof(T)}.save")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Path.Combine(Application.dataPath, $"{typeof(T)}.save"), FileMode.Open);

            T data = bf.Deserialize(stream) as T;
            stream.Close();
            return data;
        }

        DebugWrapper.LogError($"'{typeof(T)}.save' File Not Exist.");
        return null;
    }
}
