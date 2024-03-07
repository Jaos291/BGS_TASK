using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CombatUI : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject combatMenu;
    public GameObject wonMenu;
    public GameObject lostMenu;
    public CombatManager combatManager;

    [Header("Consumable Item Object")]
    [SerializeField] private GameObject _consumableInventoryItem;
    [SerializeField] private GameObject _weaponOrShieldInventoryItem;

    [Header("Consumables Grid")]
    [SerializeField] private Transform _consumablesGrid;

    [Header("Weapons Grid")]
    [SerializeField] private Transform _weaponsGrid;

    private List<ItemConsumableSO> _itemConsumables;
    private List<ItemWeaponSO> _itemWeapons;


    public TextMeshProUGUI infoText;
    public TextMeshProUGUI earnedGoldText;

    public TextMeshProUGUI playerName;
    public Slider playerHP;
    public TextMeshProUGUI enemyName;
    public Slider enemyHP;


    // Private
    private CombatUnitSO _player;
    private CombatUnitSO _enemy;
    public InventorySO _playerInventory;

    public void SetupHUD(CombatUnitSO player, CombatUnitSO enemy, float level, int gold)
    {
        _itemWeapons = new List<ItemWeaponSO>();
        _itemConsumables = new List<ItemConsumableSO>();
        // Save references for later
        this._player = player;
        this._enemy = enemy;

        // Link HUD
        this.playerName.text = this._player.unitName;
        this.playerHP.minValue = 0;
        this.playerHP.maxValue = this._player.maxHP;

        this.enemyName.text = this._enemy.unitName;
        this.enemyHP.minValue = 0;
        this.enemyHP.maxValue = this._enemy.maxHP;
    }
    public void PopulateWeapons()
    {
        _itemWeapons.Clear();
        foreach (var weapon in this._playerInventory.weapons)
        {
            _itemWeapons.Add(weapon.weapon);
        }
    }
    public void PopulateConsumables()
    {
        _itemConsumables.Clear();
        foreach (var consumable in this._playerInventory.consumables)
        {
            _itemConsumables.Add(consumable.item);
        }
    }
    public void InstantiateWeaponAndShield()
    {
        foreach (Transform item in _weaponsGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        foreach (var weapon in this._playerInventory.weapons)
        {
            if (weapon.weapon.Equals(this._playerInventory.weapon))
            {
                _itemWeapons.Add(weapon.weapon);
                GameObject newWeaponButton = Instantiate(_weaponOrShieldInventoryItem);
                newWeaponButton.GetComponent<ItemContainer>().SetUpWeaponForBattle(
                    weapon.weapon.icon,
                    weapon.weapon.itemName
                    );
                newWeaponButton.transform.SetParent(_weaponsGrid, false);
                Button newButton = newWeaponButton.GetComponent<Button>();
                newButton.onClick.AddListener(() => OnWeaponClick(weapon));
            }
            if (weapon.weapon.Equals(this._playerInventory.shield))
            {
                _itemWeapons.Add(weapon.weapon);
                GameObject newWeaponButton = Instantiate(_weaponOrShieldInventoryItem);
                newWeaponButton.GetComponent<ItemContainer>().SetUpWeaponForBattle(
                    weapon.weapon.icon,
                    weapon.weapon.itemName
                    );
                newWeaponButton.transform.SetParent(_weaponsGrid, false);
                Button newButton = newWeaponButton.GetComponent<Button>();
                newButton.onClick.AddListener(() => OnWeaponClick(weapon));
            }
        }
    }
    public void SetBattlePlayers(CombatUnitSO enemy, CombatUnitSO player)
    {
        this._enemy =   enemy;
        this._player = player;
    }
    public void InstantiateConsumables()
    {
        foreach (Transform item in _consumablesGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        foreach (var consumable in this._playerInventory.consumables)
        {
            _itemConsumables.Add(consumable.item);
            GameObject newConsumableButton = Instantiate(_consumableInventoryItem);
            newConsumableButton.GetComponent<ItemContainer>().SetUpConsumableForBattle(
                consumable.item.icon,
                consumable.item.itemName,
                consumable.item.itemID,
                consumable.amount
                );
            newConsumableButton.transform.SetParent(_consumablesGrid, false);
            Button newButton = newConsumableButton.GetComponent<Button>();
            newButton.onClick.AddListener(()=> OnConsumeItemClick(consumable.item, newConsumableButton.GetComponent<ItemContainer>()));

        }
    }
    private void OnWeaponClick(InventoryWeapon weapon)
    {
        if (this.combatManager._combatState != CombatManager.CombatStates.PLAYERTURN)
            return;

        this._player.AttackUnit(this._enemy, weapon.weapon, this._playerInventory.playerUnit.attackPower);
        if (this._enemy.currentHP <= 0)
        {
            this.combatManager.StartCoroutine(this.combatManager.CombatWon());
        }
        else
        {
            this.combatManager.StartCoroutine(this.combatManager.EnemyTurn());
        }
    }
    private void OnConsumeItemClick(ItemConsumableSO item, ItemContainer itemContainer)
    {
        if (this.combatManager._combatState != CombatManager.CombatStates.PLAYERTURN)
            return;

        if ((this._playerInventory.playerUnit.currentHP < this._playerInventory.playerUnit.maxHP) && this._playerInventory.consumables.Count > 0)
        {
            itemContainer.ConsumeItem();
            this._playerInventory.playerUnit.Heal(item.healPower);
            playerHP.value = this._playerInventory.playerUnit.currentHP;
            this._playerInventory.RemoveConsumable(item, 1);
            this.combatManager.StartCoroutine(this.combatManager.EnemyTurn());
        }
    }

    public void SetInfoText(string infoText)
    {
        this.infoText.text = infoText;
    }

    public void ShowCombatMenu()
    {
        combatMenu.SetActive(true);
        wonMenu.SetActive(false);
        lostMenu.SetActive(false);
    }

    public void ShowWonMenu(int earnedGold)
    {
        this.earnedGoldText.text = "+" + earnedGold.ToString();

        wonMenu.SetActive(true);
        combatMenu.SetActive(false);
        lostMenu.SetActive(false);
    }

    public void ShowLostMenu()
    {
        lostMenu.SetActive(true);
        wonMenu.SetActive(false);
        combatMenu.SetActive(false);
    }

    public void ResetHUD()
    {
        this._player = null;
        this._enemy = null;

        // Link HUD
        this.playerName.text = "";
        this.playerHP.minValue = 0;
        this.playerHP.maxValue = 0;
        this.playerHP.value = 0;

        this.enemyName.text = "";
        this.enemyHP.minValue = 0;
        this.enemyHP.maxValue = 0;
        this.enemyHP.value = 0;

        // Items

    }

    private void Update()
    {
        if (this._player != null)
            this.playerHP.value = this._player.currentHP;

        if (this._enemy != null)
            this.enemyHP.value = this._enemy.currentHP;
    }
}
