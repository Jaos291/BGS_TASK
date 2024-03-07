using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SaveUI : MonoBehaviour
{
    [Header("Text for this save!")]
    public TextMeshProUGUI PhraseText;
    public TextMeshProUGUI QuestionForSaving;



    public void SetText(string phraseText, string questionForSaving)
    {
        this.PhraseText.text = phraseText;
        this.QuestionForSaving.text = questionForSaving;
    }
}
