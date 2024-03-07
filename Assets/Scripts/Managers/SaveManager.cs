using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    [Header("Dependencies")]
    public SaveUI saveUI;

    [Header("Action events")]
    public UnityEvent onSaveStarted;
    public UnityEvent onSaveEnded;

    public void SetSaveText(SaveStateSO saveStateSO)
    {
        this.saveUI.SetText(saveStateSO.phraseText, saveStateSO.questionForSavingText);
        if (this.onSaveStarted!=null)
        {
            this.onSaveStarted.Invoke();
        }
    }
    public void EndSave()
    {
        if (this.onSaveEnded != null)
        {
            this.onSaveEnded.Invoke();
        }
    }
}
