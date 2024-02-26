using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : Singleton<GameInputManager>
{
    // Start is called before the first frame update
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
}
