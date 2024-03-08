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
    [SerializeField]private GameState state = GameState.GameWaitToStart;//游戏状态

    [Header("倒数开始")]
    private float countToStart = 3;
    private float countToStartMax = 3;

    [Header("游戏进行时间")]
    private float gameTimer;
    private float gameTimerMax =300;

    //倒数开始游戏事件
    public event EventHandler<float> GameCountToStartEvent;
    //游戏运行事件
    public event EventHandler<float> GamePlayingEvent;
    //游戏结束事件
    public event EventHandler GameOverEvent;
    //游戏暂停事件
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
                    //倒数开始游戏
                    countToStart -= Time.deltaTime;
                    GameCountToStartEvent?.Invoke(this, countToStart);
                }
                else
                {
                    //重置信息并开始游戏
                    countToStart = countToStartMax;
                    state = GameState.GamePlaying;
                }
                break;
            case GameState.GamePlaying:
                if (gameTimer < gameTimerMax)
                {
                    //倒数开始结束游戏
                    gameTimer += Time.deltaTime;
                    GamePlayingEvent?.Invoke(this, gameTimer/ gameTimerMax);
                }
                else
                {
                    //重置信息结束游戏
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
    /// 当前游戏是处于运行状态(用于判断交互)
    /// </summary>
    /// <returns></returns>
    public bool IsGamePlaying()
    {
        return state == GameState.GamePlaying;
    }
    /// <summary>
    /// 当前游戏是否属于终止状态
    /// </summary>
    /// <returns></returns>
    public bool IsGamePauseOver()
    {
        return state == GameState.GamePause || state == GameState.GameOver;
    }

    /// <summary>
    /// 暂停按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPauseGame_Action(object sender, EventArgs e)
    {
        state = GameState.GamePause;
        GamePauseEvent?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 设置当前游戏状态
    /// </summary>
    /// <param name="stateIndex">
    /// 0,游戏倒数开始
    /// 1，游戏进行
    /// 2，游戏暂停
    /// 3，游戏结束
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
