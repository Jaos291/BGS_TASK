using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Action events")]
    public UnityEvent onInteract;

    [Header("Interact Visual Key")]
    public GameObject visualKey;

    public void Interact()
    {
        if (onInteract != null)
            onInteract.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (visualKey != null)
            visualKey.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (visualKey != null)
            visualKey.SetActive(false);
    }
}
