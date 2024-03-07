using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Dependencies")]
    public InventoryUI inventoryUI;

    public InventorySO playerInventory;

    [Header("Action events")]
    public UnityEvent onInventoryOpened;
    public UnityEvent onInventoryClosed;

    public void OpenInventory(InventorySO playerInventorySO)
    {
        playerInventory = playerInventorySO;
        //Adding Weapons & Shields
        inventoryUI.LoadWeaponsAndShieldsForInventory();
        inventoryUI.CreateButtons();
        //Populate weapons and shields
        inventoryUI.PopulateWeapons();
        inventoryUI.PopulateShields();

        //Populate Consumables
        inventoryUI.PopulateConsumables();

        //Populate Weareables
        inventoryUI.PopulateWeareables();

        //Load Equiped Items On Save File
        inventoryUI.LoadEquipedItems();

        //Set Player Stats for Menu
        inventoryUI.SetPlayerStats();

        //TO DO: Create Key Items and Populate Key Items
    }
}
