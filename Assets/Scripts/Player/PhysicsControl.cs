using UnityEngine;

public class PhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteSetTime;
    public float coyoteTimer;

    [Header("Interpolation")]
    public RigidbodyInterpolation2D interpolate;
    public RigidbodyInterpolation2D	extrapolate;

    [Header("Ground")]
    [SerializeField] private float groundRayDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private LayerMask whatToDetect;
    public bool grounded;
    private RaycastHit2D hitInfoLeft;
    private RaycastHit2D hitInfoRight;        

    [Header("Wall")]
    [SerializeField] private float wallRayDistance;
    [SerializeField] private Transform wallCheckPointUpper;
    [SerializeField] private Transform wallCheckPointLower;
    public bool wallDetected;
    private RaycastHit2D hitInfoWallUpper;
    private RaycastHit2D hitInfoWallLower;

    private float gravityValue;

    [Header("Colliders")]
    [SerializeField] private Collider2D standColl;
    [SerializeField] private Collider2D crouchColl;

    [Header("Ceiling")]
    [SerializeField] private float ceilingRayDistance;
    [SerializeField] private Transform ceilingCheckPointLeft;
    [SerializeField] private Transform ceilingCheckPointRight;
    public bool ceilingDetected;
    private RaycastHit2D hitInfoCeilingRight;
    private RaycastHit2D hitInfoCeilingLeft;


    public float GetGravity()
    {
        return gravityValue;
    }

    void Start()
    {
        gravityValue = rb.gravityScale;
        coyoteTimer = coyoteSetTime;
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(ceilingCheckPointLeft.position, new Vector3(0,ceilingRayDistance,0),Color.red);
        Debug.DrawRay(ceilingCheckPointRight.position, new Vector3(0,ceilingRayDistance,0),Color.red);
    }

    private bool CheckCeiling()
    {
        hitInfoCeilingLeft = Physics2D.Raycast(ceilingCheckPointLeft.position,transform.right,ceilingRayDistance, whatToDetect);
        hitInfoCeilingRight = Physics2D.Raycast(ceilingCheckPointRight.position,transform.right,ceilingRayDistance, whatToDetect);
        if(hitInfoCeilingLeft || hitInfoCeilingRight)
            return true;

        return false;
    }

    private bool CheckWall()
    {
        hitInfoWallUpper = Physics2D.Raycast(wallCheckPointUpper.position,transform.right,wallRayDistance, whatToDetect);
        hitInfoWallLower = Physics2D.Raycast(wallCheckPointLower.position,transform.right,wallRayDistance, whatToDetect);
        Debug.DrawRay(wallCheckPointUpper.position, new Vector3(wallRayDistance,0,0),Color.red);
        Debug.DrawRay(wallCheckPointLower.position, new Vector3(wallRayDistance,0,0),Color.red);

        if(hitInfoWallUpper || hitInfoWallLower)
            return true;

        return false;
    }



    private bool CheckGround()
    {
        hitInfoLeft = Physics2D.Raycast(leftGroundPoint.position, Vector2.down, groundRayDistance, whatToDetect);
        hitInfoRight = Physics2D.Raycast(rightGroundPoint.position, Vector2.down, groundRayDistance, whatToDetect); 

        Debug.DrawRay(leftGroundPoint.position, new Vector3(0,-groundRayDistance,0),Color.red);
        Debug.DrawRay(rightGroundPoint.position, new Vector3(0,-groundRayDistance,0),Color.red);

        if (hitInfoLeft || hitInfoRight)
            return true;
        
        return false;        
    }

    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityValue;
    }
    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void StandColliders()
    {
        standColl.enabled =true;
        crouchColl.enabled = false;
    }

    public void CrouchColliders()
    {
        standColl.enabled = false;
        crouchColl.enabled = true;
    }

    void Update()
    {
        if (!grounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = coyoteSetTime;
        }
    }
    void FixedUpdate()
    {
        grounded = CheckGround();
        wallDetected = CheckWall();
        ceilingDetected = CheckCeiling();
    }
    public void SetInterpolate()
    {
        rb.interpolation = interpolate;
    }

    public void SetExtrapolate()
    {
        rb.interpolation = extrapolate;
    }
}
