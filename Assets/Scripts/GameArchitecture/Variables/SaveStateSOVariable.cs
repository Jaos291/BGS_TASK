using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class SaveStateSOEvent : UnityEvent<SaveStateSO> { }

	[CreateAssetMenu(
	    fileName = "SaveStateSOVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Save Request",
	    order = 120)]
	public class SaveStateSOVariable : BaseVariable<SaveStateSO, SaveStateSOEvent>
	{
	}
}