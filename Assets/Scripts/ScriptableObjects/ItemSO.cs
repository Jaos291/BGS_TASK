using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public int price;
    public bool weapon;
    public bool shield;
    public bool consumable;
    [Header("UNIQUE ITEM ID")]
    public int itemID;
    [Header("UNIQUE ITEM DESCRIPTIO")]
    public string itemDescription;
}
