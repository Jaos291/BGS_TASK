using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Animator _animator;

    public void OnMovement(InputAction.CallbackContext value)
    {
        TurnOffAllAnimations();

        float movementValue = value.ReadValue<Vector2>().magnitude;

        if (movementValue > 0 )
        {
            _animator.SetBool("IsRunning", true);
        }
        else {
            _animator.SetBool("IsRunning", false);
        }
    }

    private void TurnOffAllAnimations()
    {
        AnimatorControllerParameter[] parameters = _animator.parameters;

        foreach (var parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                _animator.SetBool(parameter.name, false);
            }
        }
    }
}
