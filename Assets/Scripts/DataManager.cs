using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class DataManager
{

    public const string ZONE_OVERVIEW_PATH = "Player/Zones/Overviews/";
    public const string LEVEL_RECORDS_PATH = "Player/Zones/LevelRecords/";
    public const string PLAYER_INFO_PATH = "Player/Info/";
    public static void Save(string fileName,string location, object data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + location;
        Directory.CreateDirectory(path);
        FileStream file = File.Open(path + fileName, FileMode.OpenOrCreate);
        try
        {
            bf.Serialize(file, data);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {

        file.Close();

        }
    }

    public static T Load<T>(string fileName, string location)
    {
        FileStream file = null;
        try
        {
            string path = Application.persistentDataPath + "/"+location + fileName;
            file = File.Open(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            return (T)bf.Deserialize(file);
        }
        catch (System.Exception e)
        {
            //Debug.LogException(e);
            return default;
        }
        finally
        {
            file?.Close();

        }
    }
}
