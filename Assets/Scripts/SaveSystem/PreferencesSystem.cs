using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName ="PreferencesSystem", menuName ="Scriptable Objects/Preferences System")]
public class PreferencesSystem : ScriptableObject
{
    [Header("Preferences")]
    public float volume;

    [Header("Dependencies")]
    public AudioMixer mixer;

    private const string VOLUME_KEY = "volume";

    public void LoadPrefs()
    {
        this.LoadVolume();
    }

    public void VolumeChanged(float value)
    {
        this.SaveVolume(value);
        this.LoadVolume();
    }

    private void SaveVolume(float value)
    {
        this.volume = value;
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.Save(); 
    }

    private void LoadVolume() 
    {
        this.volume=PlayerPrefs.GetFloat(VOLUME_KEY,1f);
        this.mixer?.SetFloat("MasterVolume", Mathf.Log10(this.volume)*20);
    }

}