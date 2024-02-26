using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("����ʱ��ʼ��Ϸ")]
    [SerializeField] private TextMeshProUGUI countDownUI;

    [Header("��Ϸ������")]
    [SerializeField] private Image slider;

    [Header("������Ϸ")]
    [SerializeField] private GameObject gameOverUI;
    [Header("��ͣ��Ϸ")]
    [SerializeField] private GameObject gamePausedUI;

    [Header("��Ϸ�˵�")]
    [SerializeField]private GameObject optionsUI;

    private void Start()
    {
        GameManager.Instance.GameCountToStartEvent += OnGameCountToStartEvent;
        GameManager.Instance.GamePlayingEvent += OnGamePlayingEvent;
        GameManager.Instance.GameOverEvent += OnGameOverEvent;
        GameManager.Instance.GamePauseEvent += OnGamePauseEvent;

        //���¿�ʼ��ť(��Ϸ�����˵�)
        gameOverUI.transform.Find("ReStart").GetComponent<Button>().onClick.AddListener(() => { Hide(gameOverUI); GameManager.Instance.SetGameState(0); });
        gameOverUI.transform.Find("ReturnTitle").GetComponent<Button>().onClick.AddListener(() => { Hide(gameOverUI); LoadingManager.LoadScene(0); });

        //���ذ�ť(��ͣ�˵�)
        gamePausedUI.transform.Find("Return").GetComponent<Button>().onClick.AddListener(() => { Hide(gamePausedUI);GameManager.Instance.SetGameState(0); });
        gamePausedUI.transform.Find("ReturnTitle").GetComponent<Button>().onClick.AddListener(() => { Hide(gamePausedUI); LoadingManager.LoadScene(0); });
        //���ý���
        gamePausedUI.transform.Find("Options").GetComponent<Button>().onClick.AddListener(() => { Show(optionsUI);Hide(gamePausedUI);});

        //Optionsҳ�棨������ذ�ť��
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
