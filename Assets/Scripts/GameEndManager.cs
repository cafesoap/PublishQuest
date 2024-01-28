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
        // ��ʼ����������ص�һ����Ƶ
      //  PlayVideo("Video1");
    }

    void OnEnable()
    {
        // ������Ƶ������ɵ��¼�
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDisable()
    {
        // ȡ��������Ƶ������ɵ��¼��������ظ�����
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    public void PlayVideo(string videoName)
    {

        // ������Ƶ�����л���ͬ����Ƶ
        string videoPath = "Videos/" + videoName; // Resources.Load ����Ҫ�ļ���չ��
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
        // ��Ƶ������ɺ���ʾ�ؿ���ť
        ShowRestartButton();
    }

    void ShowRestartButton()
    {
        // ��ʾ�ؿ���ť
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGameFunction()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
