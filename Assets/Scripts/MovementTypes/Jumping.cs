using UnityEngine;

public class Jumping : MovementType
{
    public Jumping(Rigidbody rb, Transform transform, PlayerController controller, PlayerAction action) : base(rb, transform, controller, action)
    {
        action.OnParkourGlobal += Somersault;
        action.OnJumpGlobal += WallRunOrDoubleJump;
        action.OnDashGlobal += Dash;
        controller.OnLand += Landed;
    }
    private float initialYPosition;
    private float targetYPosition;
    private float currentYVelocity;
    private bool isAscending;
    private bool isAtPeak;
    private bool doubleJumped;
    public override void EnterMovement()
    {
        initialYPosition = playerRigidbody.position.y;
        targetYPosition = initialYPosition + playerController.JumpHeight;
        currentYVelocity = 0f;
        isAscending = true;
        isAtPeak = false;
        momentum.ModifyMomentum(-0.1f);
    }
    public override void UpdateMovement()
    {
        if (IsGrounded() && Falling())
        {
            doubleJumped = false;
            Landed();
        }

        movement = new Vector3(playerAction.Movement.x, 0f, playerAction.Movement.y);

        if (!IsGrounded())
        {
            movement *= playerController.AirControlFactor;
        }

    }

    public void WallRunOrDoubleJump()
    {
        if (!IsGrounded())
        {
            if (StuckToLeftSide() || StuckToRightSide())
            {
                WallRun();
            }
            else
            {
                DoubleJump();
            }
        }
    }
    public void Landed()
    {
        if (IsGrounded())
        {
            playerController.SetMovement(playerController.RegularMovement);
        }
    }
    public void WallRun()
    {
        playerController.SetMovement(playerController.WallRun);
    }
    public void Dash()
    {
        if (playerController.CurrentMovement == this)
        {
            playerController.SetMovement(playerController.Dash);
        }
    }

    public override void FixedUpdateMovement()
    {
        Vector3 targetVelocity = playerController.RunSpeed * new Vector3(movement.x, 0, movement.z);
        targetVelocity.y = playerRigidbody.velocity.y;
        if (isAscending)
        {
            currentYVelocity += playerController.JumpForce * Time.deltaTime;
            if (playerRigidbody.position.y >= targetYPosition)
            {
                isAscending = false;
                isAtPeak = true;
            }
        }
        else if (isAtPeak)
        {
            currentYVelocity -= playerController.JumpForce * Time.deltaTime;
            if (currentYVelocity <= 0)
            {
                isAtPeak = false;
            }
        }
        else
        {
            currentYVelocity -= playerController.JumpForce * Time.deltaTime * 2;
        }
        targetVelocity.y = currentYVelocity;
        playerRigidbody.velocity = targetVelocity;
        HandleRotation();
    }
    void DoubleJump()
    {
        if (!doubleJumped)
        {
            doubleJumped = true;
            playerController.SetMovement(this);
        }
    }

    void Somersault()
    {
        if (playerController.CurrentMovement == this && !Falling())
        {
            playerController.SetMovement(playerController.Somersault);
        }
    }

    ~Jumping()
    {
        playerAction.OnParkourGlobal -= Somersault;
        playerAction.OnJumpGlobal -= WallRunOrDoubleJump;
        playerAction.OnDashGlobal -= Dash;
        playerController.OnLand -= Landed;
    }
}