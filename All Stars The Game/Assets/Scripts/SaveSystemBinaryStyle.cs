using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystemBinaryStyle
{
    public static void SaveEnemy(GameHandlerMap gameMap)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameMap.sad";
        FileStream stream = new FileStream(path, FileMode.Create);

        //EnemySpawnerData data = new EnemySpawnerData(gameMap);

        //formatter.Serialize(stream, data);
        stream.Close();
    }

    public static EnemySpawnerData LoadGameMap()
    {
        string path = Application.persistentDataPath + "/gameMap.sad";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            EnemySpawnerData data = formatter.Deserialize(stream) as EnemySpawnerData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
