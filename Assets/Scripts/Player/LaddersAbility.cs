using UnityEngine;
using UnityEngine.InputSystem;

public class LaddersAbility : BaseAbility
{
    public InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float setMinLadderTime;
    private float minimumLadderTime;
    private bool climb;
    public bool canGoOnLadder;

    private string ladderParameterName = "Ladder";
    private int ladderParameterID;

    protected override void Initialization()
    {
        base.Initialization();
        ladderParameterID = Animator.StringToHash(ladderParameterName);
        minimumLadderTime = setMinLadderTime;
    }

    void OnEnable()
    {
        ladderActionRef.action.performed += TryToClimb;
        ladderActionRef.action.canceled += TryToClimb;
    }
    void OnDisable()
    {
        ladderActionRef.action.performed -= TryToClimb;
        ladderActionRef.action.canceled -= TryToClimb;
    }

    private void TryToClimb(InputAction.CallbackContext value)
        {
            if (!isPermitted)
                return;
            linkedAnimator.enabled = true;
            if(linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash || !canGoOnLadder)
            {
                return;
            }

            linkedStateMachine.ChangeState(PlayerStates.State.Ladders);
            linkedPhysics.DisableGravity();
            linkedPhysics.ResetVelocity();
            climb = true;
            minimumLadderTime = setMinLadderTime;

        }

    private void StopClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;
        if(linkedStateMachine.currentState == PlayerStates.State.Ladders)
            return;
        linkedPhysics.ResetVelocity();
        linkedAnimator.enabled = false;
    }

    public override void ExitAbility()
    {
        linkedPhysics.EnableGravity(); 
        linkedAnimator.enabled = true; 
        linkedAnimator.speed = 1;     
        climb = false;               
    }

    public override void ProcessAbility()
    {
        if (climb)
        {
            minimumLadderTime -= Time.deltaTime;

            if (linkedInput.verticalInput == 0)
            {
                linkedAnimator.speed = 0; 
            }
            else
            {
                linkedAnimator.speed = 1; 
            }
        }


        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }

        if (canGoOnLadder == false)
        {
            if (linkedPhysics.grounded == false)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
        }

        if (linkedPhysics.grounded && minimumLadderTime <= 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
    }


    public override void ProcessFixedAbility()
    {
        if(climb)
            linkedPhysics.rb.linearVelocity = new Vector2(0, linkedInput.verticalInput*climbSpeed);
    }
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(ladderParameterID,linkedStateMachine.currentState ==PlayerStates.State.Ladders);
    }
}
