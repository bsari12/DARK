using System.IO;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriter;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private CheckpointData checkPointData;
    
    void Start()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if(File.Exists(loadPath))
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.instance.Load(helpCheck, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            if(helpCheck.checkPointKey == checkPointData.checkPointKey)
            {
                spriter.sprite = spriteEnabled;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkPoint = this;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkPoint = null;
        }
    }
    public void Activate()
    {
        spriter.sprite = spriteEnabled;
        //save data
        SaveLoadManager.instance.Save(checkPointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        
    }
}
