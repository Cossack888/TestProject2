using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    private PlayerInput input;
    private PlayerBindings bindings;
    public delegate void PlayerActionPerformed();
    public event PlayerActionPerformed OnJump;

    private Vector2 movementVector;
    public Vector2 Movement => movementVector;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        bindings = new PlayerBindings();
        bindings.Player.Enable();
        bindings.Player.Jump.performed += Jump_performed;
    }
    public void OnDisable()
    {
        bindings.Player.Jump.performed -= Jump_performed;
    }

    public void Jump_performed(InputAction.CallbackContext context)
    {
        if (context.performed) { OnJump?.Invoke(); }

    }
    public void FixedUpdate()
    {
        movementVector = bindings.Player.Movement.ReadValue<Vector2>();
    }


}
