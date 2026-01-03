using UnityEngine;

public class PatrollPhysics : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Detect Ground and Walls")]
    [SerializeField] private float checkRadius;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatToDetect;
    public bool groundDetected;
    public bool wallDetected;

    [Header("Colliders")]
    [SerializeField] private BoxCollider2D attackDetectionCol;
    [SerializeField] private PolygonCollider2D attackCol;
    [SerializeField] private PolygonCollider2D statsCol;
    public bool inAttackRange;

    [Header("Player Ahead")]
    [SerializeField] private Transform frontCheckPoint;
    [SerializeField] private float rayFrontCheckLength;
    public bool playerAhead;

    [Header("Player Behing")]
    [SerializeField] private Transform backCheckPoint;
    [SerializeField] private float rayBehindCheckLength;
    public bool playerBehind;
    public bool canCheckBehind = true;
    [SerializeField] private LayerMask playerDetectMask;

    private void FixedUpdate()
    {
        groundDetected = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, whatToDetect);
        wallDetected = Physics2D.OverlapCircle(wallCheckPoint.position, checkRadius, whatToDetect);
        playerAhead = Physics2D.Raycast(frontCheckPoint.position, transform.right, rayFrontCheckLength, playerDetectMask);
        if (canCheckBehind)
        {
            playerBehind = Physics2D.Raycast(backCheckPoint.position, transform.right * (-1), rayBehindCheckLength, playerDetectMask);
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, checkRadius);	
        Gizmos.DrawLine(frontCheckPoint.position, frontCheckPoint.position + transform.right * rayFrontCheckLength);
        Gizmos.DrawLine(backCheckPoint.position, backCheckPoint.position + transform.right * (-1) * rayBehindCheckLength);
    }

    public void NegateForces()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void ActivateAttackCol()
    {
        attackCol.enabled = true;
    }

    public void DeactivateAttackCol()
    {
        attackCol.enabled = false;
    }

    public void DeathColliderDeactivation()
    {
        DeactivateAttackCol();
        attackDetectionCol.enabled = false;
        statsCol.enabled = false;
    }
}
