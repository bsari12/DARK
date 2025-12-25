using UnityEngine;

public class DeathAbility : BaseAbility
{
    public override void EnterAbility()
    {
        player.gatherInput.DisablePlayerMap();
        linkedPhysics.ResetVelocity();
        if(linkedPhysics.grounded)
            linkedAnimator.SetBool("Death", true);
        else
        {
            // air death animation
        }
    }



    public void ResetGame()
    {
        LevelManager.instance.RestartLevel();
    }


}



