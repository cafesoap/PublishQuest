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
        // ��ȡ��ť���
        Button button = GetComponent<Button>();

        // �ڰ�ť����ӵ��������
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        // ���ŵ����Ч
        audioSource.PlayOneShot(clickSound);
    }
}
