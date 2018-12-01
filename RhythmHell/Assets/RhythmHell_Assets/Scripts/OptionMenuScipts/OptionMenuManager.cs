using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]
public class OptionMenuManager : MonoBehaviour {

    [Header("Scene Variables")]
    public string mainMenuScene;
    public float defaultSfxVolume = 0f;
    public float defaultMusicVolume = 0f;

    private float currentSfxVolume;
    private float currentMusicVolume;

    private float lastSfxUpdate;
    private float lastMusicUpdate;

    [Header("Mixer References")]
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    [Header("GUI References")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        float musicVol;
        musicMixer.GetFloat("volume", out musicVol);
        musicSlider.value = musicVol;

        float sfxVol;
        sfxMixer.GetFloat("volume", out sfxVol);
        sfxSlider.value = sfxVol;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void AdjustSfxVolume()
    {
        float sfxVolume = sfxSlider.value;
        sfxMixer.SetFloat("volume", sfxVolume);
    }

    public void AdjustMusicVolume()
    {
        float musicVolume = musicSlider.value;
        musicMixer.SetFloat("volume", musicVolume);
    }

}
