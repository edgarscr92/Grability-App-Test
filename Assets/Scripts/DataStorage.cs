using System.IO;
using UnityEngine;

public class DataStorage
{
    /// <summary>
    /// Generic method used for saving data
    /// </summary>
    /// <param name="dataToStore">Data to store</param>
    /// <param name="method">Storing method</param>
    /// <typeparam name="T">Typeof data to store</typeparam>
    public static void SaveToFile<T>(T dataToStore, string _name)
    {

        string fileName = SavingLocation(_name) + ".json";

        try
        {
            string serializedData = JsonUtility.ToJson(dataToStore, true);
            File.WriteAllText(fileName, serializedData);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("File writing error: " + ex.Message);
        }
    }

    /// <summary>
    /// Generic method used for reading save data
    /// </summary>
    /// <returns>Read data</returns>
    /// <param name="method">Storing method</param>
    /// <typeparam name="T">Typeof data to store</typeparam>
    public static T LoadFromFIle<T>(string _name)
    {

        T storedData = default(T);

        string fileName = SavingLocation(_name) + ".json";

        try
        {
            string serializedData = File.ReadAllText(fileName);
            storedData = JsonUtility.FromJson<T>(serializedData);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("File reading error: " + ex.Message);
        }

        return storedData;
    }

    public static string SavingLocation(string _name)
    {
#if UNITY_EDITOR
        return Path.Combine(Application.persistentDataPath, _name);
#elif UNITY_ANDROID
        return Path.Combine(Application.persistentDataPath, _name);
#elif UNITY_IOS
        return Path.Combine(Application.persistentDataPath, _name);
#else
        return Path.Combine(Application.dataPath, _name);
#endif
    }

    public static string WriteTextureToPlayerPrefs(string tag, Texture2D tex)
    {
        // if texture is png otherwise you can use tex.EncodeToJPG().
        byte[] texByte = tex.EncodeToPNG();

        // convert byte array to base64 string
        string base64Tex = System.Convert.ToBase64String(texByte);

        // write string to playerpref
        PlayerPrefs.SetString(tag, base64Tex);
        PlayerPrefs.Save();
        return tag;
    }

    public static Texture2D ReadTextureFromPlayerPrefs(string tag)
    {
        // load string from playerpref
        string base64Tex = PlayerPrefs.GetString(tag);

        Texture2D tex = new Texture2D(2, 2);

        // convert it to byte array
        byte[] texByte = System.Convert.FromBase64String(base64Tex);
        
        tex.LoadImage(texByte);

        return tex;
    }
}
