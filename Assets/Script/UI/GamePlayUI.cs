using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("倒计时开始游戏")]
    [SerializeField] private TextMeshProUGUI countDownUI;

    [Header("游戏进行中")]
    [SerializeField] private Image slider;

    [Header("结束游戏")]
    [SerializeField] private GameObject gameOverUI;
    [Header("暂停游戏")]
    [SerializeField] private GameObject gamePausedUI;

    [Header("游戏菜单")]
    [SerializeField]private GameObject optionsUI;

    private void Start()
    {
        GameManager.Instance.GameCountToStartEvent += OnGameCountToStartEvent;
        GameManager.Instance.GamePlayingEvent += OnGamePlayingEvent;
        GameManager.Instance.GameOverEvent += OnGameOverEvent;
        GameManager.Instance.GamePauseEvent += OnGamePauseEvent;

        //重新开始按钮(游戏结束菜单)
        gameOverUI.transform.Find("ReStart").GetComponent<Button>().onClick.AddListener(() => { Hide(gameOverUI); GameManager.Instance.SetGameState(0); });
        gameOverUI.transform.Find("ReturnTitle").GetComponent<Button>().onClick.AddListener(() => { Hide(gameOverUI); LoadingManager.LoadScene(0); });

        //返回按钮(暂停菜单)
        gamePausedUI.transform.Find("Return").GetComponent<Button>().onClick.AddListener(() => { Hide(gamePausedUI);GameManager.Instance.SetGameState(0); });
        gamePausedUI.transform.Find("ReturnTitle").GetComponent<Button>().onClick.AddListener(() => { Hide(gamePausedUI); LoadingManager.LoadScene(0); });
        //设置界面
        gamePausedUI.transform.Find("Options").GetComponent<Button>().onClick.AddListener(() => { Show(optionsUI);Hide(gamePausedUI);});

        //Options页面（点击返回按钮）
        optionsUI.transform.Find("Return").GetComponent<Button>().onClick.AddListener(() => { Hide(optionsUI); GameManager.Instance.SetGameState(0); });
    }

    private void OnGameCountToStartEvent(object sender, float e)
    {
        if(e <= 0)
        {
            Hide(countDownUI.gameObject);
        }
        else
        {
            Show(countDownUI.gameObject);
            countDownUI.text = e.ToString("0");
        }
    }
    private void OnGamePlayingEvent(object sender, float e)
    {
        slider.fillAmount = e;
    }
    private void OnGameOverEvent(object sender,EventArgs e)
    {
        Show(gameOverUI);
        gameOverUI.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = DeliveryManage.Instance.GetSuccessRecipe().ToString();
    }


    private void OnGamePauseEvent(object sender, EventArgs e)
    {
        Show(gamePausedUI);
    }

    private void Hide(GameObject gameobject)
    {
        gameobject.SetActive(false);
    }
    
    private void Show(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }
}
