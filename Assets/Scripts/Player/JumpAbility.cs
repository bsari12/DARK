using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAbility : BaseAbility
{
    public InputActionReference jumpActionRef;

    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;
    [SerializeField] private float minimumAirtime;
    private float startMinimumAirTime;
    private string jumpAnimParameterName = "Jump";
    private string ySpeedAnimParameterName = "ySpeed";
    private int jumpParameterID;
    private int ySpeedParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        startMinimumAirTime = minimumAirtime;
        jumpParameterID = Animator.StringToHash(jumpAnimParameterName);
        ySpeedParameterID = Animator.StringToHash(ySpeedAnimParameterName);
    }

    private void OnEnable()
    {
        jumpActionRef.action.performed += TryToJump;
        jumpActionRef.action.canceled += StopJump;
    }
    private void OnDisable()
    {
        jumpActionRef.action.performed -= TryToJump;
        jumpActionRef.action.canceled -= StopJump;

    }
    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirtime -= Time.deltaTime;
        if(linkedPhysics.grounded && minimumAirtime < 0)
        {
            if(linkedInput.horizontalInput !=0)
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        if(!linkedPhysics.grounded && linkedPhysics.wallDetected)
        {
            if (linkedPhysics.rb.linearVelocityY < 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
            }
        }
    }
    public override void ProcessFixedAbility()
    {
        if (!linkedPhysics.grounded)
        {
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed* linkedInput.horizontalInput, linkedPhysics.rb.linearVelocityY);             
        }
    }
    private void TryToJump(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if(linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            //linkedPhysics.EnableGravity();
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed* linkedInput.horizontalInput, 0); 
            minimumAirtime = startMinimumAirTime;
            return;
        }

        if(linkedPhysics.coyoteTimer > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed* linkedInput.horizontalInput, jumpForce); 
            minimumAirtime = startMinimumAirTime;
            linkedPhysics.coyoteTimer = -1;
        }
    }
    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("Stop jump");
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParameterID, linkedStateMachine.currentState == PlayerStates.State.Jump || linkedStateMachine.currentState==PlayerStates.State.WallJump);
        linkedAnimator.SetFloat(ySpeedParameterID, linkedPhysics.rb.linearVelocityY);
    }
}
