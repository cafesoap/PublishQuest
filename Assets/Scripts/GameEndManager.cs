using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameEndManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button restartButton;
    public GameObject gameUI;

    void Start()
    {
        // 初始化，例如加载第一个视频
      //  PlayVideo("Video1");
    }

    void OnEnable()
    {
        // 订阅视频播放完成的事件
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDisable()
    {
        // 取消订阅视频播放完成的事件，以免重复订阅
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    public void PlayVideo(string videoName)
    {

        // 根据视频名称切换不同的视频
        string videoPath = "Videos/" + videoName; // Resources.Load 不需要文件扩展名
        VideoClip videoClip = Resources.Load<VideoClip>(videoPath);
        gameUI.SetActive(false);
        if (videoClip != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("Video not found: " + videoPath);
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // 视频播放完成后显示重开按钮
        ShowRestartButton();
    }

    void ShowRestartButton()
    {
        // 显示重开按钮
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGameFunction()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
