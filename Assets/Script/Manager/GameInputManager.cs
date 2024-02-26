using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : Singleton<GameInputManager>
{
    // Start is called before the first frame update
    private GameControl gameControl;//��ü��̿������

    [Header("��ȡ������ƶ�����")]
    private Vector3 moveDir;

    //����¼����������൱��ע�ᣩ
    /// <summary>
    /// �����¼�
    /// </summary>
    public event EventHandler InteractAction;
    /// <summary>
    /// ���ⰴ���¼�
    /// </summary>
    public event EventHandler Interact_CuttingAction;
    /// <summary>
    /// ESC�����¼�����ͣ��
    /// </summary>
    public event EventHandler PauseGame_Action;
    protected override void Awake()
    {
        base.Awake();
        gameControl = new GameControl();//ʵ��������
        gameControl.Player.Move.Enable();//��Ҫʹ�õĿ���������Ҫ��Enable
        gameControl.Player.Interact.Enable();//���������¼�
        //ע�ύ���¼�
        gameControl.Player.Interact.performed += OnInteract;
        //�����в˽���
        gameControl.Player.Interact_Cutting.Enable();
        gameControl.Player.Interact_Cutting.performed += OnInteract_CuttingAction;
        //������ͣ����
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
    /// �����ƶ�����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerMoveDirction()
    {
        Vector2 controlValue;
        //��ȡ�¼���������ƶ�ֵ
        controlValue = gameControl.Player.Move.ReadValue<Vector2>();
        //���ò��޶���������
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
