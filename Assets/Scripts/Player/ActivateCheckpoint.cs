using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateCheckpoint : MonoBehaviour
{
    public InputActionReference ActivateCheck;
    [HideInInspector]
    public Checkpoint checkPoint;

    void OnEnable()
    {
        ActivateCheck.action.performed += TryToActivate;
    }
    void OnDisable()
    {
        ActivateCheck.action.performed -= TryToActivate;
    }
    private void TryToActivate(InputAction.CallbackContext value)
    {
        if(checkPoint == null)
            return;
        // activate
        checkPoint.Activate();
    }
}
