using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public CharacterSO character;
    [TextArea(2, 5)]
    public string text;
    public bool giveWeaponHere;
    public bool giveItemHere;
    public ItemWeaponSO weapon;
    public ItemConsumableSO consumable;
    public InventorySO playerInventory;
}

[CreateAssetMenu(fileName = "NewConversation", menuName = "Scriptable Objects/Conversation/Conversation")]
public class ConversationSO : ScriptableObject
{
    public InventorySO playerInventory;
    public string ConversationName;
    public bool alreadyTalked;
    public bool alreadyGaveItem;
    public CharacterSO leftCharacter;
    public CharacterSO rightCharacter;
    public string buttonContinueText;
    public string buttonFinishText;
    public Sentence[] sentences;
    public Sentence[] AlreadyTalkedSentences;
}
