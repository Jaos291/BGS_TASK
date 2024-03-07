using UnityEngine;

[System.Serializable]
public class SaveRequest
{
    public SaveStateSO save;

    public SaveRequest(SaveStateSO save)
    {
        this.save = save;
    }
}
