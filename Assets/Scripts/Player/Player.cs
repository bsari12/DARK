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
    public ItemType currentWeaponType;

    [Header("Primary Weapon")]
    public GameObject primaryWeaponPrefab;


    [Header("Weapon Positions")]
    [SerializeField] private Transform currentShootingPos;
    [SerializeField] private Transform standingShootPos;
    [SerializeField] private Transform crouchShootPos;


    void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities =GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
        currentShootingPos = standingShootPos;
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
        Debug.Log("Current state is: "+ stateMachine.currentState);
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

    public void SetStandShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = standingShootPos;
            currentWeaponPrefab.transform.position = standingShootPos.position;
        }
    }

    public void SetCrouchShootPos()
    {
        if(currentWeaponType == ItemType.PrimaryWeapon)
        {
            currentShootingPos = crouchShootPos;
            currentWeaponPrefab.transform.position = crouchShootPos.position;
        }
    }

    
}
