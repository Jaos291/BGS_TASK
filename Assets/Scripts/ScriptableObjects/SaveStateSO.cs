using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSave", menuName = "Scriptable Objects/Save")]
public class SaveStateSO : ScriptableObject
{
    public string phraseText;
    public string questionForSavingText;
}
