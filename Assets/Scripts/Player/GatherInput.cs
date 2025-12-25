using UnityEngine;
using UnityEngine.InputSystem;


public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;
    public InputActionReference verticalActionRef;
    
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;


    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {

        playerMap.Disable();
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
        verticalInput = verticalActionRef.action.ReadValue<float>();
        Debug.Log("Horizontal Input: "+ horizontalInput);
    }

    public void DisablePlayerMap()
    {
        playerMap.Disable();
    }
}
