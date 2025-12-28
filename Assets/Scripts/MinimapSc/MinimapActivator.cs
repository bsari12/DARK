using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapActivator : MonoBehaviour
{
    public InputActionReference miniMapActivatorRef;
    private Player player;
    private bool mapActivated = false;
    [SerializeField] private CanvasGroup minimapCanvasGroup;

    void Start()
    {
        player = GetComponent<Player>();
    }
    void OnEnable()
    {
        miniMapActivatorRef.action.performed += TryToOpenMinimap;
    }
    void OnDisable()
    {
        miniMapActivatorRef.action.performed -= TryToOpenMinimap;
    }

    private void TryToOpenMinimap(InputAction.CallbackContext value)
    {
        if(mapActivated)
        {
            mapActivated = !mapActivated;
            minimapCanvasGroup.alpha = 0;
            player.gatherInput.DisableMinimap();
            if(player.playerStats.GetCurrentHealth()>0)
                player.gatherInput.EnablePlayerMap();
        }
        else
        {
            if(player.playerStats.GetCurrentHealth()<=0)
                return;

            mapActivated = !mapActivated;
            minimapCanvasGroup.alpha = 1;
            player.gatherInput.DisablePlayerMap();
            player.gatherInput.EnableMinimap();
        }
    }





}
