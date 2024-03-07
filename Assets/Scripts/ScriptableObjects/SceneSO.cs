using UnityEngine;

[CreateAssetMenu(fileName = "NewScene", menuName = "Scriptable Objects/Scene")]
public class SceneSO : ScriptableObject
{
    [Header("Scene Information")]
    public string sceneName;
    [Header("Scene Loading point")]
    public LevelEntranceSO levelEntrance;
}
