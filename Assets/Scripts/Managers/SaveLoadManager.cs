using UnityEngine;
using System.IO;
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    private void Awake()
    {
        if(instance = null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);	
        }
    }
    public void SaveExample(ExampleData dataToSave, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(savePath,JsonUtility.ToJson(dataToSave, true));
    }
    public void LoadExample(ExampleData dataToLoadInto, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath,fileName);
        if (File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString,dataToLoadInto);
        }
    }
    public void DeleteExample(string fileName)
    {
        string dataPath = Path.Combine(Application.persistentDataPath,fileName);
        File.Delete(dataPath);
    }
}
