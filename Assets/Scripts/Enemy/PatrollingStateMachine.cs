using UnityEngine;

public class PatrollingStateMachine : EnemySimpleStateMachine
{
    [SerializeField] private PatrollPhysics patrollPhysics;

    [Header("IDLE STATE")]
    [SerializeField] private string idleAnimationName;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    private float idleStateTimer;

    [Header("MOVE STATE")]
    [SerializeField] private string moveAnimationName;
    [SerializeField] private float speed;
    [SerializeField] private float maxMoveTime;
    [SerializeField] private float minMoveTime;
    [SerializeField] private float minimumTurnDelay;
    private float moveStateTimer;
    private float turnCooldown;

    [Header("ATTACK STATE")]
    [SerializeField] private string attackAnimationName;

    [Header("DEATH STATE")]
    [SerializeField] private string deathAnimationName;



    #region IDLE
    public override void EnterIdle()
    {
        anim.Play(idleAnimationName);
        idleStateTimer = Random.Range(minIdleTime, maxIdleTime);
        patrollPhysics.NegateForces();
    }

    public override void UpdateIdle()
    {
        idleStateTimer -= Time.deltaTime;
        if(idleStateTimer <= 0)
        {
            ChangeState(EnemyState.Move);
        }
        if(patrollPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }

    }

    public override void ExitIdle()
    {
        //do smth
    }


    #endregion

    #region MOVE

    public override void EnterMove()
    {
        anim.Play(moveAnimationName);
        moveStateTimer = Random.Range(minMoveTime, maxMoveTime);
    }

    public override void UpdateMove()
    {
        moveStateTimer-= Time.deltaTime;
        if(moveStateTimer <=0)
            ChangeState(EnemyState.Idle);

        if(turnCooldown > 0)
            turnCooldown -=Time.deltaTime;
            
        if(patrollPhysics.wallDetected || patrollPhysics.groundDetected == false)
        {
            if(turnCooldown > 0)
                return;
            ForceFlip();
            speed*= -1;
            turnCooldown = minimumTurnDelay;
        }
        if(patrollPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    public override void FixUpdateMove()
    {
        patrollPhysics.rb.linearVelocity = new Vector2(speed, patrollPhysics.rb.linearVelocityY);
    }


    #endregion

    #region ATTACK

    public override void EnterAttack()
    {
        anim.Play(attackAnimationName);
        patrollPhysics.NegateForces();
    }

    public void EndOfAttack()
    {
        if(patrollPhysics.inAttackRange)
        {
            anim.Play(attackAnimationName, 0, 0);
        }
        else
        {
            ChangeState(previousState);
        }
    }
    #endregion

    #region DEATH

    public override void EnterDeath()
    {
        anim.Play(deathAnimationName);
        patrollPhysics.DeathColliderDeactivation();
        patrollPhysics.NegateForces();
    }

    #endregion



}
