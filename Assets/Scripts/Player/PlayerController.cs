using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float _speed;

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D _rigidbody2D;


    private Vector2 _movementInput;

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _movementInput * _speed;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        _movementInput = value.ReadValue<Vector2>();
    }
}
