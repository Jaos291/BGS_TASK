using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "SaveSystem", menuName = "Scriptable Objects/Save System")]
public class SaveSystem : ScriptableObject
{
    public string saveFilename = "savefile.sav";
    public string backupSaveFilename = "savefile.sav.bak";
    private string SceneName;

    public InventorySO playerInventory;

    [HideInInspector] public SaveData saveData = new SaveData();
    public SceneSO[] sceneScenes;

    // Save System Logic

    public void EnableContinueButton()
    {
        if (FileManager.LoadFromFile(this.saveFilename, out var json))
        {
            GameObject ContinueButton = GameObject.Find("ContinueButton");
            ContinueButton.GetComponent<Button>().enabled = true;
            ContinueButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void LoadSaveDataFromDisk(SceneLoader sceneLoader)
    {
        if (FileManager.LoadFromFile(this.saveFilename, out var json))
        {
            this.saveData.FromJson(json);
            this.LoadSavedInventory(sceneLoader);
        }
        else
        {
            Debug.Log("Couldn't load!");
        }
    }

    public void SaveDataToDisk()
    {
        if (FileManager.MoveFile(saveFilename, backupSaveFilename))
        {
            if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
            {
                Debug.Log("Save successful");
            }
        }
    }

    private void CreateEmptySaveFile()
    {
        FileManager.WriteToFile(saveFilename, "");
    }

    public void DeleteFile()
    {
        FileManager.DeleteFile(saveFilename);
    }

    // Save System saving logic

    public void SaveInventory()
    {
        if (!FileManager.LoadFromFile(this.saveFilename, out var json))
        {
            this.CreateEmptySaveFile(); // Create empty file
            this.saveData.DefaultData(); // Create default data
            this.SaveDataToDisk(); // Save default data to file
        }
        else
        {
            // Reset data and repopulate it with real info from the inventory
            this.saveData.ResetData();

            // Gold
            this.saveData.gold = this.playerInventory.gold;

            // Weapons
            foreach (var weapon in this.playerInventory.weapons)
            {
                this.saveData.weapons.Add(new SerializedItem(weapon.weapon.itemName, 1));
            }
            // Consumables
            foreach (var consumable in this.playerInventory.consumables)
            {
                this.saveData.consumables.Add(new SerializedItem(consumable.item.itemName, consumable.amount));
            }
            this.saveData.weapon= this.playerInventory.weapon;
            this.saveData.shield= this.playerInventory.shield;
            this.saveData.playerStats = this.playerInventory.playerUnit.ReturnPlayerStats();
            //NPC's talked
            this.saveData.talkedNPCs = this.playerInventory.talkedNPCs;

            //Scene Name

            this.saveData.scene = SceneManager.GetActiveScene().name;

            // Save to disk
            this.SaveDataToDisk();
        }
    }

    public void LoadSavedInventory(SceneLoader sceneLoader)
    {
        // Reset inventory and repopulate it with the info from the saved data
        this.playerInventory.gold = 0;
        this.playerInventory.weapons.Clear();
        this.playerInventory.consumables.Clear();


        // Gold
        this.playerInventory.gold = this.saveData.gold;


        // Weapons

        // Get all weapon scriptable objects that are in the "Resources" folder
        var weaponSOs = new List<ItemWeaponSO>(
            Resources.LoadAll<ItemWeaponSO>("InventoryItems/Weapons")
        );
        // For each item we have in the save data file...
        for (int weaponIndex = 0; weaponIndex < this.saveData.weapons.Count; weaponIndex++)
        {
            // Get the serialized data (the saved item info)
            var serializedWeapon = this.saveData.weapons[weaponIndex];
            // Get the actual Scriptable Object from the all we have from the folder
            var weaponSO = weaponSOs.Find((c) => { return c.itemName == serializedWeapon.itemName; });

            // If the scriptable object is actually there...
            if (weaponSO != null)
            {
                // Create a inventory consumable and put the correct amount
                var weaponItem = new InventoryWeapon(weaponSO, serializedWeapon.amount);

                if (!this.playerInventory.weapons.Contains(weaponItem))
                {
                    // Then, put in in the inventory
                    this.playerInventory.weapons.Insert(weaponIndex, weaponItem);
                }
            }
        }
        this.playerInventory.weapon = saveData.weapon;
        this.playerInventory.shield = saveData.shield;


        // Consumables

        // Get all consumable scriptable objects that are in the "Resources" folder
        var consumablesSOs = new List<ItemConsumableSO>(
            Resources.LoadAll<ItemConsumableSO>("InventoryItems/Consumables")
        );
        // For each item we have in the save data file...
        for (int consumableIndex = 0; consumableIndex < this.saveData.consumables.Count; consumableIndex++)
        {
            // Get the serialized data (the saved item info)
            var serializedConsumable = this.saveData.consumables[consumableIndex];
            // Get the actual Scriptable Object from the all we have from the folder
            var consumableSO = consumablesSOs.Find((c) => { return c.itemName == serializedConsumable.itemName; });

            // If the scriptable object is actually there...
            if (consumableSO != null)
            {
                // Create a inventory consumable and put the correct amount
                var consumableItem = new InventoryConsumable(consumableSO, serializedConsumable.amount);

                if (!this.playerInventory.consumables.Contains(consumableItem))
                {
                    // Then, put in in the inventory
                    this.playerInventory.consumables.Insert(consumableIndex, consumableItem);
                }
            }
        }
        //Load player stats
        this.playerInventory.playerUnit.LoadPlayerStats(this.saveData.playerStats);
        //SavedScene
        string savedScene = this.saveData.scene;
        LoadSceneFromContinue(sceneLoader, savedScene);
    }
    public void LoadSceneFromContinue(SceneLoader sceneLoader, string sceneName)
    {
        foreach (var item in sceneScenes)
        {
            if (item.sceneName.Equals(sceneName))
            {
                sceneLoader.sceneToLoad = item;
                sceneLoader.levelEntrance= item.levelEntrance;
            }
        }
    }
}