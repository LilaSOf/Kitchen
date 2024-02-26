using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlicesObjectType
{
    Tomato,Cabbage,Chess
}

[System.Serializable]
public class Food_Data
{
    public string name;
    public GameObject prefab;
    public Sprite sprite;
}
[System.Serializable]
public class KitchenObjectSlicesData
{
    [Header("标记哪个物体的切割过程")]
    public SlicesObjectType slicesObjectType;

    public KitchenObject inputObject;
    public KitchenObject outputObject;
    [Header("需要切割的次数")]
    public int cuttingProgressMax;
}

[System.Serializable]
public class KitchenObjectCookedData
{
    public KitchenObject inputObject;
    public KitchenObject outputObject;
    [Header("煮熟或烧糊的时间")]
    public float cookedOrBurned;
    [Header("当前食材属于第几阶段")]
    public CookedChangeState cookType;
}

//加入食材的状态
public enum CookedChangeState
{
    Raw, Cooked
}

/// <summary>
/// 通过继承该接口获得拿取厨房物品的属性
/// </summary>
public interface IKitchenObjectParent
{
    /// <summary>
    /// 物品呈现的位置
    /// </summary>
    /// <returns></returns>
    public Transform GetKitchenObjectFollowTransform();
    /// <summary>
    /// 设置当前kitchenObject
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void SetKitchenObject(KitchenObject kitchenObject);
    /// <summary>
    /// 获得当前物品数据
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetKitchenObject();
    /// <summary>
    /// 清除柜台数据
    /// </summary>
    public void ClearKitchenObject();
    /// <summary>
    /// 当前柜台（属性）是否有物品
    /// </summary>
    /// <returns></returns>
    public bool HasKitchenObject();
}

