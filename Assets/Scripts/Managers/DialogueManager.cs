using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("Dependencies")]
    public DialogueUI dialogueUI;
    public InventorySO playerInventory;

    [Header("Action events")]
    public UnityEvent onConversationStarted;
    public UnityEvent onConversationEnded;

    private Queue<Sentence> sentences;

    private ConversationSO conversationObject;
    private bool talked = false;
    private bool item = false;

    private void Start()
    {
        this.sentences = new Queue<Sentence>();
    }

    public void StartConversation(ConversationSO conversation)
    {
        conversationObject = conversation;
        if (this.sentences.Count != 0)
            return;

        this.dialogueUI.ChangeButtonText(conversationObject.buttonContinueText);
        if (!playerInventory.talkedNPCs.Contains(conversationObject.ConversationName))
        {
            conversation.alreadyTalked = false;
        }
        else
        {
            conversation.alreadyTalked = true;
        }
        if (!conversation.alreadyTalked)
        {
            foreach (var sentence in conversation.sentences)
            {
                this.sentences.Enqueue(sentence);
            }
            talked = true;
        }
        else
        {
            talked = true;
            foreach (var sentence in conversation.AlreadyTalkedSentences)
            {
                this.sentences.Enqueue(sentence);
            }
        }
        

        this.dialogueUI.StartConversation(
            leftCharacterName: conversation.leftCharacter.fullname,
            leftCharacterPortrait: conversation.leftCharacter.portrait,
            rightCharacterName: conversation.rightCharacter.fullname,
            rightCharacterPortrait: conversation.rightCharacter.portrait
        );

        if (this.onConversationStarted != null)
            this.onConversationStarted.Invoke();

        this.NextSentence();
    }

    public void NextSentence()
    {
        if (this.dialogueUI.IsSentenceInProcess())
        {
            this.dialogueUI.FinishDisplayingSentence();
            return;
        }
        if (this.sentences.Count==1)
        {
            this.dialogueUI.FinishButtonLine(conversationObject.buttonFinishText);
        }
        if (this.sentences.Count == 0)
        {
            this.EndConversation();
            return;
        }

        var sentence = this.sentences.Dequeue();
        if (!sentence.giveWeaponHere || !sentence.giveItemHere)
        {
            this.dialogueUI.UnableImageForObtainedObject();
        }
        this.dialogueUI.DisplaySentence(
            characterName: sentence.character.fullname,
            sentenceText: sentence.text
        );
        if (sentence.giveWeaponHere)
        {
            if (sentence.weapon != null && sentence.playerInventory != null)
            {
                this.dialogueUI.SetImageForObtainedObject(sentence.weapon.icon);
                if (!conversationObject.alreadyGaveItem)
                {
                    sentence.playerInventory.AddWeapon(sentence.weapon);
                    conversationObject.alreadyGaveItem = true;
                    item = true;
                }
            }
        }
        if (sentence.giveItemHere)
        {
            if (sentence.consumable != null && sentence.playerInventory != null)
            {
                this.dialogueUI.SetImageForObtainedObject(sentence.consumable.icon);
                if (!conversationObject.alreadyGaveItem)
                {
                    sentence.playerInventory.AddConsumable(sentence.consumable);
                    conversationObject.alreadyGaveItem = true;
                    item = true;
                }
            }
        }
    }

    public void EndConversation()
    {
        conversationObject.alreadyTalked = true;
        talked = true;
        if (!playerInventory.talkedNPCs.Contains(conversationObject.ConversationName))
        {
            playerInventory.talkedNPCs.Add(conversationObject.ConversationName);
        }
        this.dialogueUI.UnableImageForObtainedObject();
        this.dialogueUI.EndConversation();
        if (this.onConversationEnded != null)
            this.onConversationEnded.Invoke();
    }
}
