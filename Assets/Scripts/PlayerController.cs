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
    [Header("Ground Settings")]
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    public float GroundDistance => groundDistance;
    public LayerMask GroundMask => groundMask;
    public float RunSpeed => runSpeed;
    public float WalkSpeed => walkSpeed;
    public float JumpForce => jumpForce;
    public Transform GroundCheck => groundCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        action = GetComponent<PlayerAction>();
        SetMovement(new RegularMovement(rb, transform, this, action));
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
    }
}
