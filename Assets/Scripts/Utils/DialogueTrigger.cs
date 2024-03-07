using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Configuration")]
    public ConversationSO conversation;

    [Header("Broadcasting events")]
    public ConversationSOGameEvent conversationRequestEvent;

    [Header("If this character OR chest gives an item/consumable/weapon")]
    public ItemWeaponSO weapon;
    public ItemConsumableSO consumable;
    public InventorySO playerInventory;

    public void TriggerConversation()
    {
        this.conversationRequestEvent.Raise(this.conversation);
    }
}
