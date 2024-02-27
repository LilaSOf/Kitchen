using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OptionsUI : Singleton<OptionsUI>
{
    [Header("游戏音效")]
    [SerializeField] private Button audioBtn;
    [SerializeField] private Button menuMusicBtn;

    [Header("文字显示")]
    [SerializeField] private Text audioText;
    [SerializeField] private Text menuMusicText;
    protected override void Awake()
    {
        base.Awake();
        audioBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.SetMenuMusic();
            UpdateTextShow();
        });

        menuMusicBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.SetMenuMusic();
            UpdateTextShow();
        });
        
    }
    private void Start()
    {
        UpdateTextShow();
    }
    // Start is called before the first frame update

    private void UpdateTextShow()
    {
        audioText.text = $"游戏音效音量  ：  {Mathf.Round(AudioManager.Instance.GetMusicVolume() * 10f)}";
        menuMusicText.text = $"背景音量  ：  {Mathf.Round(MusicManager.Instance.GetMusicVolume() * 10f)}";
    }
}
