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
   /// 设置背景音乐的音量
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
    /// 获取当前音乐的音量数值
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        return audioSource.volume;
    }
}
