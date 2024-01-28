using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Start()
    {
        // 获取按钮组件
        Button button = GetComponent<Button>();

        // 在按钮上添加点击监听器
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        // 播放点击音效
        audioSource.PlayOneShot(clickSound);
    }
}
