using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMovement currentMovement;
    private Rigidbody rb;
    private PlayerAction action;
    [Header("Player Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float somersaultDuration;
    [SerializeField] private float wallRunDuration;
    [Header("Ground Settings")]
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [Header("WallRun Settings")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float wallDistance;

    private RegularMovement regularMovement;
    private Jumping jumping;
    private Somersault somersault;
    private WallRun wallRun;
    [SerializeField] TMP_Text velocityMeter;
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
    public Transform GroundCheck => groundCheck;
    public RegularMovement RegularMovement => regularMovement;
    public Jumping Jumping => jumping;
    public Somersault Somersault => somersault;
    public WallRun WallRun => wallRun;
    public IMovement CurrentMovement => currentMovement;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        action = GetComponent<PlayerAction>();
        regularMovement = new RegularMovement(rb, transform, this, action);
        jumping = new Jumping(rb, transform, this, action);
        somersault = new Somersault(rb, transform, this, action);
        wallRun = new WallRun(rb, transform, this, action);
        SetMovement(regularMovement);
    }

    void Update()
    {
        currentMovement?.UpdateMovement();
        velocityMeter.text = rb.velocity.ToString();
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
