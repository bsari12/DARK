using UnityEngine;

public class PhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;
    [Header("Ground")]
    [SerializeField] private float groundRayDistance;
    [SerializeField] private Transform leftGroundPoint;
    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private LayerMask whatToDetect;
    public bool grounded;
    private RaycastHit2D hitInfoLeft;
    private RaycastHit2D hitInfoRight;        

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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        grounded = CheckGround();
    }
}
