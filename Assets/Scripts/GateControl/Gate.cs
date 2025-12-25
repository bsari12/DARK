using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Load Next Level");
        }
    }

}
