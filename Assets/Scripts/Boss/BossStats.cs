using UnityEngine;

public class BossStats : EnemyStats
{
    [SerializeField] private BossStateMachine bossStateMachine;

    protected override void DamageProcess()
    {
        
    }

    protected override void DeathProcess()
    {
        bossStateMachine.ChangeState(BossStateMachine.BossState.Death); 
    }

}
