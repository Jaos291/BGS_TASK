using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Animator _animator;

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 movementInput = value.ReadValue<Vector2>();

        if (movementInput.x != 0 && movementInput.y == 0)
        {
            _animator.SetBool("IsRunning", true);
        }
        // Check for upward movement
        else if (movementInput.y > 0)
        {
            _animator.SetBool("IsRunningUpwards", true);
        }
        // Check for downward movement
        else if (movementInput.y < 0)
        {
            _animator.SetBool("IsRunningDownwards", true);
        }
    }

}
