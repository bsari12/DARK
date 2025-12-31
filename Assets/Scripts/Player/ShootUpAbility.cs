using UnityEngine;
using UnityEngine.InputSystem;

public class ShootUpAbility : BaseAbility
{
    public InputActionReference shootUpRef;
    private Weapon currentWeapon;
    private bool shootUpActivated;
    public float airSpeed;

    void OnEnable()
    {
        shootUpRef.action.performed += TryToShootUp;
        shootUpRef.action.canceled += StopShootUp;
    }
    void OnDisable()
    {
        shootUpRef.action.performed -= TryToShootUp;
        shootUpRef.action.canceled -= StopShootUp;
    }
    protected override void Initialization()
    {
        base.Initialization();
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
    }
    public override void EnterAbility()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        player.setUpShootPos();
    }
    public override void ExitAbility()
    {
        shootUpActivated = false;
        player.SetStandShootPos();
    }
    public override void ProcessAbility()
    {
        if(!linkedPhysics.grounded)
        linkedPhysics.rb.linearVelocity = new Vector2(linkedInput.horizontalInput*airSpeed, linkedPhysics.rb.linearVelocityY);
        else   
            linkedPhysics.rb.linearVelocity = Vector2.zero;
    }
    private void TryToShootUp(InputAction.CallbackContext value)
    {
        if(!isPermitted || currentWeapon == null)
            return;
        if(linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash
        || linkedStateMachine.currentState == PlayerStates.State.WallJump
        || linkedStateMachine.currentState == PlayerStates.State.WallSlide
        || linkedStateMachine.currentState == PlayerStates.State.Crouch
        || linkedStateMachine.currentState == PlayerStates.State.Reload
        || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        linkedStateMachine.ChangeState(PlayerStates.State.ShootUp);
        shootUpActivated = true;
        if(linkedPhysics.grounded)
            linkedPhysics.ResetVelocity();
    }

    private void StopShootUp(InputAction.CallbackContext value)
    {
        if(shootUpActivated == false)
            return;
        
        if(linkedPhysics.grounded)
        {
            if(linkedInput.horizontalInput != 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
        shootUpActivated = false;

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool("ShootUp", linkedStateMachine.currentState == PlayerStates. State.ShootUp);
    }
}
