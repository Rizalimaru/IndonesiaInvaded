using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{

    public static AudioSetting instance;
    public Slider sliderMasterVolume;
    [SerializeField] private Slider sliderBackgroundMusic;
    [SerializeField] private Slider sliderSoundEffect;

    private float previousMasterVolume;

    public Sprite[] spritemute;
    public Button buttonMute;

    private AudioManager audioManagerInstance;

    public static AudioSetting Instance { get; set;}

    private void Start()
    { 
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    private void Awake()
    {
        audioManagerInstance = AudioManager.Instance;

        if (audioManagerInstance != null)
        {
            sliderMasterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            sliderBackgroundMusic.value = PlayerPrefs.GetFloat("BackgroundMusic", 1.0f);
            sliderSoundEffect.value = PlayerPrefs.GetFloat("SoundEffect", 1.0f);

            // Mengupdate sprite button mute/unmute
            if (audioManagerInstance.IsMasterMuted())
            {
                buttonMute.image.sprite = spritemute[1];
            }
            else
            {
                buttonMute.image.sprite = spritemute[0];
            }
            // Menambahkan listener pada slider
            sliderMasterVolume.onValueChanged.AddListener(SetMasterVolume);
            sliderBackgroundMusic.onValueChanged.AddListener(audioManagerInstance.SetBackgroundMusic);
            sliderSoundEffect.onValueChanged.AddListener(audioManagerInstance.SetSoundEffect);

        }
        else
        {
            Debug.LogWarning("AudioManager instance not found.");
        }   

        // Singleton pattern untuk instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Sceneloader(int SceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIndex);
    }
    
    public void PlaySFXSound()
    {
        audioManagerInstance.PlaySFX("UISFX", 0);
    }

    public void PlaySFXButtonClicked()
    {
        audioManagerInstance.PlaySFX("Button", 1);
    }

    public void PlayPauseSoundEffect()
    {
        audioManagerInstance.PlaySFX("Pause", 0);
    }

    public void PlaySFXCollision()
    {
        audioManagerInstance.PlaySFX("Collision", 0);
    }

    // Stop Background Music




    // Set master volume berdasarkan slider value

    public void SetMasterVolume(float sliderValue)
    {
        audioManagerInstance.SetMasterVolume(sliderValue);
        if (sliderValue <= 0.0001f)
        {
            buttonMute.image.sprite = spritemute[1]; // Sprite untuk status mute
        }
        else
        {
            buttonMute.image.sprite = spritemute[0]; // Sprite untuk status unmute
        }
    }

    public void ButtonMute()
    {
        if (buttonMute.image != null)
        {
            audioManagerInstance.ToggleMasterMute();
            if (audioManagerInstance.IsMasterMuted())
            {
                buttonMute.image.sprite = spritemute[1]; // Sprite untuk status mute
                previousMasterVolume = sliderMasterVolume.value;
                sliderMasterVolume.value = 0.0001f; // Atur slider ke nilai minimum jika mute
            }
            else
            {
                sliderMasterVolume.value = previousMasterVolume;
                if (previousMasterVolume <= 0.0001f)
                {
                    previousMasterVolume = 1f;
                }
                buttonMute.image.sprite = spritemute[0]; // Sprite untuk status unmute
                sliderMasterVolume.value = previousMasterVolume;
                sliderMasterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 0.3f); // Atur slider ke nilai sebelumnya jika unmute
            }
        }
    }  
    
}
