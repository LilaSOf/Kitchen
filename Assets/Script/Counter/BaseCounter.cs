using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class BaseCounter :NetworkBehaviour, IKitchenObjectParent
{
    [Header("柜台物体数据")]
    [SerializeField] protected KitchenObject kitchenObject;
    [Header("柜台放置物品点")]
    [SerializeField] protected Transform kitchenTopPoint;

    //放置在柜台的音效事件
    public static event EventHandler KitchenObjectDropEvent;

    /// <summary>
    /// 通过重写交互方法实现不同
    /// </summary>
    /// <param name="player">玩家代码</param>
    public virtual void Interact(Player player)
    {

    }

    /// <summary>
    /// 重写获得特殊交互的方法
    /// </summary>
    /// <param name="player"></param>
    public virtual void InteractAlt(Player player)
    {

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
