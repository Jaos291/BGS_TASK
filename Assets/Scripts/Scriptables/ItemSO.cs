using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    [Header("UNIQUE ITEM ID")]
    public int itemID;
    [Header("UNIQUE ITEM DESCRIPTIO")]
    public string itemDescription;

    public Sprite Icon;

    public string itemName;

    public string itemType;

    public int price;

    [HideInInspector]public int reSellPrice;

}
