using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    // Start is called before the first frame update
    [SerializeField] private AudioData_SO audioData;

    private float volumeParameter = 1f;
    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        DeliveryManage.Instance.DeliverySuccess += OnDeliverySuccessEvnet;
        DeliveryManage.Instance.DeliveryFailure += OnDeliveryFailureEvnet;
        Player.PlayerPickingUP += OnPlayerPickUpItemEvent;
        CuttingCounter.OnAnyCut += OnAnyCutEvent;
        BaseCounter.KitchenObjectDropEvent += OnKitchenObjectDropEvent;
        TrashCounter.TranshedEvent += OnTranshedEvent;
    }

    private void OnTranshedEvent(object sender, System.EventArgs e)
    {
       TrashCounter trashCounter = sender as TrashCounter;
        PlayAudio(audioData.trash, trashCounter.transform.position);
    }

    private void OnKitchenObjectDropEvent(object sender, System.EventArgs e)
    {
       BaseCounter baseCounter = sender as BaseCounter;
        PlayAudio(audioData.objectDrop, baseCounter.transform.position);
    }

    private void OnAnyCutEvent(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlayAudio(audioData.chop, cuttingCounter.transform.position);
    }

    private void OnPlayerPickUpItemEvent(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlayAudio(audioData.objectPick, player.transform.position);
    }

    private void OnDeliverySuccessEvnet(object sender, System.EventArgs e)
    {
        PlayAudio(audioData.deliveryScucess, DeliveryCounter.Instance.transform.position);
    }

    private void OnDeliveryFailureEvnet(object sender, System.EventArgs e)
    {
        PlayAudio(audioData.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    /// <summary>
    /// 播放组件音乐
    /// </summary>
    /// <param name="audioClipArray"></param>
    /// <param name="pos"></param>
    /// <param name="volume"></param>
    private void PlayAudio(AudioClip[] audioClipArray, Vector3 pos, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0,audioClipArray.Length)], pos, volume * volumeParameter);
    }


    /// <summary>
    /// 播放单个音乐
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="pos"></param>
    /// <param name="volume"></param>
    private void PlayAudio(AudioClip audioClip,Vector3 pos,float volume = 1)
    {
        AudioSource.PlayClipAtPoint(audioClip, pos, volume * volumeParameter);
    }
    
    public void PlayFootStopSource(Vector3 pos,float volume)
    {
        AudioSource.PlayClipAtPoint(audioData.footSpeed[Random.Range(0, audioData.footSpeed.Length)], pos, volume * volumeParameter);
    }

    public void PlayStoveWaringSource(Vector3 pos)
    {
        PlayAudio(audioData.warning, pos);
    }
    /// <summary>
    /// 设置音效音乐的音量
    /// </summary>
    public void SetMenuMusic()
    {
        volumeParameter += 0.1f;
        if (volumeParameter > 1)
        {
            volumeParameter = 0;
        }
    }

    /// <summary>
    /// 获取当前音乐的音量数值
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        return volumeParameter;
    }
}
