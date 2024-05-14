using System.Collections;
using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager instance;

    [Header("Default Slow Motion Settings")]
    public float defaultSlowMotionScale = 0.2f; // Default skala waktu saat slow motion
    public float defaultSlowMotionDuration = 2f; // Default durasi slow motion
    public float waitBeforeSlowMotion = 0.5f; // Waktu tunggu sebelum slow motion dimulai

    private float originalTimeScale;
    private float originalFixedDeltaTime;
    private bool isSlowMotionActive = false;

    private void Awake()
    {
        // Pastikan hanya ada satu instance dari SlowMotionManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Membuat objek ini tidak dihancurkan saat memuat scene baru
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        originalTimeScale = Time.timeScale;
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void StartSlowMotion()
    {
        if (!isSlowMotionActive)
        {
            StartCoroutine(HandleSlowMotion(defaultSlowMotionScale, defaultSlowMotionDuration));
        }
    }

    public void StartSlowMotion(float scale, float duration)
    {
        if (!isSlowMotionActive)
        {
            StartCoroutine(HandleSlowMotion(scale, duration));
        }
    }

    private IEnumerator HandleSlowMotion(float scale, float duration)
    {
        yield return new WaitForSeconds(waitBeforeSlowMotion);

        // Mengaktifkan slow motion
        Time.timeScale = scale;
        Time.fixedDeltaTime = originalFixedDeltaTime * scale;
        isSlowMotionActive = true;

        yield return new WaitForSecondsRealtime(duration);

        StopSlowMotion();
    }

    public void StopSlowMotion()
    {
        // Menonaktifkan slow motion
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        isSlowMotionActive = false;
    }
}
