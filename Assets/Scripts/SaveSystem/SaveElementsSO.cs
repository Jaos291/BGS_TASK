using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSaveElements", menuName = "Scriptable Objects/Save")]
public class SaveElementsSO : ScriptableObject
{
    public ItemWeaponSO[] weapons;
    public ItemConsumableSO[] consumables;
}
