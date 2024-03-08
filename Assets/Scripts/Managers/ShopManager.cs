using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Dependencies")]
    public ShopUI shopUI;
    public InventorySO playerInventory;
    //////
    [Header("Base Item Game Object for Shop")]
    public GameObject itemPrefab;

    [Header("Grid to Add Item Prefab")]

    public LayoutGroup layoutGroup;

    [HideInInspector] public List<ItemWeaponSO> _itemWeapons;
    [HideInInspector] public List<ItemConsumableSO> _itemConsumables;
    [HideInInspector] public List<ItemWeareableSO> _itemWeareables;
    ///////

    [Header("Shop Configuration")]
    private List<int> weaponsPrices = new List<int>();
    private List<int> consumablePrices = new List<int>();
    private List<int> weareablePrices = new List<int>();

    [Header("Action events")]
    public UnityEvent onShopOpened;
    public UnityEvent onShopClosed;

    private InventorySO _shopInventory;

    private List<Button> _weaponButtons = new List<Button>();
    private List<Button> _consumableButtons = new List<Button>();
    private List<Button> _weareableButtons = new List<Button>();
    public void OpenShop(InventorySO shopInventory)
    {
        if (this._shopInventory != null)
            return;

        this._shopInventory = shopInventory;

        foreach (var item in this._shopInventory.weapons)
        {
            weaponsPrices.Add(item.weapon.price);
        }

        foreach (var item in this._shopInventory.consumables)
        {
            consumablePrices.Add(item.item.price);
        }

        foreach (var item in this._shopInventory.weareables)
        {
            weareablePrices.Add(item.weareable.price);
        }

        this.shopUI.SetupHUD(this._shopInventory, this.weaponsPrices, this.consumablePrices, this.weareablePrices, this.playerInventory);

        //Set buttons for each Weapon / Consumable

        foreach (Transform item in layoutGroup.transform)
        {
            GameObject.Destroy(item.gameObject);
            foreach (Button button in _weaponButtons)
            {
                button.onClick.RemoveAllListeners();
            }
            foreach (Button button in _consumableButtons)
            {
                button.onClick.RemoveAllListeners();
            }
            _itemWeapons.Clear();
            _weaponButtons.Clear();
            _consumableButtons.Clear();
            _weareableButtons.Clear();
        }
        foreach (var weapon in this._shopInventory.weapons)
        {
            _itemWeapons.Add(weapon.weapon);
            GameObject newItemButton = Instantiate(itemPrefab);
            if (weapon.weapon.shield)
            {
                newItemButton.GetComponent<ItemContainer>().SetupItemForShop(
                weapon.weapon.icon,
                weapon.weapon.itemName,
                weapon.weapon.price,
                "shield",
                weapon.weapon.itemID,
                weapon.weapon.itemDescription
                );
                newItemButton.transform.SetParent(layoutGroup.transform, false);
                _weaponButtons.Add(newItemButton.GetComponent<Button>());
            }
            if (weapon.weapon.weapon)
            {
                newItemButton.GetComponent<ItemContainer>().SetupItemForShop(
                weapon.weapon.icon,
                weapon.weapon.itemName,
                weapon.weapon.price,
                "weapon",
                weapon.weapon.itemID,
                weapon.weapon.itemDescription
                );
                newItemButton.transform.SetParent(layoutGroup.transform, false);
                _weaponButtons.Add(newItemButton.GetComponent<Button>());
            }
            Button newButton = newItemButton.GetComponent<ItemContainer>().buyButton;
            newButton.onClick.AddListener(()=> OnBuyItemClicked(weapon.weapon));
            Button descriptionButton = newItemButton.GetComponent<Button>();
            descriptionButton.onClick.AddListener(()=> OnDescriptionItemClicked(weapon.weapon));
        }

        foreach (var consumable in this._shopInventory.consumables)
        {
            _itemConsumables.Add(consumable.item);
            GameObject newItemButton = Instantiate(itemPrefab);
            newItemButton.GetComponent<ItemContainer>().SetupItemForShop(
                consumable.item.icon,
                consumable.item.itemName,
                consumable.item.price,
                "consumable",
                consumable.item.itemID,
                consumable.item.itemDescription
                );
            newItemButton.transform.SetParent(layoutGroup.transform, false);
            _consumableButtons.Add(newItemButton.GetComponent<Button>());

            Button newButton = newItemButton.GetComponent<ItemContainer>().buyButton;
            newButton.onClick.AddListener(() => OnBuyItemClicked(consumable.item));

            Button descriptionButton = newItemButton.GetComponent<Button>();
            descriptionButton.onClick.AddListener(() => OnDescriptionItemClicked(consumable.item));
        }

        foreach (var weareable in this._shopInventory.weareables)
        {
            _itemWeareables.Add(weareable.weareable);
            GameObject newItemButton = Instantiate(itemPrefab);
            newItemButton.GetComponent<ItemContainer>().SetupItemForShop(
                weareable.weareable.icon,
                weareable.weareable.itemName,
                weareable.weareable.price,
                "weareable",
                weareable.weareable.itemID,
                weareable.weareable.itemDescription
                );
            newItemButton.transform.SetParent(layoutGroup.transform, false);
            _consumableButtons.Add(newItemButton.GetComponent<Button>());

            Button newButton = newItemButton.GetComponent<ItemContainer>().buyButton;
            newButton.onClick.AddListener(() => OnBuyItemClicked(weareable.weareable));

            Button descriptionButton = newItemButton.GetComponent<Button>();
            descriptionButton.onClick.AddListener(() => OnDescriptionItemClicked(weareable.weareable));
        }

        //Create Listener for each button


        if (this.onShopOpened != null)
            this.onShopOpened.Invoke();
    }

    public void CloseShop()
    {
        foreach (Transform item in layoutGroup.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        _itemWeapons.Clear();
        _weaponButtons.Clear();
        _consumableButtons.Clear();
        _weaponButtons.Clear();
        this._shopInventory = null;

        if (this.onShopClosed != null)
            this.onShopClosed.Invoke();
    }

    private void BuyItem(ItemSO itemSO)
    {
        // Weapons
        int itemId = itemSO.itemID;
        if (itemSO.weapon || itemSO.shield) // Player is trying to buy a weapon
        {
            var itemPrice = this.weaponsPrices[itemId];

            if (this.playerInventory.gold < itemPrice)
            {  // No money no shopping
                shopUI.EnableWarningText("Not Enough Cash");
                return;
            }
            var item = this._shopInventory.weapons[itemId];
            foreach (var weapons in playerInventory.weapons)
            {
                if (weapons.weapon.itemName.Equals(item.weapon.itemName))
                {
                    shopUI.EnableWarningText("Already have this weapon!");
                    return;
                }
            }
            this.playerInventory.GetGold(itemPrice);
            this.playerInventory.AddWeapon(item.weapon);
            this.shopUI.SetupPlayerHUDforShop();
        }


        // Consumables

        else if (itemSO.consumable) // Player is trying to buy a consumable
        {
            var consumableIndex = itemId % 2;
            var itemPrice = this.consumablePrices[consumableIndex];

            if (this.playerInventory.gold < itemPrice) 
            { // No money no shopping
            shopUI.EnableWarningText("Not Enough Cash");
            return;
            }
            var shopItem = this._shopInventory.consumables[consumableIndex];
            this.playerInventory.GetGold(itemPrice);
            this.playerInventory.AddConsumable(shopItem.item);
            this._shopInventory.RemoveConsumable(shopItem.item,0);
            this.shopUI.SetupPlayerHUDforShop();
        }

        else if (itemSO.weareable){
            var weareableIndex = itemId % 2;
            var itemPrice = this.weaponsPrices[weareableIndex];

            if (this.playerInventory.gold < itemPrice)
            { // No money no shopping
                shopUI.EnableWarningText("Not Enough Cash");
                return;
            }


            var shopItem = this._shopInventory.weareables[weareableIndex];

            Animator animator = PlayerSpawner.Instance.playerReference.GetComponent<Animator>();
            foreach (AnimatorControllerParameter parameter in PlayerSpawner.Instance.playerReference.GetComponent<Animator>().parameters)
            {
                if (parameter.type.Equals(AnimatorControllerParameterType.Bool))
                {
                    animator.SetBool(parameter.name, false);
                }
            }

            PlayerSpawner.Instance.playerReference.GetComponent<Animator>().SetBool(this._shopInventory.weareables[itemId].weareable.animationType, true);

            foreach (var weareable in playerInventory.weareables)
            {
                if (weareable.weareable.itemName.Equals(shopItem.weareable.itemName))
                {
                    shopUI.EnableWarningText("Weared!");
                    return;
                }
            }

            this.playerInventory.GetGold(itemPrice);
            this.playerInventory.AddWeareable(shopItem.weareable);
            this._shopInventory.RemoveWeareable(shopItem.weareable, 0);
            this.shopUI.SetupPlayerHUDforShop();
        }
    }
    private void ShowItemDescription(ItemSO itemSO)
    {
        shopUI.itemDescription.text = itemSO.itemDescription;
    }
    private void OnDescriptionItemClicked(ItemSO itemSO)
    {
        shopUI.itemDescriptionGameObject.SetActive(true);
        ShowItemDescription(itemSO);
    }
    private void OnBuyItemClicked(ItemSO itemSO)
    {
        BuyItem(itemSO);
    }
}
