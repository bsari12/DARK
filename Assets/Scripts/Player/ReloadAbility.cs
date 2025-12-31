using UnityEngine;
using UnityEngine.InputSystem;

public class ReloadAbility : BaseAbility
{
    public InputActionReference reloadActionRef;
    private Weapon currentWeapon;

    protected override void Initialization()
    {
        base.Initialization();
    }

    public override void EnterAbility()
    {
        base.EnterAbility();
    }
    void OnEnable()
    {
        reloadActionRef.action.performed += TryToReload;
    }
    
    void OnDisable()
    {
        reloadActionRef.action.performed -= TryToReload;
    }

    private void TryToReload(InputAction.CallbackContext value)
    {
        if(!isPermitted || currentWeapon == null)
            return;
        
        if(linkedPhysics.grounded == false || linkedStateMachine.currentState == PlayerStates.State.Ladders || linkedStateMachine.currentState == PlayerStates.State.Dash || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if(currentWeapon.ReloadCheck()== false || currentWeapon.isReloading)
            return;

            currentWeapon.Reload();
            Shooting.OnUpdateAmmo?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    
}
