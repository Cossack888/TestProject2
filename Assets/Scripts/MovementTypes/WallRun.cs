using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class WallRun : MovementType
{
    private float duration;
    private float timeElapsed = 0.0f;
    private Vector3 wallNormal;
    private Vector3 wallRunDirection;
    private bool isWallRunning = false;

    public WallRun(Rigidbody rb, Transform transform, PlayerController controller, PlayerAction action)
        : base(rb, transform, controller, action)
    {
    }

    public override void EnterMovement()
    {
        timeElapsed = 0f;
        duration = playerController.WallRunDuration;

        playerRigidbody.useGravity = false;
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);

        playerRigidbody.AddForce(Vector3.up * playerController.JumpForce * 4, ForceMode.Impulse);

        if (DetectWall(out wallNormal))
        {

            if (Physics.Raycast(playerTransform.position, -playerTransform.forward, playerController.WallDistance, playerController.WallMask))
            {
                wallRunDirection = Vector3.Cross(wallNormal, Vector3.up).normalized;
            }
            else if (Physics.Raycast(playerTransform.position, playerTransform.forward, playerController.WallDistance, playerController.WallMask))
            {
                wallRunDirection = Vector3.Cross(Vector3.up, wallNormal).normalized;
            }
            RotatePlayerToWallRunDirection();
            isWallRunning = true;
        }
        else
        {
            EndWallRun();
        }
    }



    public override void FixedUpdateMovement()
    {
        if (isWallRunning)
        {
            if (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                if (IsWallRunning())
                {
                    playerRigidbody.velocity = wallRunDirection * playerController.WalkSpeed;
                    RotatePlayerToWallRunDirection();
                }
                else
                {
                    EndWallRun();
                }
            }
            else
            {
                EndWallRun();
            }
        }
    }

    private bool DetectWall(out Vector3 wallNormal)
    {
        wallNormal = Vector3.zero;
        RaycastHit hit;
        bool wallOnLeft = Physics.Raycast(playerTransform.position, -playerTransform.forward, out hit, playerController.WallDistance, playerController.WallMask);
        if (wallOnLeft)
        {
            wallNormal = hit.normal;
            return true;
        }
        bool wallOnRight = Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, playerController.WallDistance, playerController.WallMask);
        if (wallOnRight)
        {
            wallNormal = hit.normal;
            return true;
        }
        return false;
    }

    private bool IsWallRunning()
    {
        return StuckToLeftSide() || StuckToRightSide();
    }

    private void RotatePlayerToWallRunDirection()
    {
        if (wallRunDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(wallRunDirection, Vector3.forward);
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation * Quaternion.Euler(0f, 90f, 90f), playerController.RotationSpeed * Time.deltaTime);
        }

    }

    private void EndWallRun()
    {
        playerRigidbody.useGravity = true;
        playerController.SetMovement(playerController.RegularMovement);
        isWallRunning = false;
    }

    public override void ExitMovement()
    {
        playerRigidbody.useGravity = true;
    }
}
