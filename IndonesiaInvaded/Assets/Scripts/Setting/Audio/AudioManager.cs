using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    private Dictionary<AudioSource, bool> soundEffectStatus = new Dictionary<AudioSource, bool>();

    // Membuat kelas untuk menyimpan grup sound effect

    [System.Serializable]
    public class SoundEffectGroup
    {
        public string groupName;
        public AudioSource[] soundEffects;
    }

    public SoundEffectGroup[] audioSFXGroups;

    // Membuat kelas untuk menyimpan grup background music
    [System.Serializable]
    public class BackgroundMusicGroup
    {
        public string groupName;
        public AudioSource[] backgroundMusics;
    }

    public BackgroundMusicGroup[] audioBackgroundMusicGroups;

    public AudioMixer audioMixer;

    // Menyiapkan variabel untuk menyimpan nilai volume sebelumnya

    private AudioSetting AudioSettingInstance;


    // Menyiapkan key untuk menyimpan nilai volume pada PlayerPrefs
    private const string MasterVolumeKey = "MasterVolume";
    private const string BackgroundMusicKey = "BackgroundMusic";
    private const string SoundEffectKey = "SoundEffect";

    private bool isSoundEffectsPaused = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        // Set nilai AudioMixer berdasarkan PlayerPrefs
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat(MasterVolumeKey, 1f)) * 20);
        audioMixer.SetFloat("BackgroundMusic", Mathf.Log10(PlayerPrefs.GetFloat(BackgroundMusicKey, 1f)) * 20);
        audioMixer.SetFloat("SoundEffect", Mathf.Log10(PlayerPrefs.GetFloat(SoundEffectKey, 1f)) * 20);
    }


    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(MasterVolumeKey, sliderValue);
        PlayerPrefs.Save();
    }

    public void SetBackgroundMusic(float sliderValue)
    {
        audioMixer.SetFloat("BackgroundMusic", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(BackgroundMusicKey, sliderValue);
        PlayerPrefs.Save();
    }

    public void SetSoundEffect(float sliderValue)
    {
        audioMixer.SetFloat("SoundEffect", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(SoundEffectKey, sliderValue);
        PlayerPrefs.Save();
    }

    public void PlayBackgroundMusicWithTransition(string groupName, int index, float fadeInDuration)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.backgroundMusics.Length)
        {
            StartCoroutine(FadeInAndPlayBackgroundMusic(group.backgroundMusics[index], fadeInDuration));
        }
        else
        {
            Debug.LogWarning("Background music group or index not found.");
        }
    }

    IEnumerator FadeInAndPlayBackgroundMusic(AudioSource audioSource, float fadeInDuration)
    {
        // Simpan volume awal
        float startVolume = 0f;

        // Set volume awal ke 0 untuk fade in
        audioSource.volume = startVolume;

        // Hitung target volume
        float targetVolume = audioSource.volume;

        // Hitung increment per frame
        float deltaVolume = 1f / fadeInDuration;

        // Fade in musik dengan menambahkan volume setiap frame
        while (audioSource.volume < 1)
        {
            audioSource.volume += deltaVolume * Time.deltaTime;
            yield return null;
        }

        // Pastikan volume tidak melebihi 1
        audioSource.volume = Mathf.Clamp(audioSource.volume, 0f, 1f);

        // Mainkan musik setelah selesai fade in
        audioSource.Play();
    }

    public void StopBackgroundMusicWithTransition(string groupName, float fadeOutDuration)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null)
        {
            foreach (AudioSource audioSource in group.backgroundMusics)
            {
                StartCoroutine(FadeOutBackgroundMusic(audioSource, fadeOutDuration));
            }
        }
        else
        {
            Debug.LogWarning("Background music group not found.");
        }
    }

    IEnumerator FadeOutBackgroundMusic(AudioSource audioSource, float fadeOutDuration)
    {
        // Simpan volume awal
        float startVolume = audioSource.volume;

        // Hitung target volume
        float targetVolume = 0f;

        // Hitung increment per frame
        float deltaVolume = startVolume / fadeOutDuration;

        // Fade out musik dengan mengurangi volume setiap frame
        while (audioSource.volume > 0)
        {
            audioSource.volume -= deltaVolume * Time.deltaTime;
            yield return null;
        }

        // Pastikan volume tidak negatif
        audioSource.volume = 0f;

        // Hentikan musik setelah selesai fade out
        audioSource.Stop();
    }

    public void PlaySFX(string groupName, int index)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.soundEffects.Length)
        {
            group.soundEffects[index].Play();
        }
        else
        {
            Debug.LogWarning("Sound effect group or index not found.");
        }
    }


    public void ToggleMasterMute()
    {
        float currentVolume = 0f;
        audioMixer.GetFloat("MasterVolume", out currentVolume);

        if (currentVolume <= -80f)
        {
            // Toggle mute menjadi tidak terdengar
            audioMixer.SetFloat("MasterVolume", 0f);
        }
        else
        {
            // Toggle mute menjadi terdengar
            audioMixer.SetFloat("MasterVolume", -80f);
        }

        PlayerPrefs.SetFloat(MasterVolumeKey, currentVolume <= -80f ? 0f : currentVolume);
        PlayerPrefs.Save();
    }

    public bool IsMasterMuted()
    {
        float currentVolume = 0f;
        audioMixer.GetFloat("MasterVolume", out currentVolume);
        return currentVolume <= -80f;
    }



    public void PauseSoundEffectGroup(string groupName)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null)
        {
            foreach (AudioSource sfx in group.soundEffects)
            {
                if (sfx.isPlaying)
                {
                    soundEffectStatus[sfx] = true; // Menyimpan status play/pause sebelumnya
                    sfx.Pause();
                }
            }
        }
        else
        {
            Debug.LogWarning("Sound effect group not found.");
        }
    }

    public void ResumeSoundEffectGroup(string groupName)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null)
        {
            foreach (AudioSource sfx in group.soundEffects)
            {
                if (soundEffectStatus.ContainsKey(sfx) && soundEffectStatus[sfx])
                {
                    sfx.UnPause(); // Mengembalikan status play/pause sebelumnya
                }
            }
        }
        else
        {
            Debug.LogWarning("Sound effect group not found.");
        }
    }


    
}
