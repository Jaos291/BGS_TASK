using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRenderer : MonoBehaviour
{
    [Header("SpriteRenderer")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 movementInput = value.ReadValue<Vector2>();

        if ((movementInput.x > 0 && movementInput.y == 0) && PlayerIsLookingLeft())
        {
            _spriteRenderer.flipX = false;
        }
        else if ((movementInput.x < 0 && movementInput.y == 0) && !PlayerIsLookingLeft())
        {
            _spriteRenderer.flipX = true;
        }
    }

    private bool PlayerIsLookingLeft()
    {
        return _spriteRenderer.flipX;
    }
}
