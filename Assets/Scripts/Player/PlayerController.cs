using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMovement currentMovement;
    private Rigidbody rb;
    private PlayerAction action;
    public delegate void Landing();
    public event Landing OnLand;
    [Header("Player Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float somersaultDuration;
    [SerializeField] private float wallRunDuration;
    [SerializeField] private float airControlFactor;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float dashForce;
    [SerializeField] private float rollSpeed;
    [Header("Ground Settings")]
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform feetCheck;
    [SerializeField] private Transform headCheck;
    [Header("WallRun Settings")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float wallDistance;
    [Header("Crouch Settings")]
    [SerializeField] private Mesh sphere;
    [SerializeField] private Mesh capsule;
    private RegularMovement regularMovement;
    private Jumping jumping;
    private Somersault somersault;
    private WallRun wallRun;
    private Dash dash;
    private Crouching crouching;
    public float GroundDistance => groundDistance;
    public float WallDistance => wallDistance;
    public LayerMask GroundMask => groundMask;
    public LayerMask WallMask => wallMask;
    public float RunSpeed => runSpeed;
    public float WalkSpeed => walkSpeed;
    public float JumpForce => jumpForce;
    public float RotationSpeed => rotationSpeed;
    public float SomersaultDuration => somersaultDuration;
    public float WallRunDuration => wallRunDuration;
    public float AirControlFactor => airControlFactor;
    public float JumpHeight => jumpHeight;
    public float DashForce => dashForce;
    public float RollSpeed => rollSpeed;
    public Mesh Sphere => sphere;
    public Mesh Capsule => capsule;
    public Transform GroundCheck => groundCheck;
    public Transform FeetCheck => feetCheck;
    public Transform HeadCheck => headCheck;
    public RegularMovement RegularMovement => regularMovement;
    public Jumping Jumping => jumping;
    public Somersault Somersault => somersault;
    public WallRun WallRun => wallRun;
    public Dash Dash => dash;
    public Crouching Crouching => crouching;
    public IMovement CurrentMovement => currentMovement;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        action = GetComponent<PlayerAction>();
        regularMovement = new RegularMovement(rb, transform, this, action);
        jumping = new Jumping(rb, transform, this, action);
        somersault = new Somersault(rb, transform, this, action);
        wallRun = new WallRun(rb, transform, this, action);
        dash = new Dash(rb, transform, this, action);
        crouching = new Crouching(rb, transform, this, action);
        SetMovement(regularMovement);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnLand?.Invoke();
        }
    }
    void Update()
    {
        currentMovement?.UpdateMovement();
    }
    private void FixedUpdate()
    {
        currentMovement?.FixedUpdateMovement();
    }

    public void SetMovement(IMovement newMovement)
    {
        currentMovement?.ExitMovement();
        currentMovement = newMovement;
        currentMovement?.EnterMovement();
        Debug.Log(currentMovement.ToString());
    }
}
