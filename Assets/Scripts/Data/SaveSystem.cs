using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(SaveData data){
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/data.fun";
        //string path = Application.dataPath + "/data.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        //SaveData data = new SaveData(_nivells);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadData(){
        string path = Application.persistentDataPath + "/data.fun";
        //string path = Application.dataPath + "/data.fun";
        
        if(File.Exists(path)){
            Debug.Log("File found in " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else{
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteData(){
        string path = Application.persistentDataPath + "/data.fun";

        if (File.Exists(path)){
            File.Delete(path);
            Debug.Log("Save file deleted from " + path);
        }
        else{
            Debug.LogWarning("No save file found to delete at " + path);
        }
    }
}
