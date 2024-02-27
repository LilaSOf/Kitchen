using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    // Start is called before the first frame update
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
   /// <summary>
   /// ���ñ������ֵ�����
   /// </summary>
   public void SetMenuMusic()
    {
        audioSource.volume += 0.1f;
        if(audioSource.volume >= 1)
        {
            audioSource.volume = 0;
        }
    }

    /// <summary>
    /// ��ȡ��ǰ���ֵ�������ֵ
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        return audioSource.volume;
    }
}
