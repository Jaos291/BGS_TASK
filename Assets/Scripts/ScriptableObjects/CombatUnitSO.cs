using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Scriptable Objects/Combat/Unit")]
public class CombatUnitSO : ScriptableObject
{
    public Sprite[] sprites;

    public string unitName;

    public float baseHP;
    public float maxHP;
    public float currentHP;
    public float baseMana;
    public float maxMana;
    public float currentMana;

    [Header("Attack and Defense with items")]
    public float attackPower;
    public float magicPower;
    public float defense;
    public float magicResist;
    public float agility;
    public float accuracy;

    [Header("Base attack and defense items according to level")]
    public float baseAttackPower;
    public float baseMagicPower;
    public float baseDefense;
    public float baseMagicResist;
    public float baseAgility;
    public float baseAccuracy;

    [Header("Level and Experience")]
    public float currentLevel;
    public float currentExperience;
    public float experienceForNextLevel;

    public GameObject unitPrefab;

    public InventorySO inventory;

    [Header("Enemy info")]
    public float giveExperience;
    public float gold;

    public void AttackUnit(CombatUnitSO other, ItemWeaponSO weapon, float attackPower)
    {
        float damage = weapon.attackPower + attackPower;
        other.TakeDamage(damage);
    }
    public List<string> ReturnPlayerStats()
    {
        List<string> stats = new List<string>();
        stats.Clear();
        stats.Add(baseHP.ToString());
        stats.Add(maxHP.ToString());
        stats.Add(currentHP.ToString());
        stats.Add(baseMana.ToString());
        stats.Add(maxMana.ToString());
        stats.Add(currentMana.ToString());
        stats.Add(attackPower.ToString());
        stats.Add(magicPower.ToString());
        stats.Add(defense.ToString());
        stats.Add(magicResist.ToString());
        stats.Add(agility.ToString());
        stats.Add(accuracy.ToString());
        stats.Add(baseAttackPower.ToString());
        stats.Add(baseMagicPower.ToString());
        stats.Add(baseDefense.ToString());
        stats.Add(baseMagicResist.ToString());
        stats.Add(baseAgility.ToString());
        stats.Add(baseAccuracy.ToString());
        stats.Add(currentLevel.ToString());
        stats.Add(currentExperience.ToString());
        stats.Add(experienceForNextLevel.ToString());
        return stats;
    }
    public void LoadPlayerStats(List<string> list)
    {
        int i = 0;
        List<string> stats = new List<string>();
        stats = list;
        baseHP = int.Parse(stats[i]);
        i++;
        maxHP = int.Parse(stats[i]);
        i++;
        baseMana = int.Parse(stats[i]);
        i++;
        maxMana = int.Parse(stats[i]);
        i++;
        currentMana = int.Parse(stats[i]);
        i++;
        attackPower = int.Parse(stats[i]);
        i++;
        magicPower = int.Parse(stats[i]);
        i++;
        defense = int.Parse(stats[i]);
        i++;
        magicResist = int.Parse(stats[i]);
        i++;
        agility = int.Parse(stats[i]);
        i++;
        accuracy = int.Parse(stats[i]);
        i++;
        baseAttackPower = int.Parse(stats[i]);
        i++;
        baseMagicPower = int.Parse(stats[i]);
        i++;
        baseDefense = int.Parse(stats[i]);
        i++;
        baseMagicResist = int.Parse(stats[i]);
        i++;
        baseAgility = int.Parse(stats[i]);
        i++;
        baseAccuracy = int.Parse(stats[i]);
        i++;
        currentLevel = int.Parse(stats[i]);
        i++;
        currentExperience = int.Parse(stats[i]);
        i++;
        experienceForNextLevel = int.Parse(stats[i]);
        i++;
    }
    public void TakeDamage(float damage)
    {
        float totalDamage = damage - (damage * (defense/100));
        this.currentHP -= totalDamage;

        if (this.currentHP < 0f)
            this.currentHP = 0f;
    }

    public void Heal(float heal)
    {
        this.currentHP += heal;

        if (this.currentHP > this.maxHP)
            this.currentHP = this.maxHP;
    }

    private void OnDisable()
    {
        this.maxHP = this.baseHP;
    }

    public float GiveExperience()
    {
        return giveExperience;
    }
    public void TryToLevelUp()
    {
        experienceForNextLevel = currentLevel * 100f * 1.25f;
        if (currentExperience >= experienceForNextLevel)
        {
            currentLevel += 1;
        }
    }
}
