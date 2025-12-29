using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    public InputActionReference shootActionRef;
    public Weapon currentWeapon;
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
        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite,currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    void OnEnable()
    {
        shootActionRef.action.performed += TryToShoot;
        shootActionRef.action.canceled += StopShooting;
    }
    void OnDisable()
    {
        shootActionRef.action.performed -= TryToShoot;
        shootActionRef.action.canceled -= StopShooting;
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

    private void Shoot()
    {
        if(currentWeapon.currentAmmo<=0)
            return;
        lineRenderer.positionCount = 2;
        Vector3 direction = currentWeapon.shootingPoints.right;
        RaycastHit2D hitInfo = Physics2D.Raycast(currentWeapon.shootingPoints.position,direction,Mathf.Infinity,whatToHit);
        if (hitInfo)
        {
            startPoint = currentWeapon.shootingPoints.position;
            endPoint = hitInfo.point;
            //lineRenderer.SetPosition(0,startPoint);
            //lineRenderer.SetPosition(1, endPoint);
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

    private IEnumerator ShootDelay()
    {
        shootCooldownOver = false;
        yield return new WaitForSeconds(currentWeapon.shootCooldown);
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
