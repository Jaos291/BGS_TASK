using UnityEngine;
using ScriptableObjectArchitecture;

public class SaveStateTrigger : MonoBehaviour
{
    [Header("Configuration")]
    public SaveStateSO saveStateSO;

    [Header("Broadcasting events")]
    public SaveStateSOGameEvent saveStateRequestEvent;

    public void TriggerSave()
    {
        this.saveStateRequestEvent.Raise(this.saveStateSO);
    }
}
