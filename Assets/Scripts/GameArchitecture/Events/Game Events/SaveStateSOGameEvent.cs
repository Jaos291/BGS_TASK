using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "SaveStateGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Save Request",
	    order = 120)]
	public sealed class SaveStateSOGameEvent : GameEventBase<SaveStateSO>
	{
	}
}