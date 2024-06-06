using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
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

        [HideInInspector] public float originalVolume;
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

    #region  Volume Functions
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

    #endregion

    #region Background Music Functions

    // Stop semua musik latar belakang
    public void StopAllBackgroundMusic()
    {
        foreach (BackgroundMusicGroup group in audioBackgroundMusicGroups)
        {
            foreach (AudioSource audioSource in group.backgroundMusics)
            {
                StartCoroutine(FadeOutAndStop(audioSource, 0.6f));
            }
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Kembalikan volume ke nilai awal jika diinginkan
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

    //pause background music
    public void PauseBackgroundMusic(string groupName)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null)
        {
            group.originalVolume = group.backgroundMusics[0].volume;
            StartCoroutine(FadeOutAndPause(group, 1f));
        }
    }

    //resume background music with transition
    public void ResumeBackgroundMusic(string groupName)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null)
        {
            StartCoroutine(FadeInAndResume(group, 1f));
        }
    }

    private IEnumerator FadeOutAndPause(BackgroundMusicGroup group, float duration)
    {
        float currentTime = 0;
        float startVolume = group.backgroundMusics[0].volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            foreach (AudioSource audioSource in group.backgroundMusics)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            }
            yield return null;
        }

        foreach (AudioSource audioSource in group.backgroundMusics)
        {
            audioSource.Pause();
        }
    }

    private IEnumerator FadeInAndResume(BackgroundMusicGroup group, float duration)
    {
        foreach (AudioSource audioSource in group.backgroundMusics)
        {
            audioSource.UnPause();
        }

        float currentTime = 0;
        float startVolume = group.originalVolume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            foreach (AudioSource audioSource in group.backgroundMusics)
            {
                audioSource.volume = Mathf.Lerp(0, startVolume, currentTime / duration);
            }
            yield return null;
        }
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


    // Fungsi untuk transisi ke musik latar belakang berdasarkan scene
    public void TransitionToBackgroundMusic()
    {
        StopBackgroundMusicWithTransition("Battle", 1f);
        ResumeBackgroundMusic(GetCurrentSceneMusic());
    }


    //fungsi untuk transisi ke musik battle berdasarkan scene
    public void TransitionToBattleMusic()
    {
        PauseBackgroundMusic(GetCurrentSceneMusic());
        PlayBackgroundMusicWithTransition("Battle", 0, 1f);
    }

    private string GetCurrentSceneMusic()
    {
        // Mengembalikan nama musik yang sesuai dengan scene saat ini
        switch (SceneManager.GetActiveScene().name)
        {
            case "Gameplay1":
                return "GameJakarta";
            case "Gameplay2":
                return "GameInvert";
            case "Gameplay3":
                return "GameBandung";
            default:
                return "DefaultMusic"; // Tambahkan nilai default untuk menghindari error
        }
    }

    #endregion



    #region SFX Functions

    public void PlaySFX(string groupName, int index)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.soundEffects.Length)
        {
            group.soundEffects[index].Play();
        }
    }

    public void StopSFX(string groupName, int index)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.soundEffects.Length)
        {
            group.soundEffects[index].Stop();
        }
    }

    //stop sfx group

    public void StopSFXGroup(string groupName)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null)
        {
            foreach (AudioSource sfx in group.soundEffects)
            {
                sfx.Stop();
            }
        }
    }

    // Fungsi untuk memainkan suara efek dengan rate 10% untuk boss yang kena hit 
    public void PlaySFXWithChance(string groupName, int index, float chance)
    {
        if (Random.value <= chance)
        {
            PlaySFX(groupName, index);
        }
    }

    // Stop semua suara efek
    public void StopSpesificSFX(){
        StopSFXGroup("AttackPlayer");
        StopSFXGroup("Skillplayer");
        StopSFXGroup("RangedAttack");
        StopSFXGroup("SkillBoss");
        StopSFXGroup("BossDukun");

    }

    // Example usage for boss hit with a 10% chance
    public void PlayBossHitSFX(string groupName, int index)
    {
        PlaySFXWithChance(groupName, index, 0.1f);
    }
    
    #endregion

    #region Mute Functions

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

    #endregion


    #region Sound Effect Group Functions

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

    public void PauseSFX(){
        PauseSoundEffectGroup("AttackPlayer");
        PauseSoundEffectGroup("Skillplayer");
        PauseSoundEffectGroup("RangedAttack");
        PauseSoundEffectGroup("SkillBoss");
        PauseSoundEffectGroup("BossDukun");
    }

    public void ResumeSFX(){
        ResumeSoundEffectGroup("AttackPlayer");
        ResumeSoundEffectGroup("Skillplayer");
        ResumeSoundEffectGroup("RangedAttack");
        PauseSoundEffectGroup("SkillBoss");
        PauseSoundEffectGroup("BossDukun");
    }

    #endregion

}


    

