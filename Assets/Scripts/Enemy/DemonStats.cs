using UnityEngine;

public class DemonStats : EnemyStats
{
    [SerializeField] protected EnemySimpleStateMachine enemyStateMachine;
    protected override void DamageProcess()
    {
        base.DamageProcess();
    }

    protected override void DeathProcess()
    {
        enemyStateMachine.ChangeState(EnemySimpleStateMachine.EnemyState.Death);
    }

}
