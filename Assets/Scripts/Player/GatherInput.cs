using UnityEngine;
using UnityEngine.InputSystem;


public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;

    private float horizontalInput;


    void OnEnable()
    {
        jumpActionRef.action.performed += TryToJump;
        jumpActionRef.action.canceled += StopJump;
    }
    void OnDisable()
    {
        jumpActionRef.action.performed -= TryToJump;
        jumpActionRef.action.canceled -= StopJump;
        playerMap.Disable();
    }
    private void TryToJump(InputAction.CallbackContext value)
    {
        Debug.Log("Jump");
    }
    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("Stop jump");
    }

    void Start()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");
        playerMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = moveActionRef.action.ReadValue<float>();
        Debug.Log("Horizontal Input: "+ horizontalInput);
    }
}
