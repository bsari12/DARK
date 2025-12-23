using UnityEngine;
using System.Collections;

public class KnockBackAbility : BaseAbility
{
    public IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        linkedStateMachine.ChangeState(PlayerStates.State.KnockBack);
        linkedPhysics.ResetVelocity();
        if(transform.position.x >= enemyObject.transform.position.x)
        {
            linkedPhysics.rb.linearVelocity = force;
        }
        else
        {
            linkedPhysics.rb.linearVelocity = new Vector2(-force.x, force.y);
        }
        yield return new WaitForSeconds(duration);
        
        if(player.playerStats.GetCurrentHealth() >0)
        {
            if(linkedPhysics.grounded)
            {
                if(linkedInput.horizontalInput !=0)
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
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Death);
        }

    }

}
