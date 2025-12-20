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
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
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
        if (!isPermitted)
            return;
        if (linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysics.rb.linearVelocity = new Vector2(airSpeed* linkedInput.horizontalInput, jumpForce); 
            minimumAirtime = startMinimumAirTime;
        }
    }
    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("Stop jump");
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParameterID, linkedStateMachine.currentState == PlayerStates.State.Jump);
        linkedAnimator.SetFloat(ySpeedParameterID, linkedPhysics.rb.linearVelocityY);
    }
}
