using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "SaveStateSO")]
	public sealed class SaveStateSOGameEventListener : BaseGameEventListener<SaveStateSO, SaveStateSOGameEvent, SaveStateSOUnityEvent>
	{
	}
}