using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string ID;
    public ItemType itemType;
    public float damage;
    public float shootCooldown;
    public bool isAutomatic;

    [Header("Ammo")]
    public int currentAmmo;
    public int maxAmmo;
    public int storageAmmo;

    [Header("References")]
    public Transform shootingPoints;
    public Transform shellSpawnPoints;
    public GameObject shellPrefab;

    public float visibleLineTime;
}
