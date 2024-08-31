using System;
using Unity.VisualScripting;
using UnityEngine;

public class RegularMovement : MovementType
{
    public RegularMovement(Rigidbody rb, Transform transform, PlayerController controller, PlayerAction action) : base(rb, transform, controller, action)
    {
        action.OnJump += Jump;
        action.OnParkour += WallRun;
    }
    private Vector3 movement;
    public override void EnterMovement()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerController.GroundCheck.position, Vector3.down, out hit, Mathf.Infinity, playerController.GroundMask))
        {
            playerTransform.position = new Vector3(playerTransform.position.x, hit.point.y + playerController.GroundDistance + Vector3.Distance(playerTransform.position, playerController.GroundCheck.position) / 2, playerTransform.position.z);
        }
    }
    public override void UpdateMovement()
    {
        movement = new Vector3(playerAction.Movement.x, 0f, playerAction.Movement.y);

    }
    public override void FixedUpdateMovement()
    {
        playerRigidbody.velocity = playerController.WalkSpeed * movement;
        HandleRotation();
    }
    public void Jump()
    {
        if (IsGrounded())
        {
            playerController.SetMovement(playerController.Jumping);
        }
    }
    public void WallRun()
    {
        if (StuckToLeftSide() || StuckToRightSide())
        {
            playerController.SetMovement(playerController.WallRun);
        }
    }
    public void HandleRotation()
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.forward);
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation * Quaternion.Euler(0f, 90f, 90f), playerController.RotationSpeed * Time.deltaTime);
        }
    }
    ~RegularMovement()
    {
        playerAction.OnJump -= Jump;
        playerAction.OnParkour -= WallRun;
    }
}