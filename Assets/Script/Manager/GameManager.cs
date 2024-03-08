using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    private enum GameState
    {
        GamePause,GameStartCount,GamePlaying,GameOver,GameWaitToStart
    }
    [SerializeField]private GameState state = GameState.GameWaitToStart;//��Ϸ״̬

    [Header("������ʼ")]
    private float countToStart = 3;
    private float countToStartMax = 3;

    [Header("��Ϸ����ʱ��")]
    private float gameTimer;
    private float gameTimerMax =300;

    //������ʼ��Ϸ�¼�
    public event EventHandler<float> GameCountToStartEvent;
    //��Ϸ�����¼�
    public event EventHandler<float> GamePlayingEvent;
    //��Ϸ�����¼�
    public event EventHandler GameOverEvent;
    //��Ϸ��ͣ�¼�
    public event EventHandler GamePauseEvent;
    
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        GameInputManager.Instance.PauseGame_Action += OnPauseGame_Action;
        GameInputManager.Instance.InteractAction += OnInteractAction;
    }

  
    private void Update()
    {
        switch (state)
        {
            case GameState.GameWaitToStart:

                break;
            case GameState.GamePause:
                
                break;
            case GameState.GameStartCount:
                DeliveryManage.Instance.ReSetSuccessRecipe();
                if (countToStart > 0)
                {
                    //������ʼ��Ϸ
                    countToStart -= Time.deltaTime;
                    GameCountToStartEvent?.Invoke(this, countToStart);
                }
                else
                {
                    //������Ϣ����ʼ��Ϸ
                    countToStart = countToStartMax;
                    state = GameState.GamePlaying;
                }
                break;
            case GameState.GamePlaying:
                if (gameTimer < gameTimerMax)
                {
                    //������ʼ������Ϸ
                    gameTimer += Time.deltaTime;
                    GamePlayingEvent?.Invoke(this, gameTimer/ gameTimerMax);
                }
                else
                {
                    //������Ϣ������Ϸ
                    countToStart = 0;
                    gameTimer = 0;
                    state = GameState.GameOver;
                }
                break;
            case GameState.GameOver:
                GameOverEvent?.Invoke(this, EventArgs.Empty);
                break;
        }
    }
    private void OnInteractAction(object sender, EventArgs e)
    {
        if(state == GameState.GameWaitToStart)
        {
            state = GameState.GameStartCount;
        }
    }

    /// <summary>
    /// ��ǰ��Ϸ�Ǵ�������״̬(�����жϽ���)
    /// </summary>
    /// <returns></returns>
    public bool IsGamePlaying()
    {
        return state == GameState.GamePlaying;
    }
    /// <summary>
    /// ��ǰ��Ϸ�Ƿ�������ֹ״̬
    /// </summary>
    /// <returns></returns>
    public bool IsGamePauseOver()
    {
        return state == GameState.GamePause || state == GameState.GameOver;
    }

    /// <summary>
    /// ��ͣ��ť�¼�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPauseGame_Action(object sender, EventArgs e)
    {
        state = GameState.GamePause;
        GamePauseEvent?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// ���õ�ǰ��Ϸ״̬
    /// </summary>
    /// <param name="stateIndex">
    /// 0,��Ϸ������ʼ
    /// 1����Ϸ����
    /// 2����Ϸ��ͣ
    /// 3����Ϸ����
    /// </param>
    public void SetGameState(int stateIndex)
    {
        state = stateIndex switch
        {
            0 => GameState.GameStartCount,
            1 => GameState.GamePlaying,
            2 => GameState.GamePause,
            3 => GameState.GameOver,
            _ => GameState.GamePause
        };
    }
}
