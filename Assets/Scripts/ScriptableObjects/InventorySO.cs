using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class InventoryConsumable
{
    public ItemConsumableSO item;
    public int amount;


    public InventoryConsumable(ItemConsumableSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

}
[System.Serializable]
public class InventoryWeapon
{
    public ItemWeaponSO weapon;
    public int amount;
    public InventoryWeapon(ItemWeaponSO weapon, int amount)
    {
        this.weapon = weapon;
        this.amount = amount;
    }
}

[System.Serializable]
public class InventoryWeareable

{

    public ItemWeareableSO weareable;

    public int amount;



    public InventoryWeareable(ItemWeareableSO wareable, int amount)

    {

        this.weareable = wareable;

        this.amount = amount;

    }

}
public class NpcConversation
{
    public string npcName;
    public bool alreadyTalked;
    public bool alreadyGaveItem;
}

[CreateAssetMenu(fileName = "NewInventory", menuName = "Scriptable Objects/Inventory/Inventory")]
public class InventorySO : ScriptableObject
{
    [Header("Sprites")]
    public Sprite[] sprites;

    [Header("Combat Unit")]
    public CombatUnitSO playerUnit;

    [Header("Gold")]
    public int gold = 0;

    [Header("Weapons")]
    public List<InventoryWeapon> weapons = new List<InventoryWeapon>();

    [Header("Consumables")]
    public List<InventoryConsumable> consumables = new List<InventoryConsumable>();

    [Header("Weareables")]
    public List<InventoryWeareable> weareables = new List<InventoryWeareable>();

    [Header("Equiped Items")]
    public ItemWeaponSO weapon;

    public ItemWeaponSO shield;

    public ItemWeareableSO weareable;

    [Header("Already Created this shop on World")]
    public bool isCreated;

    public List<string> talkedNPCs = new List<string>();

    public void AddGold(int gold)
    {
        this.gold += Mathf.Abs(gold);
    }

    public List<string> returnTalkedNPCS()
    {
        return this.talkedNPCs;
    }

    public void GetGold(int gold)
    {
        this.gold -= Mathf.Abs(gold);
    }

    public bool AddWeapon(ItemWeaponSO weapon)
    {
        var weaponFound = this.weapons.Find((c) => { return c.weapon.itemName == weapon.itemName ; });
        if (weaponFound != null)
        {
            weaponFound.amount += 1;
        }
        else
        {
            var newWeapon = new InventoryWeapon(weapon, 1);
            this.weapons.Add(newWeapon);
        }
        return true;
    }
    public bool AddConsumable(ItemConsumableSO consumable)
    {
        var consumableFound = this.consumables.Find((c) => { return c.item.itemName == consumable.itemName; });

        if (consumableFound != null)
        {
            consumableFound.amount += 1;
        }
        else
        {
            var newInventoryConsumable = new InventoryConsumable(consumable, 1);
            this.consumables.Add(newInventoryConsumable);
        }

        return true;
    }

    public bool AddWeareable(ItemWeareableSO weareable)
    {
        var weareableFound = this.weareables.Find((c) => { return c.weareable.itemName == weareable.itemName; });

        if (weareableFound != null)
        {
            weareableFound.amount += 1;
        }
        else
        {
            var newInventoryConsumable = new InventoryWeareable(weareable, 1);
            this.weareables.Add(newInventoryConsumable);
        }

        return true;
    }

    public bool RemoveWeapon(ItemWeaponSO weapon)
    {
        var weaponFound = this.weapons.Find((c) => { return c.weapon.itemName == weapon.itemName; });
        if (weaponFound != null) 
        {
            weaponFound.amount -= 1;

            if (weaponFound.amount==0)
            {
                return this.weapons.Remove(weaponFound);
            }
        }
        return false;
    }

    public bool RemoveConsumable(ItemConsumableSO consumable, int negative)
    {
        var consumableFound = this.consumables.Find((c) => { return c.item.itemName == consumable.itemName; });

        if (consumableFound != null)
        {
            consumableFound.amount = consumableFound.amount - negative;

            if (consumableFound.amount == 0)
            {
                return this.consumables.Remove(consumableFound);
            }
        }

        return false;
    }

    public bool RemoveWeareable(ItemWeareableSO weareable, int negative)
    {
        var weareableFound = this.weareables.Find((c) => { return c.weareable.itemName == weareable.itemName; });

        if (weareableFound != null)
        {
            weareableFound.amount = weareableFound.amount - negative;

            if (weareableFound.amount == 0)
            {
                return this.weareables.Remove(weareableFound);
            }
        }

        return false;
    }

    public void EquipWeapon(ItemWeaponSO weapon)
    {
        TryToEquipWeapon(weapon);
    }
    public void EquipShield(ItemWeaponSO shield)
    {
        TryToEquipShield(shield);
    }

    public void EquipWearaeble(ItemWeareableSO weareable)
    {
        TryToEquipWeareable(weareable);
    }

    private ItemWeaponSO TryToEquipWeapon(ItemWeaponSO weapon)
    {
        var foundWeapon = this.weapons.Find((c) => { return c.weapon.itemName.Equals(weapon.itemName); });

        if (foundWeapon != null)
        {
            weapon = foundWeapon.weapon;
        }
        return weapon;
    }
    private ItemWeaponSO TryToEquipShield(ItemWeaponSO shield)
    {
        var foundShield = this.weapons.Find((c) => { return c.weapon.itemName.Equals(shield.itemName); });

        if (foundShield != null)
        {
            shield = foundShield.weapon;
        }
        return shield;
    }

    private ItemWeareableSO TryToEquipWeareable(ItemWeareableSO weareable)
    {
        var foundWeareable = this.weareables.Find((c) => { return c.weareable.itemName.Equals(weareable.itemName); });
        if (foundWeareable != null)
        {
            weareable = foundWeareable.weareable;
        }

        return weareable;
    }
    private void ConsumeConsumable()
    {

    }
}
