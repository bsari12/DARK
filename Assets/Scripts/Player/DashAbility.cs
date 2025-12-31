using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : BaseAbility
{
    public InputActionReference dashActionRef;
    [SerializeField]private float dashForce;
    [SerializeField] private float maxDashDuration;
    private float dashTimer;

    private string dashAnimParameterName = "Dash";
    private int dashParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        dashParameterID =Animator.StringToHash(dashAnimParameterName);
    }

    void OnEnable()
    {
        dashActionRef.action.performed += TryToDash;
    }
    void OnDisable()
    {
        dashActionRef.action.performed -= TryToDash;
    }

    public override void EnterAbility()
    {
        //player.playerStats.DisableDamage();
        player.playerStats.DisableStatsCollider();
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity();
        linkedPhysics.ResetVelocity();
        //player.playerStats.EnableDamage();
        player.playerStats.EnableStatsCollider();
    }

    private void TryToDash(InputAction.CallbackContext value)
    {
        if(!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack )
            return;
        
        if(linkedStateMachine.currentState == PlayerStates.State.Dash || linkedPhysics.wallDetected || linkedStateMachine.currentState == PlayerStates.State.Crouch || linkedStateMachine.currentState == PlayerStates.State.Reload)
            return;


        linkedStateMachine.ChangeState(PlayerStates.State.Dash);
        linkedPhysics.DisableGravity();
        linkedPhysics.ResetVelocity();
        if(player.facingRight)
            linkedPhysics.rb.linearVelocityX = dashForce;
        else
            linkedPhysics.rb.linearVelocityX = -dashForce;
        
        dashTimer= maxDashDuration;
    }

    public override void ProcessAbility()
    {
        dashTimer -= Time.deltaTime;
        if(linkedPhysics.wallDetected)
            dashTimer = -1;

        if(dashTimer <= 0)
        {
            if(linkedPhysics.grounded)
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(dashParameterID, linkedStateMachine.currentState ==PlayerStates.State.Dash);
    }

}
