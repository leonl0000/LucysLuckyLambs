using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static int saveSlot = 0;

    public static string[] saves = {Path.Combine(Application.persistentDataPath, "save_slot_1.bin"),
        Path.Combine(Application.persistentDataPath, "save_slot_2.bin"),
        Path.Combine(Application.persistentDataPath, "save_slot_3.bin")};

    #if UNITY_STANDALONE_WIN
    #endif

    #if UNITY_WEBGL

    #endif

    public static void SaveGame(hellSceneManager hsm, int saveSlot) {
        string save = saves[saveSlot - 1];
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(save, FileMode.Create);

        SaveData saveData = new SaveData(hsm);
        binaryFormatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static bool SaveFileExists(int saveSlot) {
        return File.Exists(saves[saveSlot - 1]);
    }

    public static SaveData LoadGame(int saveSlot) {
        string save = saves[saveSlot - 1];
        if(File.Exists(save)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(save, FileMode.Open);

            SaveData saveData = binaryFormatter.Deserialize(stream) as SaveData;
            stream.Close();
            return saveData;


        } else {
            Debug.Log(save + " not found");
            return null;
        }
    } 

}
