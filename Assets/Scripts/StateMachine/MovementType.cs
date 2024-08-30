using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementTypeBase : IMovement
{
    protected Rigidbody playerRigidbody;
    protected Transform playerTransform;
    protected PlayerController playerController;

    public MovementTypeBase(Rigidbody rb, Transform transform, PlayerController controller)
    {
        playerRigidbody = rb;
        playerTransform = transform;
        playerController = controller;
    }

    public virtual void EnterMovement() { }
    public virtual void UpdateMovement() { }
    public virtual void ExitMovement() { }

    protected bool IsGrounded()
    {
        return Physics.Raycast(playerTransform.position, Vector3.down, out RaycastHit hit, playerController.GroundDistance, playerController.GroundMask);
    }
}
