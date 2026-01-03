using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GatherInput gatherInput;
    public StateMachine stateMachine;
    public PhysicsControl physicsControl;

    public Animator anim;
    public PlayerStats playerStats;
    
    private BaseAbility[] playerAbilities;
    public bool facingRight = true;

    [Header("Current Weapon")]
    public GameObject currentWeaponPrefab;
    //public Weapon currentWeapon;
    public ItemType currentWeaponType;

    [Header("Primary Weapon")]
    public GameObject primaryWeaponPrefab;


    [Header("Weapon Positions")]
    [SerializeField] private Transform currentShootingPos;
    [SerializeField] private Transform standingShootPos;
    [SerializeField] private Transform crouchShootPos;
    [SerializeField] private Transform upShootPos;
    [HideInInspector] public Vector3 defaultWeaponVectorPose;

    [Header("Secondary Weapon")]
    public GameObject secondaryWeaponPrefab;

    [Header("Secondary Weapon Positions")]
    [SerializeField] private Transform secondStandingShootPos;
    [SerializeField] private Transform secondCrouchShootPos;
    [SerializeField] private Transform secondUpShootPos;

    public List<Weapon> listToSaveAndLoad = new List<Weapon>();

    void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities =GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
        currentShootingPos = standingShootPos;
        defaultWeaponVectorPose = standingShootPos.localPosition;
    }
    void OnDisable()
    {
        foreach(Weapon weapon in listToSaveAndLoad)
        {
            weapon.SaveWeaponData();
        }
    }
    void Update()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
        Flip();
        //Debug.Log("Current state is: "+ stateMachine.currentState);
    }
    void FixedUpdate()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }
        }    
    }
    public void Flip()
    {
        if(facingRight == true && gatherInput.horizontalInput < 0)
        {
            transform.Rotate(0,180,0);
            facingRight = !facingRight;
        }
        else if(facingRight == false && gatherInput.horizontalInput > 0)
        {
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
        }
    }
    public void ForceFlip()
    {
        transform.Rotate(0,180,0);
        facingRight = !facingRight;
    }
    public void SetWeaponPosition()
    {
        if(stateMachine.currentState == PlayerStates.State.Crouch)
        {
            SetCrouchShootPos();
        }
        if(stateMachine.currentState == PlayerStates.State.ShootUp)
        {
            SetUpShootPos();
        }
        else
        {
            SetStandShootPos();
        }
    }
    public void SetStandShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = standingShootPos;
            currentWeaponPrefab.transform.position = standingShootPos.position;
            
        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondStandingShootPos;
            currentWeaponPrefab.transform.position = secondStandingShootPos.position;
        }
        defaultWeaponVectorPose = currentShootingPos.localPosition;
        SetWeaponRotation(0);
    }

    public void SetCrouchShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = crouchShootPos;
            currentWeaponPrefab.transform.position = crouchShootPos.position;
            
        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondCrouchShootPos;
            currentWeaponPrefab.transform.position = secondCrouchShootPos.position;
        }
        defaultWeaponVectorPose = currentShootingPos.localPosition;
        SetWeaponRotation(0);
    }

    public void SetUpShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = upShootPos;
            currentWeaponPrefab.transform.position = upShootPos.position;
        }
        else if (currentWeaponType == ItemType.SecondaryWeapon)
        {
            currentShootingPos = secondUpShootPos;
            currentWeaponPrefab.transform.position = secondUpShootPos.position;
        }
        defaultWeaponVectorPose = currentShootingPos.localPosition;
        SetWeaponRotation(90);
    }

    public void DeactivateCurrentWeapon()
    {
        currentWeaponPrefab.SetActive(false);
    }

    public void ActivateCurrentWeapon()
    {
        currentWeaponPrefab.SetActive(true);
    }

    private void SetWeaponRotation(float zRotation)
    {
        currentWeaponPrefab.transform.localEulerAngles = new Vector3(0,0,zRotation);
    }
    
}
