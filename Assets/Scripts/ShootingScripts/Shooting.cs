using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    public InputActionReference shootActionRef;
    public Weapon currentWeapon;
    public InputActionReference changeweaponRef;
    private Player player;
    
    private ItemType currentWeaponType;
    private bool shootButtonHeld;
    private bool shootCooldownOver = true;

    [Header("Raycast Stuff")]
    [SerializeField] private LayerMask whatToHit;
    [SerializeField] private LineRenderer lineRenderer;
    private bool isShootingLineActive= false;
    private Vector3 startPoint;
    private Vector3 endPoint;

    public static Action<Sprite, int,int,int> OnUpdateAllInfo;
    public static Action <int,int,int> OnUpdateAmmo;


    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        LoadWeapons();
        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite,currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    private void LoadWeapons()
    {
        foreach(Weapon weapon in player.listToSaveAndLoad)
        {
            weapon.LoadWeaponData();
        }
    }
    void OnEnable()
    {
        shootActionRef.action.performed += TryToShoot;
        shootActionRef.action.canceled += StopShooting;
        changeweaponRef.action.performed += TryToChangeWeapon;
    }
    void OnDisable()
    {
        shootActionRef.action.performed -= TryToShoot;
        shootActionRef.action.canceled -= StopShooting;
        changeweaponRef.action.performed -= TryToChangeWeapon;
    }

    private void TryToShoot(InputAction.CallbackContext value)
    {
        if(currentWeapon == null || player.stateMachine.currentState==PlayerStates.State.Ladders || player.stateMachine.currentState ==   PlayerStates.State.Dash || player.stateMachine.currentState == PlayerStates.State.WallSlide || player.stateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if(shootButtonHeld || shootCooldownOver == false)
            return;
        
        if (currentWeapon.isAutomatic)
        {
            shootButtonHeld = true;
            return;
        }
        shootButtonHeld = true;
        Shoot();
    }
    private void StopShooting(InputAction.CallbackContext value)
    {
        shootButtonHeld = false;
    }

    private void TryToChangeWeapon(InputAction.CallbackContext value)
    {
        if(currentWeapon == null || player.stateMachine.currentState==PlayerStates.State.Ladders || player.stateMachine.currentState ==   PlayerStates.State.Dash || player.stateMachine.currentState == PlayerStates.State.WallSlide || player.stateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        
        if(currentWeapon.isReloading)
            return;
        
        if(currentWeapon.itemType == ItemType.PrimaryWeapon)
        {
            if(player.secondaryWeaponPrefab == null)
                return;
            
            player.primaryWeaponPrefab.SetActive(false);
            player.secondaryWeaponPrefab.SetActive(true);
            player.currentWeaponPrefab = player.secondaryWeaponPrefab;
            currentWeaponType = ItemType.SecondaryWeapon;
            player.currentWeaponType = currentWeaponType;
            currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
            player.anim.SetLayerWeight(1,1);
            player.SetWeaponPosition();
        }
        else
        {
            if(player.primaryWeaponPrefab == null)
                return;
            
            player.primaryWeaponPrefab.SetActive(true);
            player.secondaryWeaponPrefab.SetActive(false);
            player.currentWeaponPrefab = player.primaryWeaponPrefab;
            currentWeaponType = ItemType.PrimaryWeapon;
            player.currentWeaponType = currentWeaponType;
            currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
            player.anim.SetLayerWeight(1,0);
            player.SetWeaponPosition();
        }
        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite,currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    private void Shoot()
    {
        if(currentWeapon.currentAmmo<=0 || currentWeapon.isReloading)
            return;
        
        currentWeapon.source.Play();

        Instantiate(currentWeapon.shellPrefab, currentWeapon.shellSpawnPoints.position, currentWeapon.transform.rotation);
        currentWeapon.effectPrefab.transform.position = currentWeapon.shootingPoints.position;
        currentWeapon.effectPrefab.SetActive(true);

        if(player.stateMachine.currentState!=PlayerStates.State.ShootUp)
            currentWeapon.transform.localPosition = player.defaultWeaponVectorPose - Vector3.right*currentWeapon.recoilStrength;
        else
            currentWeapon.transform.localPosition = player.defaultWeaponVectorPose - Vector3.up*currentWeapon.recoilStrength;

        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = currentWeapon.widthMultiplier;
        
        Vector3 direction = currentWeapon.shootingPoints.right;
        RaycastHit2D hitInfo = Physics2D.Raycast(currentWeapon.shootingPoints.position,direction,Mathf.Infinity,whatToHit);
        if (hitInfo)
        {
            startPoint = currentWeapon.shootingPoints.position;
            endPoint = hitInfo.point;
            //lineRenderer.SetPosition(0,startPoint);
            //lineRenderer.SetPosition(1, endPoint);

            Vector2 normal = hitInfo.normal;
            float angle = Mathf.Atan2(normal.y,normal.x)* Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0,0,angle);
            Instantiate(currentWeapon.hitEffectPrefab, hitInfo.point, rotation);
            
            EnemyStats enemyStats = hitInfo.collider.GetComponent<EnemyStats>();
            IDamageable damageableObject = hitInfo.collider.GetComponent<IDamageable>();

            if(enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeapon.damage);
            }

            else if(damageableObject != null)
            {
                damageableObject.TakeDamage(currentWeapon.damage);
            }

            Debug.Log("We hit something");
        }
        else
        {
            startPoint = currentWeapon.shootingPoints.position;
            endPoint = currentWeapon.shootingPoints.position+direction*10;
            //lineRenderer.SetPosition(0,startPoint);
            //lineRenderer.SetPosition(1, endPoint);
            Debug.Log("We hit nothing");
        }
        currentWeapon.currentAmmo -=1;
        StartCoroutine(ShootDelay());
        StartCoroutine(ResetShootLine());
        OnUpdateAmmo?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    public void AddStorageAmmo(string ID, int ammoToAdd)
    {
        foreach(Weapon weapon in player.listToSaveAndLoad)
        {
            if(weapon.ID == ID)
            {
                weapon.storageAmmo += ammoToAdd;
                OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
                break;
            }
        }
    }

    private IEnumerator ShootDelay()
    {
        shootCooldownOver = false;
        yield return new WaitForSeconds(currentWeapon.recoilTime);
        currentWeapon.transform.localPosition = player.defaultWeaponVectorPose;
        yield return new WaitForSeconds(currentWeapon.shootCooldown-currentWeapon.recoilTime);
        shootCooldownOver = true;
    }
    private IEnumerator ResetShootLine()
    {
        isShootingLineActive=true;
        yield return new WaitForSeconds(currentWeapon.visibleLineTime);
        lineRenderer.positionCount =0;
        isShootingLineActive=false;
    }
    void Update()
    {
        if(shootButtonHeld && currentWeapon.isAutomatic && shootCooldownOver)
        {            
            Shoot();
        }
        if(isShootingLineActive)
        {
            lineRenderer.SetPosition(0,currentWeapon.shootingPoints.position);
            lineRenderer.SetPosition(1,endPoint);
        }
    }
}
