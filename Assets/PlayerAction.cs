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
    public event PlayerActionPerformed OnParkour;

    private Vector2 movementVector;
    private float scrollAmount;
    public Vector2 Movement => movementVector;
    public float ScrollAmount => scrollAmount;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        bindings = new PlayerBindings();
        bindings.Player.Enable();
        bindings.Player.Jump.performed += Jump_performed;
        bindings.Player.Parkour.performed += Parkour_performed;
        bindings.Player.Scroll.performed += Scroll_performed;

    }
    public void OnDisable()
    {
        bindings.Player.Jump.performed -= Jump_performed;
        bindings.Player.Parkour.performed -= Parkour_performed;
        bindings.Player.Scroll.performed -= Scroll_performed;
    }
    public void Jump_performed(InputAction.CallbackContext context)
    {
        if (context.performed) { OnJump?.Invoke(); }

    }
    public void Parkour_performed(InputAction.CallbackContext context)
    {
        if (context.performed) { OnParkour?.Invoke(); }
    }
    private void Scroll_performed(InputAction.CallbackContext context)
    {
        scrollAmount += Mathf.Clamp(context.ReadValue<float>(), -1, 1);
    }
    public void FixedUpdate()
    {
        movementVector = bindings.Player.Movement.ReadValue<Vector2>();
    }


}
