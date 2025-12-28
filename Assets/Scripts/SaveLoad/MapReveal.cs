using UnityEngine;

public class MapReveal : MonoBehaviour
{
    [SerializeField] private string mapKey;
    private MinimapDisplayControl mapDisplayControl;

    void Start()
    {
        mapDisplayControl = FindAnyObjectByType<MinimapDisplayControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            mapDisplayControl.LoadMinimapData();
            mapDisplayControl.minimapData.AddToListWithCheck(mapKey);
            mapDisplayControl.DisplayUnlockedMinimaps();
            SaveLoadManager.instance.Save(mapDisplayControl.minimapData,SaveLoadManager.instance.folderName,SaveLoadManager.instance.minimapFileName);
        }
    }
}
