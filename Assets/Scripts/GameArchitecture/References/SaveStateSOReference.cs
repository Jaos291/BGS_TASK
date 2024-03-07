using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class SaveStateSOReference : BaseReference<SaveStateSO, SaveStateSOVariable>
	{
	    public SaveStateSOReference() : base() { }
	    public SaveStateSOReference(SaveStateSO value) : base(value) { }
	}
}