using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Player : NetworkBehaviour,IKitchenObjectParent
{
    //public static Player Instance { get; private set; }

    //�ı�ѡ�еĹ�̨Ŀ��
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [Header("����ƶ��ٶ�")]
    public float moveSpeed;//����ƶ��ٶ�

    [Header("���ת���ٶ�")]
    public float turnSpeed;

    [Header("���߼��㼶")]
    [SerializeField] private float playerRadious;
    [SerializeField]private LayerMask counterLayer;
    private Vector3 moveDir;//����ƶ�����
    [Header("�Ƿ������ƶ�")]
    public bool isWalking;

    [Header("������Ҫ�����ݴ洢")]
    private BaseCounter selectCounter;
    [SerializeField]private Transform kitchenHoldPoint;
    [SerializeField]private KitchenObject kitchenObject;

    //������Ч���ɵ��¼�
    public event EventHandler PlayerPickUpItemEvent;
    private  void Awake()
    {
      //  Instance = this;
    }
    private void Start()
    {
        //ע�ᰴ�������¼�
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
        //�жϵ�ǰ����/ѡ��״̬
        HandleInteration();
        //��������
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue);

        //��ȡ�ƶ�����
        if(GameManager.Instance.IsGamePauseOver())
        {
            moveDir = Vector3.zero;
        }
        else
        {
            moveDir = GameInputManager.Instance.GetPlayerMoveDirction();
        } 
        //���Բ�ʹ������ϵͳ������ײ�ж�
        HandleMove();
    }

    private void FixedUpdate()
    {
        //�ƶ�
        //HandleMove();
    }

    /// <summary>
    /// �и�����⽻�����¼�����
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
    /// һ�㽻�����¼�����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void OnInteractAction(object sender,EventArgs eventArgs)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        selectCounter?.Interact(this);
    }


    /// <summary>
    /// ��������ƶ�
    /// </summary>
    private void HandleMove()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerHeight = 2f;
        bool canMove =  !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDir, moveDistance);
        if (!canMove)
        {
            //�ж���ײ
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDirX, moveDistance);
            if(!canMove)
            {
                //�����ƶ���������ж��ܷ����������ƶ�
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove =moveDir.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadious, moveDirZ, moveDistance);
                if (canMove)//Z�����ƶ�
                {
                    moveDir = moveDirZ;
                }
            }
            else
            {
                //X�����ƶ�
                moveDir = moveDirX;
            }
        }
      if (canMove) { 
            //�ƶ��߼�
            transform.position += (moveDir * moveSpeed * Time.deltaTime);
        }
        //����ĸı�
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void HandleInteration()
    {
        Debug.DrawRay(transform.position, transform.forward * 2f,Color.blue);//��������

        float interationDistance = 2f;
        if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hitInfo,interationDistance,counterLayer))
        {
            if(hitInfo.collider.TryGetComponent<BaseCounter>(out BaseCounter counter))//�ж϶��������Ƿ���ClearCounter����������Ƿ��ǹ�̨��
            {
                SetClearCounterState(counter);
            }
            else//�����ǣ����ù�̨״̬
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
    /// ���ݹ�̨���ƽű����ù�̨״̬
    /// �ж��Ƿ�ѡ�й�̨
    /// </summary>
    /// <param name="clearCounter">���صĹ�̨���ƽű�</param>
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
    /// ���ص�ǰ�ƶ�״̬
    /// </summary>
    /// <returns>�ƶ�״̬</returns>
    public bool IsWalking()
    {
        return isWalking;
    }


    #region �ӿ�ʵ��
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
