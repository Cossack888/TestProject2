using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementType : IMovement
{
    protected Rigidbody playerRigidbody;
    protected Transform playerTransform;
    protected PlayerController playerController;
    protected PlayerAction playerAction;

    public MovementType(Rigidbody rb, Transform transform, PlayerController controller, PlayerAction action)
    {
        playerRigidbody = rb;
        playerTransform = transform;
        playerController = controller;
        playerAction = action;
    }

    public virtual void EnterMovement() { }
    public virtual void UpdateMovement() { }
    public virtual void FixedUpdateMovement() { }
    public virtual void ExitMovement() { }

    protected bool IsGrounded()
    {
        return Physics.Raycast(playerController.GroundCheck.position, Vector3.down, out RaycastHit hit, playerController.GroundDistance, playerController.GroundMask);
    }



}
