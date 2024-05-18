using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        // Play the cutscene video
        videoPlayer.Play();

        // Wait for the video to finish playing
        videoPlayer.loopPointReached += LoadMainMenuScene;
    }

    private void LoadMainMenuScene(VideoPlayer vp)
    {
        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }
}
