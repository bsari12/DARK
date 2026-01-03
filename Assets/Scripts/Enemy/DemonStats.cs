using UnityEngine;

public class DemonStats : EnemyStats
{
    protected override void DamageProcess()
    {
        base.DamageProcess();
    }

    protected override void DeathProcess()
    {
        enemyStateMachine.ChangeState(EnemySimpleStateMachine.EnemyState.Death);
    }

}
