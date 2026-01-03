using System.IO;
using UnityEngine;

public class DeathAbility : BaseAbility
{
    public override void EnterAbility()
    {
        SpawnMode.spawnFromCheckPoint = true;
        player.gatherInput.DisablePlayerMap();
        linkedPhysics.ResetVelocity();

        linkedAnimator.SetBool("Death", true);
        
        Invoke(nameof(ResetGame), 2f);
        player.DeactivateCurrentWeapon();
    }

    public override void ExitAbility()
    {
        player.ActivateCurrentWeapon();
    }

    public void ResetGame()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if(File.Exists(loadPath))
        {
            CheckpointData checkData = new CheckpointData();
            SaveLoadManager.instance.Load(checkData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            LevelManager.instance.LoadLevelString(checkData.sceneToLoad);
        }
        else
        {
            LevelManager.instance.RestartLevel();
        }
        
    }


}



