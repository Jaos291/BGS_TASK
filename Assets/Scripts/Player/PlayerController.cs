using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    public float speed;

    [Header("Dependencies")]
    public Rigidbody2D _rigidbody;

    private Vector2 _movementInput;

    public void OnMovement(InputAction.CallbackContext value)
    {
        _movementInput = value.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = _movementInput * speed;
    }

}
