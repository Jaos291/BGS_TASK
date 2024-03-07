using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;

public class ItemContainer : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCost;
    public string itemType;
    public GameObject outOfStock;
    public int itemID;
    public TextMeshProUGUI amount;
    public Button buyButton;
    public TextMeshProUGUI description;

    public void SetupItemForShop(Sprite image, string itemName, int itemCost, string itemType, int itemID, string description)
    {
        itemImage.sprite = image;
        this.itemName.text = itemName;
        this.itemCost.text = itemCost.ToString();
        this.itemType = itemType;
        this.itemID = itemID;
        this.description.text = description;
    }
    public void SetupWeaponForInventory(Sprite image, string itemName, string itemType, int itemID)
    {
        itemImage.sprite = image;
        this.itemName.text = itemName;
        this.itemType = itemType;
        this.itemID = itemID;
    }
    public void SetupConsumableForInventory(Sprite image, string itemName, string itemType, int itemID, int amount)
    {
        itemImage.sprite = image;
        this.itemName.text = itemName;
        this.itemType = itemType;
        this.itemID = itemID;
        this.amount.text = amount.ToString();
    }
    public void SetUpConsumableForBattle(Sprite image, string itemName, int itemID, int amount)
    {
        itemImage.sprite = image;
        this.itemName.text = itemName;
        this.itemID = itemID;
        this.amount.text = amount.ToString();
    }
    public void SetUpWeaponForBattle(Sprite image, string itemName)
    {
        itemImage.sprite= image;
        this.itemName.text = itemName;
    }
    public void SetupItemForEquiped(Sprite image, string itemName)
    {
        itemImage.color = Color.white;
        itemImage.sprite = image;
        this.itemName.text = itemName;
    }
    public void UnequipItem()
    {
        itemImage.color = Color.clear;
        itemImage.sprite = null;
        this.itemName.text = null;
    }
    public void ConsumeItem()
    {
        int actualConsumablesAmount = int.Parse(this.amount.text);
        if (actualConsumablesAmount != 0) 
        {
            actualConsumablesAmount -=1;
        }
        this.amount.text = actualConsumablesAmount.ToString();
    }
    public void ShowDescription()
    {

    }
}
