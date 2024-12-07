using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour {

	[Header("Audio")]
	public AudioMixer mixer;
	//public AudioMixerGroup master;
	//public Slider masterSlider;
	public AudioMixerGroup music;
	public Slider musicSlider;
	public AudioMixerGroup SFX;
	public Slider sfxSlider;
	public AudioMixerGroup voices;
	public Slider voicesSlider;
	[Header("Quality")]
	public TMP_Dropdown qualityDropdown;

	private float volumenMusic;
	private float volumenVoice;
	public PlayerSaveData saveData;

	public AudioSource audioSource;

	void Awake() 
	{
		ConfigDropdown();
		Invoke(nameof(ChangeMusicStart), .1f);
	}

	private void ChangeMusicStart()
    {
		audioSource.mute = false;
		ChangeMusic(saveData.volumen);
	}

	private void ConfigDropdown() {
		qualityDropdown.options.Clear();
		string[] names = QualitySettings.names;
		foreach(string name in names) {
			qualityDropdown.options.Add(new TMP_Dropdown.OptionData(name));
		}
		qualityDropdown.value = QualitySettings.GetQualityLevel();
	}

    private void Start()
    {
		ShowVolumes();
		Debug.Log("Entra aquí");
	}

    private void ShowVolumes() {
		//mixer.GetFloat(master.name, out float value);
		//masterSlider.value = GetVolumeLog(value);
		//mixer.GetFloat(music.name, out value);

		//mixer.GetFloat(SFX.name, out value);
		//sfxSlider.value = GetVolumeLog(value);
		//mixer.GetFloat(voices.name, out value);

		Debug.Log(saveData.volumen);




		musicSlider.value = saveData.volumen;
		voicesSlider.value = saveData.volumenVoice;
	}




	//public void ChangeMaster(float volume) => mixer.SetFloat(master.name, SetVolumeLog(volume));
	public void ChangeMusic(float volume)
	{
		mixer.SetFloat(music.name, SetVolumeLog(volume));
		ButtonManager.buttonManager.saveDataPreset.volumen = volume;
		ButtonManager.buttonManager.saveDataPreset.Save();
	}


	public void ChangeVoices(float volume)
	{
		ButtonManager.buttonManager.saveDataPreset.volumenVoice = volume;
		mixer.SetFloat(voices.name, SetVolumeLog(volume));
		ButtonManager.buttonManager.saveDataPreset.Save();
	}





	//public void ChangeMusic(float volume) => mixer.SetFloat(music.name, SetVolumeLog(volume));

	public void ChangeSFX(float volume) => mixer.SetFloat(SFX.name, SetVolumeLog(volume));

	//public void ChangeVoices(float volume) => mixer.SetFloat(voices.name, SetVolumeLog(volume));

	//public void ChangeQuality(int quality) => QualitySettings.SetQualityLevel(quality, true);
	public void ChangeQuality(int quality)
	{
		PlayerPrefs.SetInt("quality", quality);
		QualitySettings.SetQualityLevel(quality, true);
	}

	private float SetVolumeLog(float volume) => Mathf.Clamp(20 * Mathf.Log10(volume), -80f, 0f);

	private float GetVolumeLog(float volume) => Mathf.Clamp01(20 / Mathf.Pow(10f, volume));


	[System.Serializable]
	public struct GameConfig {

		// volume
		public float master;
		public float music;
		public float SFX;
		public float voices;
		// quality
		public int quality;
	}
}
