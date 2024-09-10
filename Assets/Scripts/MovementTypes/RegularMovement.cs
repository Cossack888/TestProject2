using UnityEngine;

public class RegularMovement : MovementType
{
    public RegularMovement(Rigidbody rb, Transform transform, PlayerController controller, PlayerAction action) : base(rb, transform, controller, action)
    {
        action.OnJumpGlobal += Jump;
        action.OnDashGlobal += Dash;
    }
    public override void UpdateMovement()
    {
        movement = new Vector3(playerAction.Movement.x, 0f, playerAction.Movement.y);
        float slowMultiplier = playerAction.IsSprinting ? 5 : 10;
        momentum.ModifyMomentum(-Time.deltaTime / slowMultiplier);
        if (playerAction.IsCrouching && momentum.CurrentMomentum > 0)
        {
            playerController.SetMovement(playerController.Crouching);
        }
    }
    public override void FixedUpdateMovement()
    {
        Vector3 targetVelocity = (playerAction.IsSprinting ? playerController.RunSpeed + momentum.CurrentMomentum : playerController.WalkSpeed + momentum.CurrentMomentum) * new Vector3(movement.x, 0, movement.z);
        targetVelocity.y = playerRigidbody.velocity.y;

        playerRigidbody.velocity = targetVelocity;

        HandleRotation();
    }
    public void Jump()
    {
        if (IsGrounded())
        {
            playerController.SetMovement(playerController.Jumping);
        }
    }
    public void Dash()
    {
        if (playerController.CurrentMovement == this)
        {
            playerController.SetMovement(playerController.Dash);
        }
    }

    ~RegularMovement()
    {
        playerAction.OnJumpGlobal -= Jump;
        playerAction.OnDashGlobal -= Dash;
    }
}