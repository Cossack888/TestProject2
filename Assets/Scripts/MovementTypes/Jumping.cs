using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Jumping : MovementType
{
    public Jumping(Rigidbody rb, Transform transform, PlayerController controller, PlayerAction action) : base(rb, transform, controller, action)
    {
    }
    private bool falling;
    private Vector3 previousVelocity;
    private Vector3 movement;

    public override void EnterMovement()
    {
        previousVelocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        playerRigidbody.velocity = previousVelocity / 2;
        playerRigidbody.AddForce(Vector3.up * playerController.JumpForce, ForceMode.Impulse);
    }
    public override void UpdateMovement()
    {
        falling = playerRigidbody.velocity.y < 0;
        movement = new Vector3(playerAction.Movement.x, 0f, playerAction.Movement.y);
        if (IsGrounded() && falling)
        {
            playerController.SetMovement(playerController.RegularMovement);
        }
    }

    public void HandleRotation()
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.forward);
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0);
            playerTransform.rotation = targetRotation * Quaternion.Euler(0f, 90f, 90f);
        }
    }
    public override void FixedUpdateMovement()
    {
        playerRigidbody.velocity = new Vector3(playerController.WalkSpeed / 2 * movement.x, playerRigidbody.velocity.y, playerController.WalkSpeed / 2 * movement.z);
        HandleRotation();
    }
}
