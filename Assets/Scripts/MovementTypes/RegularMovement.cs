using System;
using UnityEngine;

public class RegularMovement : MovementType
{
    public RegularMovement(Rigidbody rb, Transform transform, PlayerController controller, PlayerAction action) : base(rb, transform, controller, action)
    {
        action.OnJump += Jump;
    }
    private Vector3 movement;
    public override void UpdateMovement()
    {
        movement = new Vector3(playerAction.Movement.x, 0f, playerAction.Movement.y);

    }
    public override void FixedUpdateMovement()
    {
        playerRigidbody.MovePosition(playerTransform.position + playerController.WalkSpeed * Time.deltaTime * movement);
    }
    public void Jump()
    {
        if (IsGrounded())
            playerRigidbody.AddForce(Vector3.up * playerController.JumpForce, ForceMode.Impulse);
    }
    ~RegularMovement()
    {
        playerAction.OnJump -= Jump;
    }
}