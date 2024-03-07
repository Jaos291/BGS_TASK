using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{

    public int gold = 0;
    public List<SerializedItem> weapons = new List<SerializedItem>();
    public List<SerializedItem> consumables = new List<SerializedItem>();
    public ItemWeaponSO weapon = null;
    public ItemWeaponSO shield = null;
    public string scene;
    public List<string> playerStats = new List<string>();
    public List<string> talkedNPCs = new List<string>();

    public void DefaultData()
    {
        this.ResetData();
        var firstWeaponSO = Resources.Load<ItemWeaponSO>("InventoryItems/Weapons/FirstWeapon");
        
        if (firstWeaponSO != null)
        {
            this.weapons.Add(new SerializedItem(firstWeaponSO.itemName,1));
        }
    }

    public void ResetData()
    {
        this.gold = 0;
        this.weapons.Clear();
        this.consumables.Clear();
        this.scene = null;
        this.weapon = null;
        this.shield = null;
        this.playerStats.Clear();
        talkedNPCs.Clear();
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json,this);
    }
}
