using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class ShopUI : MonoBehaviour
{
    [Header("Dependencies")]

    // Shop Inventory

    public TextMeshProUGUI warningText;
    public TextMeshProUGUI outOfStockText;
    public GameObject shopSecondConsumableOutOfStock;


    // Player Inventory

    public Image playerFirstWeaponImage;

    public Image playerSecondWeaponImage;

    public Image playerFirstConsumableImage;
    public TextMeshProUGUI playerFirstConsumableAmountText;

    public Image playerSecondConsumableImage;
    public TextMeshProUGUI playerSecondConsumableAmountText;

    public TextMeshProUGUI playerGoldText;

    //Public Access to this shop items to use BUY function
    [HideInInspector] public List<ItemWeaponSO> _itemWeapons;
    [HideInInspector] public List<ItemConsumableSO> _itemConsumables;
    [HideInInspector] public List<ItemWeareableSO> _itemWeareables;

    [SerializeField] private GameObject _weaponInventoryItem;
    [SerializeField] private GameObject _consumableInventoryItem;
    [SerializeField] private GameObject _weareableInventoryItem;
    [SerializeField] private LayoutGroup _playerInventoryGrid;


    private InventorySO _shopInventory;
    private List<int> _weaponPrices;
    private List<int> _consumablePrices;
    private InventorySO _playerInventory;

    public GameObject itemDescriptionGameObject;
    public TextMeshProUGUI itemDescription;
    // Public functions
    public void SetupHUD(InventorySO shopInventory, List<int> weaponPrices, List<int> consumablePrices, InventorySO playerInventory)
    {

        //References
        this._shopInventory = shopInventory;
        this._weaponPrices = weaponPrices;
        this._consumablePrices = consumablePrices;
        this._playerInventory = playerInventory;

        // Player items
        SetupPlayerHUDforShop();
    }

    public void SetupPlayerHUDforShop()
    {
        this.ConfigurePlayerWeapons();
        this.ConfigurePlayerConsumables();
        this.ConfigurePlayerGold();
        this.ConfigurePlayerClothes();
    }

    // Private

    /*private void Update()
    {
        if (this._shopInventory == null || this._playerInventory == null)
            return;

        this.ConfigurePlayerWeapons();
        this.ConfigurePlayerConsumables();
        this.ConfigurePlayerGold();
    }*/

    public void EnableWarningText(string text)
    {
        StartCoroutine(WarningTextCoroutine(text));
    }
    public IEnumerator WarningTextCoroutine(string text)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = text;
        yield return new WaitForSecondsRealtime(2);
        warningText.gameObject.SetActive(false);
    }

    private void ConfigurePlayerWeapons()
    {
        foreach (Transform item in _playerInventoryGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        foreach (var weapon in this._playerInventory.weapons)
        {
            if (weapon.weapon.weapon)
            {
                GameObject newItemButton = Instantiate(_weaponInventoryItem);
                newItemButton.GetComponent<ItemContainer>().SetupWeaponForInventory(
                weapon.weapon.icon,
                weapon.weapon.itemName,
                "weapon",
                weapon.weapon.itemID
                );
                newItemButton.transform.SetParent(_playerInventoryGrid.transform, false);
            }
        }
        foreach (var shield in this._playerInventory.weapons)
        {
            if (shield.weapon.shield)
            {
                GameObject newItemButton = Instantiate(_weaponInventoryItem);
                newItemButton.GetComponent<ItemContainer>().SetupWeaponForInventory(
                shield.weapon.icon,
                shield.weapon.itemName,
                "shield",
                shield.weapon.itemID
                );
                newItemButton.transform.SetParent(_playerInventoryGrid.transform, false);
            }
        }
        /*if (this._playerInventory.weapons.Count > 1)
        {
            // Player has 2 weapons
            var weaponItem = this._playerInventory.weapons[1];

            this.playerSecondWeaponImage.sprite = weaponItem.weapon.icon;
            this.playerSecondWeaponImage.color = Color.white;
        }
        else
        {
            this.playerSecondWeaponImage.sprite = null;
            this.playerSecondWeaponImage.color = Color.clear;
        }

        if (this._playerInventory.weapons.Count > 0)
        {
            // Player has 1 weapon
            var weaponItem = this._playerInventory.weapons[0];

            this.playerFirstWeaponImage.sprite = weaponItem.weapon.icon;
            this.playerFirstWeaponImage.color = Color.white;
        }
        else
        {
            this.playerFirstWeaponImage.sprite = null;
            this.playerFirstWeaponImage.color = Color.clear;
        }*/
    }
    private void ConfigurePlayerConsumables()
    {
        foreach (var consumable in this._playerInventory.consumables)
        {
            _itemConsumables.Add(consumable.item);
            GameObject newItemButton = Instantiate(_consumableInventoryItem);
            newItemButton.GetComponent<ItemContainer>().SetupConsumableForInventory(
                consumable.item.icon,
                consumable.item.itemName,
                "consumable",
                consumable.item.itemID,
                consumable.amount
                );
            newItemButton.transform.SetParent(_playerInventoryGrid.transform, false);
        }
        
        /*if (this._playerInventory.consumables.Count > 1)
        {
            // Player has 2 weapons
            var consumableItem = this._playerInventory.consumables[1];

            this.playerSecondConsumableImage.sprite = consumableItem.item.icon;
            this.playerSecondConsumableImage.color = Color.white;
            this.playerSecondConsumableAmountText.text = consumableItem.amount.ToString();
        }
        else
        {
            this.playerSecondConsumableImage.sprite = null;
            this.playerSecondConsumableImage.color = Color.clear;
            this.playerSecondConsumableAmountText.text = null;
        }

        if (this._playerInventory.consumables.Count > 0)
        {
            // Player has 1 weapon
            var consumableItem = this._playerInventory.consumables[0];

            this.playerFirstConsumableImage.sprite = consumableItem.item.icon;
            this.playerFirstConsumableImage.color = Color.white;
            this.playerFirstConsumableAmountText.text = consumableItem.amount.ToString();
        }
        else
        {
            this.playerFirstConsumableImage.sprite = null;
            this.playerFirstConsumableImage.color = Color.clear;
            this.playerFirstConsumableAmountText.text = null;
        }*/
    }

    private void ConfigurePlayerClothes()
    {
        foreach (var clothes in this._playerInventory.weareables)
        {
            _itemWeareables.Add(clothes.weareable);
            GameObject newItemButton = Instantiate(_weareableInventoryItem);
            newItemButton.GetComponent<ItemContainer>().SetupWeareableForInventory(
                clothes.weareable.icon,
                clothes.weareable.name,
                "weareable",
                clothes.weareable.itemID
                );
            newItemButton.transform.SetParent(_playerInventoryGrid.transform,false);
        }
    }

    private void ConfigurePlayerGold()
    {
        this.playerGoldText.text = this._playerInventory.gold.ToString();
    }
}
