using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class BaseCounter :NetworkBehaviour, IKitchenObjectParent
{
    [Header("��̨��������")]
    [SerializeField] protected KitchenObject kitchenObject;
    [Header("��̨������Ʒ��")]
    [SerializeField] protected Transform kitchenTopPoint;

    //�����ڹ�̨����Ч�¼�
    public static event EventHandler KitchenObjectDropEvent;

    /// <summary>
    /// ͨ����д��������ʵ�ֲ�ͬ
    /// </summary>
    /// <param name="player">��Ҵ���</param>
    public virtual void Interact(Player player)
    {

    }

    /// <summary>
    /// ��д������⽻���ķ���
    /// </summary>
    /// <param name="player"></param>
    public virtual void InteractAlt(Player player)
    {

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
        return kitchenTopPoint;
    }

    public bool HasKitchenObject()
    {
       return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null) {
            KitchenObjectDropEvent?.Invoke(this,EventArgs.Empty);
        }
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
    #endregion
}
