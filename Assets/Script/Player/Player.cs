using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Player : NetworkBehaviour,IKitchenObjectParent
{
    //public static Player Instance { get; private set; }

    //改变选中的柜台目标
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [Header("玩家移动速度")]
    public float moveSpeed;//玩家移动速度

    [Header("玩家转身速度")]
    public float turnSpeed;

    [Header("射线检测层级")]
    [SerializeField] private float playerRadious;
    [SerializeField]private LayerMask counterLayer;
    private Vector3 moveDir;//玩家移动方向
    [Header("是否正在移动")]
    public bool isWalking;

    [Header("交互需要的数据存储")]
    private BaseCounter selectCounter;
    [SerializeField]private Transform kitchenHoldPoint;
    [SerializeField]private KitchenObject kitchenObject;

    //控制音效生成的事件
    public event EventHandler PlayerPickUpItemEvent;
    private  void Awake()
    {
      //  Instance = this;
    }
    private void Start()
    {
        //注册按键交互事件
        GameInputManager.Instance.InteractAction += OnInteractAction;
        GameInputManager.Instance.Interact_CuttingAction += OnInteract_CuttingAction;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {
            return;
        }
        isWalking = moveDir != Vector3.zero;
        //判断当前交互/选择状态
        HandleInteration();
        //绘制射线
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue);

        //获取移动方向
        if(GameManager.Instance.IsGamePauseOver())
        {
            moveDir = Vector3.zero;
        }
        else
        {
            moveDir = GameInputManager.Instance.GetPlayerMoveDirction();
        } 
        //尝试不使用物理系统进行碰撞判定
        HandleMove();
    }

    private void FixedUpdate()
    {
        //移动
        //HandleMove();
    }

    /// <summary>
    /// 切割等特殊交互的事件订阅
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnInteract_CuttingAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectCounter?.InteractAlt(this);
    }


    /// <summary>
    /// 一般交互的事件订阅
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void OnInteractAction(object sender,EventArgs eventArgs)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectCounter?.Interact(this);
    }


    /// <summary>
    /// 控制玩家移动
    /// </summary>
    private void HandleMove()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerHeight = 2f;
        bool canMove =  !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDir, moveDistance);
        if (!canMove)
        {
            //判断碰撞
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDirX, moveDistance);
            if(!canMove)
            {
                //不能移动的情况下判断能否其他方向移动
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove =moveDir.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDirZ, moveDistance);
                if (canMove)//Z方向移动
                {
                    moveDir = moveDirZ;
                }
            }
            else
            {
                //X方向移动
                moveDir = moveDirX;
            }
        }
      if (canMove) { 
            //移动逻辑
            transform.position += (moveDir * moveSpeed * Time.deltaTime);
        }
        //朝向的改变
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 交互方法
    /// </summary>
    private void HandleInteration()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f,Color.blue);//绘制射线

        float interationDistance = 2f;
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hitInfo,interationDistance,counterLayer))
        {
            if(hitInfo.collider.TryGetComponent<BaseCounter>(out BaseCounter counter))//判断对象身上是否有ClearCounter组件（即：是否是柜台）
            {
                SetClearCounterState(counter);
            }
            else//若不是，设置柜台状态
            {
                SetClearCounterState(null);
            }
        }
        else
        {
            SetClearCounterState(null);
        }
    }

    /// <summary>
    /// 根据柜台控制脚本设置柜台状态
    /// 判断是否选中柜台
    /// </summary>
    /// <param name="clearCounter">返回的柜台控制脚本</param>
    private void SetClearCounterState(BaseCounter targetCounter)
    {
        if(selectCounter != targetCounter)
        {
            //selectCounter?.DisSelect();
            //clearCounter?.OnSelect();
            selectCounter = targetCounter;
        }

        OnSelectedCounterChanged?.Invoke(this,new OnSelectedCounterChangedEventArgs()
        {
            selectedCounter = targetCounter
        });    
    }

    /// <summary>
    /// 返回当前移动状态
    /// </summary>
    /// <returns>移动状态</returns>
    public bool IsWalking()
    {
        return isWalking;
    }


    #region 接口实现
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenHoldPoint;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            PlayerPickUpItemEvent?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion
}
