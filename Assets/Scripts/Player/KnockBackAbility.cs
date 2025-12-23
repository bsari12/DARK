using UnityEngine;
using System.Collections;

public class KnockBackAbility : BaseAbility
{
    private Coroutine currentKnockBack;

    public override void ExitAbility()
    {
        currentKnockBack = null;
        linkedPhysics.ResetVelocity();
    }
    public void StartKnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        if(currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
        else
        {
            // do nothing OR
            StopCoroutine(currentKnockBack);
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
    }

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

        if(player.playerStats.GetCurrentHealth()>0)
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
    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool("KnockBack", linkedStateMachine.currentState == PlayerStates.State.KnockBack);
    }

}
