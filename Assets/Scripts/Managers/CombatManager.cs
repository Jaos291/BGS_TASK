using UnityEngine;
using System.Collections;



public class CombatManager : MonoBehaviour
{
    public enum CombatStates
    {
        NONE,
        START,
        PLAYERTURN,
        ENEMYTURN,
        WON,
        LOST
    }
    [Header("Combat Configuration")]
    public int baseGoldPrize = 10;
    public float prizeIncrementPerLevel = 0.20f; // 20%
    public float difficultyIncrementPerLevel = 0.15f; // 15%
    public float timeBetweenActions = 1.5f; // 1.5 seconds

    [Header("Combat Messages Configuration")]
    public string combatStartedInfoText = "An enemy appeared...";
    public string playerTurnInfoText = "Player's turn...";
    public string playerWonInfoText = "Enemy defeated!";
    public string enemyTurnInfoText = "Enemy's turn...";
    public string enemyAttackedInfoText = "Enemy attacked!";

    [Header("Dependencies")]
    public CombatUI combatUI;
    public InventorySO playerInventory;

    // Private

    public CombatStates _combatState = CombatStates.NONE;
    private int _gold = 0;

    private CombatRequest _request;
    private CombatUnitSO _currentEnemy;
    private GameObject _currentEnemyGO;

    public void SetupCombat(CombatRequest request)
    {
        combatUI._playerInventory = this.playerInventory;
        // Save references for later
        this._request = request;

        // Instantiate player
        GameObject playerGO = Instantiate(this._request.player.unitPrefab, request.playerPosition.position, Quaternion.identity);
        playerGO.transform.parent = request.playerPosition;

        // Start combat
        StartCoroutine(StartCombat());
        combatUI.PopulateConsumables();
        combatUI.PopulateWeapons();
        combatUI.InstantiateWeaponAndShield();
        combatUI.InstantiateConsumables();
    }

    public void NextCombat()
    {
        StartCoroutine(StartCombat());
    }

    public void ResetCombat()
    {
        this._gold = 0;
        StartCoroutine(StartCombat());
    }

    /*public void OnPlayerUsedItem(int inventoryItemId)
    {
        if (this._combatState != CombatStates.PLAYERTURN)
            return;

        if (inventoryItemId == 0 || inventoryItemId == 1) // Player used a weapon
        {
            if (this._request.player.inventory.weapons.Count > inventoryItemId)
            {
                var usedWeapon = this._request.player.inventory.weapons[inventoryItemId];

                this._request.player.AttackUnit(this._currentEnemy, usedWeapon.weapon, this.playerInventory.playerUnit.attackPower);

                if (this._currentEnemy.currentHP <= 0f) // Enemy is dead
                {
                    StartCoroutine(CombatWon());
                }
                else
                {
                    StartCoroutine(EnemyTurn());
                }
            }
        }

        else if (inventoryItemId == 2 || inventoryItemId == 3) 
        {
            var consumableIndex = inventoryItemId % 2;

            if (this._request.player.inventory.consumables.Count > consumableIndex)
            {
                var usedConsumable = this._request.player.inventory.consumables[consumableIndex];

                this._request.player.Heal(usedConsumable.item.healPower);
                this._request.player.inventory.RemoveConsumable(usedConsumable.item, 1);
            }

            StartCoroutine(EnemyTurn());
        }
    }*/


    // Combat status

    public IEnumerator StartCombat()
    {
        this._combatState = CombatStates.START;


        int randomNumber = Random.Range(0, this._request.enemies.Length);
        this._currentEnemy = this._request.enemies[randomNumber];
        combatUI.SetBattlePlayers(this._currentEnemy, this._request.player);

        this._currentEnemy.currentHP = this._currentEnemy.maxHP;

        // Instantiate enemy
        this._currentEnemyGO = Instantiate(this._currentEnemy.unitPrefab, this._request.enemyPosition.position, Quaternion.identity);
        this._currentEnemyGO.transform.parent = this._request.enemyPosition;

        // Configure HUD
        this.combatUI.ResetHUD();
        this.combatUI.ShowCombatMenu();
        this.combatUI.SetupHUD(this._request.player, this._currentEnemy, this._currentEnemy.currentLevel, this._gold);
        this.combatUI.SetInfoText(combatStartedInfoText);

        yield return new WaitForSeconds(this.timeBetweenActions);

        PlayerTurn();
    }

    public void PlayerTurn()
    {
        this._combatState = CombatStates.PLAYERTURN;
        this.combatUI.SetInfoText(playerTurnInfoText);

        // Wait until player attacks
    }

    public IEnumerator EnemyTurn()
    {
        this._combatState = CombatStates.ENEMYTURN;
        this.combatUI.SetInfoText(enemyTurnInfoText);

        yield return new WaitForSeconds(this.timeBetweenActions);

        // Enemy attacks
        this.combatUI.SetInfoText(enemyAttackedInfoText);

        var usedWeapon = this._currentEnemy.inventory.weapons[0]; // Always the first weapon for the enemy, change if you like
        this._currentEnemy.AttackUnit(this._request.player, usedWeapon.weapon, this._currentEnemy.attackPower);

        yield return new WaitForSeconds(this.timeBetweenActions);

        if (this._request.player.currentHP <= 0f) // Player is dead
        {
            CombatLost();
        }
        else
        {
            PlayerTurn();
        }
    }

    public IEnumerator CombatWon()
    {
        this._combatState = CombatStates.WON;

        // Set UI text
        this.combatUI.SetInfoText(playerWonInfoText);

        // Get some money for your win
        var earnedGold = (int)(this._currentEnemy.gold);
        this._gold += earnedGold;
        float experience = this._currentEnemy.giveExperience;
        this.playerInventory.playerUnit.currentExperience += experience;
        this.playerInventory.gold += earnedGold;
        this.playerInventory.playerUnit.TryToLevelUp();
        // Wait a bit
        yield return new WaitForSeconds(this.timeBetweenActions);

        // And show the win menu
        this.combatUI.ShowWonMenu(this._gold);
        this.ResetEnemysHPToBase();
        Destroy(this._currentEnemyGO);
        this._currentEnemyGO = null;
    }

    public void CombatLost()
    {
        this._combatState = CombatStates.LOST;

        this.combatUI.ShowLostMenu();
        this.ResetEnemysHPToBase();
    }

    public void FinishCombat()
    {
        this._request.player.inventory.AddGold(this._gold);
    }

    private void ResetEnemysHPToBase()
    {
        this._currentEnemy.maxHP = this._currentEnemy.baseHP;
    }
}
