using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : Singleton<GameInputManager>
{
    // Start is called before the first frame update
    private const string PLAYER_KEY_SAVE = "PlayerKey";

    private GameControl gameControl;//获得键盘控制组件

    [Header("获取的玩家移动方向")]
    private Vector3 moveDir;

    //添加事件（在其他类当中注册）
    /// <summary>
    /// 交互事件
    /// </summary>
    public event EventHandler InteractAction;
    /// <summary>
    /// 特殊按键事件
    /// </summary>
    public event EventHandler Interact_CuttingAction;
    /// <summary>
    /// ESC触发事件（暂停）
    /// </summary>
    public event EventHandler PauseGame_Action;
    protected override void Awake()
    {
        base.Awake();

        gameControl = new GameControl();//实例化对象

        if(PlayerPrefs.HasKey(PLAYER_KEY_SAVE))
        {
            gameControl.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_KEY_SAVE));
        }
        gameControl.Player.Move.Enable();//需要使用的控制内容需要先Enable
        gameControl.Player.Interact.Enable();//开启交互事件
        //注册交互事件
        gameControl.Player.Interact.performed += OnInteract;
        //开启切菜交互
        gameControl.Player.Interact_Cutting.Enable();
        gameControl.Player.Interact_Cutting.performed += OnInteract_CuttingAction;
        //开启暂停交互
        gameControl.Player.Pause.Enable();
        gameControl.Player.Pause.performed += OnPauseGame_Action;
    }

    private void OnPauseGame_Action(InputAction.CallbackContext context)
    {
        PauseGame_Action?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteract_CuttingAction(InputAction.CallbackContext context)
    {
        //Debug.Log("Cutting");
        Interact_CuttingAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        InteractAction?.Invoke(this,EventArgs.Empty);
    }

    /// <summary>
    /// 设置移动方向
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerMoveDirction()
    {
        Vector2 controlValue;
        //获取新键盘输入的移动值
        controlValue = gameControl.Player.Move.ReadValue<Vector2>();
        //设置并限定方向向量
        moveDir.Set(controlValue.x, 0, controlValue.y);
        moveDir.Normalize();
        return moveDir;
    }
    // Update is called once per frame
    private void OnDestroy()
    {
        gameControl.Player.Interact.performed -= OnInteract;
        gameControl.Player.Interact_Cutting.performed -= OnInteract_CuttingAction;
        gameControl.Player.Pause.performed -= OnPauseGame_Action;

        gameControl.Dispose();
    }


    /// <summary>
    /// 根据当前按键类型获得对应文本
    /// </summary>
    /// <param name="key">按键属性</param>
    /// <returns></returns>
    public string GetKeyName(KeyAttribute key)
    {
        string keyString = key switch
        {
            KeyAttribute.WalkUp => gameControl.Player.Move.bindings[1].ToDisplayString(),
            KeyAttribute.WalkDown => gameControl.Player.Move.bindings[2].ToDisplayString(),
            KeyAttribute.WalkLeft => gameControl.Player.Move.bindings[3].ToDisplayString(),
            KeyAttribute.WalkRight => gameControl.Player.Move.bindings[4].ToDisplayString(),
            KeyAttribute.Interact => gameControl.Player.Interact.bindings[0].ToDisplayString(),
            KeyAttribute.InteractAlt => gameControl.Player.Interact_Cutting.bindings[0].ToDisplayString(),
            KeyAttribute.Pause => gameControl.Player.Pause.bindings[0].ToDisplayString(),
            _ => ""
        };
        return keyString;
    }

    /// <summary>
    /// 设置新的按键
    /// </summary>
    public void SetNewKey(KeyAttribute key,Action complete)
    {
        gameControl.Player.Disable();
        InputAction interact;
        int bindingIndex;
        switch (key)
        {
            case KeyAttribute.WalkUp:
                interact = gameControl.Player.Move;
                bindingIndex = 1;
                break;
            case KeyAttribute.WalkDown:
                interact = gameControl.Player.Move;
                bindingIndex = 2;
                break;
            case KeyAttribute.WalkLeft:
                interact = gameControl.Player.Move;
                bindingIndex = 3;
                break;
            case KeyAttribute.WalkRight:
                interact = gameControl.Player.Move;
                bindingIndex = 4;
                break;
            case KeyAttribute.Interact:
                interact = gameControl.Player.Interact;
                bindingIndex = 0;
                break;
            case KeyAttribute.Pause:
                interact = gameControl.Player.Pause;
                bindingIndex = 0;
                break;
            case KeyAttribute.InteractAlt:
                interact = gameControl.Player.Interact;
                bindingIndex = 0;
                break;
            default: interact = gameControl.Player.Interact;
                bindingIndex = 0;
                break;
        }
        interact.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            complete?.Invoke();
           PlayerPrefs.SetString(PLAYER_KEY_SAVE,gameControl.SaveBindingOverridesAsJson());
            gameControl.Player.Enable();
        }).Start();

    }
}
