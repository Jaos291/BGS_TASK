using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using static UnityEditor.Progress;

public class InventoryUI: MonoBehaviour
{
    [SerializeField]private InventorySO playerInventory;

    [Header("Gameobject for EQUIPED Weapon, shield & weareables!")]
    [SerializeField] private GameObject _equipedWeaponPrefab;
    [SerializeField] private GameObject _equipedShieldPrefab;
    [SerializeField] private GameObject _equipedWeareable;

    [Header("GameObject for weapon Shield and items")]
    [SerializeField] private GameObject _weaponInventoryItem;
    [SerializeField] private GameObject _consumableInventoryItem;
    [SerializeField] private GameObject _weareableInventoryItem;

    [Header("Player Combat Unit Info")]
    [SerializeField] private TextMeshProUGUI _currentHP;
    [SerializeField] private TextMeshProUGUI _totalHP;
    [SerializeField] private Slider _currentHPSlider;

    [SerializeField] private TextMeshProUGUI _currentMana;
    [SerializeField] private TextMeshProUGUI _totalMana;
    [SerializeField] private Slider _currentManaSlider;

    [SerializeField] private TextMeshProUGUI _currentEXP;
    [SerializeField] private TextMeshProUGUI _totalEXP;
    [SerializeField] private Slider _currenEXPSlider;

    [SerializeField] private TextMeshProUGUI _attack;
    [SerializeField] private TextMeshProUGUI _magicPower;
    [SerializeField] private TextMeshProUGUI _defense;
    [SerializeField] private TextMeshProUGUI _magicResist;
    [SerializeField] private TextMeshProUGUI _agility;
    [SerializeField] private TextMeshProUGUI _accuracy;

    [Header("Weapons Grid Layout")]
    [SerializeField] private LayoutGroup _weaponGrid;

    [Header("Shields Grid Layout")]
    [SerializeField] private LayoutGroup _shieldsGrid;

    [Header("Consumables Grid Layout")]
    [SerializeField] private LayoutGroup _consumablesGrid;

    [Header("Weareables!")]
    [SerializeField] private LayoutGroup _weareablesGrid;

    [Header("KeyItems Grid Layout")]
    [SerializeField] private LayoutGroup _keyItemsGrid;
    

    [Header("Gold")]
    [SerializeField] private TextMeshProUGUI _goldValue;

    [HideInInspector] public List<ItemWeaponSO> _weaponsAndShield;
    [HideInInspector] public List<ItemConsumableSO> _itemConsumables;
    [HideInInspector] public List<ItemWeareableSO> _itemWeareables;

    [Header("Unequip Buttons")]
    [SerializeField] private Button _unequipWeapon;
    [SerializeField] private Button _unequipShield;
    [SerializeField] private Button _unequipWeareable;


    //Buttons group for each weapon, shield, item and consumable;
    private List<Button> _weaponButtons = new List<Button>();
    private List<Button> _shieldButtons = new List<Button>();
    private List<Button> _consumableButtons = new List<Button>();
    private List<Button> _weareablesButtons = new List<Button>();
    private List<Button> _keyButtons = new List<Button>();
    public void CreateButtons()
    {
        //Creating buttons for weapons
        foreach (Transform item in _weaponGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
            foreach (Button button in _weaponButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
        //Creating buttons for shields
        foreach (Transform item in _shieldsGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
            foreach (Button button in _shieldButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
        //Creating buttons for consumables
        foreach (Transform item in _consumablesGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
            foreach (Button button in _consumableButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        foreach (Transform weareable in _weareablesGrid.transform)
        {
            GameObject.Destroy(weareable.gameObject);
            foreach (Button button in _weareablesButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        _unequipShield.onClick.RemoveAllListeners();
        _unequipWeapon.onClick.RemoveAllListeners();
        _unequipWeareable.onClick.RemoveAllListeners();
    }
    public void LoadWeaponsAndShieldsForInventory()
    {
        //Adding Weapons
        foreach (var weapon in playerInventory.weapons)
        {
            _weaponsAndShield.Add(weapon.weapon);
        }
        //Adding Consumables
        foreach (var consumable in playerInventory.consumables)
        {
            _itemConsumables.Add(consumable.item);
        }
        //Adding Weareables
        foreach (var weareable in playerInventory.weareables)
        {
            _itemWeareables.Add(weareable.weareable);
        }
    }
    public void PopulateWeapons()
    {
        foreach (var weapon in this.playerInventory.weapons)
        {
            _weaponsAndShield.Add(weapon.weapon);
            GameObject newItemButton = Instantiate(_weaponInventoryItem);
            if (weapon.weapon.weapon)
            {
                newItemButton.GetComponent<ItemContainer>().SetupWeaponForInventory(
                weapon.weapon.icon,
                weapon.weapon.itemName,
                "weapon",
                weapon.weapon.itemID
                );
                newItemButton.transform.SetParent(_weaponGrid.transform, false);
                _weaponButtons.Add(newItemButton.GetComponent<Button>());
            }
            Button newButton = newItemButton.GetComponent<Button>();
            newButton.onClick.AddListener(() => OnEquipItemClick(weapon.weapon));
        }
    }
    public void PopulateShields()
    {
        foreach (var weapon in this.playerInventory.weapons)
        {
            _weaponsAndShield.Add(weapon.weapon);
            GameObject newItemButton = Instantiate(_weaponInventoryItem);
            if (weapon.weapon.shield)
            {
                newItemButton.GetComponent<ItemContainer>().SetupWeaponForInventory(
                weapon.weapon.icon,
                weapon.weapon.itemName,
                "shield",
                weapon.weapon.itemID
                );
                newItemButton.transform.SetParent(_shieldsGrid.transform, false);
                _shieldButtons.Add(newItemButton.GetComponent<Button>());
            }
            Button newButton = newItemButton.GetComponent<Button>();
            newButton.onClick.AddListener(() => OnEquipItemClick(weapon.weapon));
        }
    }
    public void PopulateConsumables()
    {
        foreach (var consumable in this.playerInventory.consumables)
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
            newItemButton.transform.SetParent(_consumablesGrid.transform, false);
            Button newButton = newItemButton.GetComponent<Button>();
            newButton.onClick.AddListener(() => OnConsumeItemClick(consumable.item, newItemButton.GetComponent<ItemContainer>()));
        }
    }

    public void PopulateWeareables()
    {
        int i = 0;
        foreach (var weareables in this.playerInventory.weareables)
        {
            _itemWeareables.Add(weareables.weareable);
            GameObject newItemButton = Instantiate(_weareableInventoryItem);
            newItemButton.GetComponent<ItemContainer>().SetupWeareableForInventory(
                weareables.weareable.icon,
                weareables.weareable.itemName,
                "weareable",
                weareables.weareable.itemID
                );
            newItemButton.transform.SetParent(_weareablesGrid.transform, false);
            Button newButton = newItemButton.GetComponent<Button>();
            newButton.onClick.AddListener(() => OnEquipoWeareableClick(weareables.weareable));
        }
    }

    private void EquipItem(ItemWeaponSO itemWeaponSO)
    {

        if (itemWeaponSO.weapon)
        {
            if (this.playerInventory.weapon == null || this.playerInventory.weapon.itemName != itemWeaponSO.itemName)
            {
                _equipedWeaponPrefab.GetComponent<ItemContainer>().SetupItemForEquiped(
                    itemWeaponSO.icon,
                    itemWeaponSO.itemName
                );
                this.playerInventory.EquipWeapon(itemWeaponSO);
                this.playerInventory.weapon = itemWeaponSO;
                this.playerInventory.playerUnit.attackPower = this.playerInventory.weapon.attackPower + this.playerInventory.playerUnit.baseAttackPower;
            }


        }
        else if (itemWeaponSO.shield)
        {
            if (this.playerInventory.shield == null || this.playerInventory.shield.itemName != itemWeaponSO.itemName)
            {
                _equipedShieldPrefab.GetComponent<ItemContainer>().SetupItemForEquiped(
                    itemWeaponSO.icon,
                    itemWeaponSO.itemName
                );
                this.playerInventory.EquipShield(itemWeaponSO);
                this.playerInventory.shield = itemWeaponSO;
                this.playerInventory.playerUnit.defense = this.playerInventory.shield.attackPower + this.playerInventory.playerUnit.baseDefense;
            }
        }
        SetPlayerStats();
    }

    private void EquipWeareable(ItemWeareableSO itemWeareableSO)
    {
        _equipedWeareable.GetComponent<ItemContainer>().SetupWeareableForInventory(
            itemWeareableSO.icon,
            itemWeareableSO.itemName,
            "weareable",
            itemWeareableSO.itemID
            );
        this.playerInventory.EquipWearaeble(itemWeareableSO);
        this.playerInventory.weareable = itemWeareableSO;
        this.playerInventory.playerUnit.defense = this.playerInventory.weareable.defense + this.playerInventory.playerUnit.baseDefense;
       /* Animator animator = PlayerSpawner.Instance.playerReference.GetComponent<Animator>();
        foreach (AnimatorControllerParameter parameter in PlayerSpawner.Instance.playerReference.GetComponent<Animator>().parameters)
        {
            if (parameter.type.Equals(AnimatorControllerParameterType.Bool))
            {
                animator.SetBool(parameter.name,false);
            }
        }
        PlayerSpawner.Instance.playerReference.GetComponent<Animator>().SetBool(itemWeareableSO.animationType,true);*/
        SetPlayerStats();
    }
    private void ConsumeItem(ItemConsumableSO itemSO, ItemContainer itemContainer)
    {
        if (itemSO.consumable)
        {
            if ((this.playerInventory.playerUnit.currentHP < this.playerInventory.playerUnit.maxHP) && this.playerInventory.consumables.Count > 0)
            {
                itemContainer.ConsumeItem();
                this.playerInventory.playerUnit.Heal(itemSO.healPower);
                _currentHPSlider.value = playerInventory.playerUnit.currentHP;
                this.playerInventory.RemoveConsumable(itemSO, 1);
                _currentHP.text = playerInventory.playerUnit.currentHP.ToString();
            }
            else
            {
                Debug.Log("Hp is full!");
            }
        }
    }
    private void OnEquipItemClick(ItemWeaponSO itemSO)
    {
        EquipItem(itemSO);
    }

    private void OnEquipoWeareableClick(ItemWeareableSO weareableSO)
    {
        EquipWeareable(weareableSO);
    }

    private void OnConsumeItemClick(ItemConsumableSO itemSO, ItemContainer itemContainer)
    {
        ConsumeItem(itemSO, itemContainer);
    }
    public void CloseInventory()
    {
        foreach (Transform item in _weaponGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        foreach (Transform item in _shieldsGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        foreach (Transform item in _consumablesGrid.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
        _weaponsAndShield.Clear();
        _itemConsumables.Clear();
        _weaponButtons.Clear();
        _consumableButtons.Clear();
        this.playerInventory = null;
    }
    public void SetPlayerStats()
    {

        //Set CurrentPlayer Stats

        //Life:
        _currentHP.text = playerInventory.playerUnit.currentHP.ToString();
        _totalHP.text = playerInventory.playerUnit.maxHP.ToString();
        _currentHPSlider.maxValue = playerInventory.playerUnit.maxHP;
        _currentHPSlider.value = playerInventory.playerUnit.currentHP;

        //Mana
        _currentMana.text = playerInventory.playerUnit.currentMana.ToString();
        _totalMana.text = playerInventory.playerUnit.maxMana.ToString();
        _currentManaSlider.maxValue = playerInventory.playerUnit.maxMana;
        _currentManaSlider.value = playerInventory.playerUnit.currentMana;

        //Attack, defense, and resists
        _attack.text = playerInventory.playerUnit.attackPower.ToString();
        _magicPower.text = playerInventory.playerUnit.magicPower.ToString();
        _defense.text = playerInventory.playerUnit.defense.ToString();
        _magicResist.text = playerInventory.playerUnit.magicResist.ToString();
        _agility.text = playerInventory.playerUnit.agility.ToString();
        _accuracy.text = playerInventory.playerUnit.accuracy.ToString();

        //Experience
        _currentEXP.text = playerInventory.playerUnit.currentExperience.ToString();
        _totalEXP.text = playerInventory.playerUnit.experienceForNextLevel.ToString();
        _currenEXPSlider.value = playerInventory.playerUnit.currentExperience;

    }
    public void LoadEquipedItems()
    {
        _goldValue.text = playerInventory.gold.ToString();
        if (this.playerInventory.weapon != null)
        {
            _equipedWeaponPrefab.GetComponent<ItemContainer>().SetupItemForEquiped(
                this.playerInventory.weapon.icon,
                this.playerInventory.weapon.itemName
                );
            this.playerInventory.playerUnit.attackPower = this.playerInventory.weapon.attackPower + this.playerInventory.playerUnit.baseAttackPower;
        }
        else
        {
            this.playerInventory.playerUnit.attackPower = this.playerInventory.playerUnit.baseAttackPower;
        }
        if (this.playerInventory.shield != null)
        {
            _equipedShieldPrefab.GetComponent<ItemContainer>().SetupItemForEquiped(
                    this.playerInventory.shield.icon,
                    this.playerInventory.shield.itemName
                );
            this.playerInventory.playerUnit.defense = this.playerInventory.shield.attackPower + this.playerInventory.playerUnit.baseDefense;
        }
        else
        {
            this.playerInventory.playerUnit.defense = this.playerInventory.playerUnit.baseDefense;
        }
        _unequipShield.onClick.AddListener(UnequipShield);
        _unequipWeapon.onClick.AddListener(UnequipWeapon);
    }

    private void UnequipWeapon()
    {
        if (this.playerInventory.weapon != null)
        {
            this.playerInventory.playerUnit.attackPower = this.playerInventory.playerUnit.baseAttackPower;
            _equipedWeaponPrefab.GetComponent<ItemContainer>().UnequipItem();
            this.playerInventory.weapon = null;
            SetPlayerStats();
        }
    }
    private void UnequipShield()
    {
        if (this.playerInventory.shield != null)
        {
            this.playerInventory.playerUnit.defense = this.playerInventory.playerUnit.baseDefense;
            _equipedShieldPrefab.GetComponent<ItemContainer>().UnequipItem();
            this.playerInventory.shield = null;
            SetPlayerStats();
        }
    }
}
