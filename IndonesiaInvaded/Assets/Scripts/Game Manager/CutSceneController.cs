using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    public GameObject afterPortalGO;  // Referensi ke GameObject yang berisi Video Player 1
    public GameObject beforePortalGO;
    public GameObject[] obj;
    private VideoPlayer afterPortal;
    private VideoPlayer beforePortal;
    private VideoPlayer activeVideoPlayer;
    private bool isCutscenePlaying = false;

    void Start()
    {
        afterPortal = afterPortalGO.GetComponent<VideoPlayer>();
        beforePortal = beforePortalGO.GetComponent<VideoPlayer>();
        // Pastikan VideoPlayer tidak null
        if (afterPortal != null)
        {
            afterPortal.loopPointReached += EndReached;  // Event ketika video 1 selesai
            afterPortalGO.SetActive(false);  // Nonaktifkan video player 1 di awal
        }

        if (beforePortal != null)
        {
            beforePortal.loopPointReached += EndReached;  // Event ketika video 2 selesai
            beforePortalGO.SetActive(false);  // Nonaktifkan video player 2 di awal
        }
    }

    public void StartCutscene(int videoIndex)
    {
        if (videoIndex == 1 && afterPortal != null)
        {
            StartCoroutine(PlayCutscene(afterPortal));
        }
        else if (videoIndex == 2 && beforePortal != null)
        {
            StartCoroutine(PlayCutscene(beforePortal));
        }
    }

    IEnumerator PlayCutscene(VideoPlayer videoPlayer)
    {
        Time.timeScale = 0f;  // Menghentikan waktu
        foreach (GameObject go in obj)
        {
            go.SetActive(false);
        }
        videoPlayer.gameObject.SetActive(true);  // Mengaktifkan video player yang dipilih
        videoPlayer.Play();  // Memulai video
        isCutscenePlaying = true;
        activeVideoPlayer = videoPlayer;

        // Tunggu sampai video selesai
        while (isCutscenePlaying)
        {
            yield return null;
        }

        // Mengembalikan ke keadaan semula
        Time.timeScale = 1f;
        foreach (GameObject go in obj)
        {
            go.SetActive(true);
        }
        videoPlayer.gameObject.SetActive(false);
    }

    void EndReached(VideoPlayer vp)
    {
        isCutscenePlaying = false;
    }
}
