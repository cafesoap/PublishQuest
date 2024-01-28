using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MySceneManager : MonoBehaviour
{
    public GameObject ui;

    public GameObject video;
    public VideoPlayer videoPlayer;
    public string nextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        video.SetActive(true);
        ui.SetActive(false);
       
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        // �������д��Ƶ������ɺ���߼���������ת����һ������
         SceneManager.LoadScene(nextSceneName);
    }
}
