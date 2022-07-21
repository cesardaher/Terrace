using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class SaveSystem 
{ 
    public static void SaveScene(SceneProperties properties)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save";

        FileStream stream = new FileStream(path, FileMode.Create);

        SceneData data = new SceneData(properties);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static SceneData LoadScene()
    {
        string path = Application.persistentDataPath + "/save";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SceneData data = formatter.Deserialize(stream) as SceneData;

            stream.Close();
            
            return data;

        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }

    public static void DeleteSaveFile()
    {
        string path = Application.persistentDataPath + "/save";
        if (File.Exists(path))
        {

        }

    }
}
