using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera minimapCamera;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dragSpeed;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;


    public InputActionReference moveActionRef;
    public InputActionReference zoomActionRef;
    public InputActionReference middleClickRef;
    public InputActionReference deltaActionRef;

    void Update()
    {
        HandleZoom();
        HandleMove();

    }
    private void HandleZoom()
    {
        float scroll = zoomActionRef.action.ReadValue<float>();
        if(scroll !=0)
        {
            minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
        }
    }

    private void HandleMove()
    {
        float zoomFactor = minimapCamera.orthographicSize;
        Vector2 moveInput = moveActionRef.action.ReadValue<Vector2>();
        if(moveInput != Vector2.zero)
        {
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0) * zoomFactor * moveSpeed * Time.deltaTime;
            minimapCamera.transform.position += move;
        }
        if(middleClickRef.action.IsPressed())
        {
            Vector2 delta = deltaActionRef.action.ReadValue<Vector2>();
            if(delta != Vector2.zero)
            {
                Vector3 dragMove = new Vector3(-delta.x, -delta.y, 0) * zoomFactor * dragSpeed * Time.deltaTime;
                minimapCamera.transform.position += dragMove;
            }

        }
    }











}
