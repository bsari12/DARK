using UnityEngine;

public class WallSlideAbility : BaseAbility
{
    [SerializeField] private float maxSlideSpeed;

    public override void EnterAbility()
    {
        linkedPhysics.rb.linearVelocity = Vector2.zero;
    }
    public override void ProcessAbility()
    {
        if (linkedPhysics.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            return;
        }
        if(player.facingRight && linkedInput.horizontalInput < 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }
        if(!player.facingRight && linkedInput.horizontalInput > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }
        if (!linkedPhysics.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }
    }
    public override void ProcessFixedAbility()
    {
        linkedPhysics.rb.linearVelocityY=Mathf.Clamp(linkedPhysics.rb.linearVelocityY,-maxSlideSpeed,1);
    }



}
