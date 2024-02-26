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
    [Header("����ĸ�������и����")]
    public SlicesObjectType slicesObjectType;

    public KitchenObject inputObject;
    public KitchenObject outputObject;
    [Header("��Ҫ�и�Ĵ���")]
    public int cuttingProgressMax;
}

[System.Serializable]
public class KitchenObjectCookedData
{
    public KitchenObject inputObject;
    public KitchenObject outputObject;
    [Header("������պ���ʱ��")]
    public float cookedOrBurned;
    [Header("��ǰʳ�����ڵڼ��׶�")]
    public CookedChangeState cookType;
}

//����ʳ�ĵ�״̬
public enum CookedChangeState
{
    Raw, Cooked
}

/// <summary>
/// ͨ���̳иýӿڻ����ȡ������Ʒ������
/// </summary>
public interface IKitchenObjectParent
{
    /// <summary>
    /// ��Ʒ���ֵ�λ��
    /// </summary>
    /// <returns></returns>
    public Transform GetKitchenObjectFollowTransform();
    /// <summary>
    /// ���õ�ǰkitchenObject
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void SetKitchenObject(KitchenObject kitchenObject);
    /// <summary>
    /// ��õ�ǰ��Ʒ����
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetKitchenObject();
    /// <summary>
    /// �����̨����
    /// </summary>
    public void ClearKitchenObject();
    /// <summary>
    /// ��ǰ��̨�����ԣ��Ƿ�����Ʒ
    /// </summary>
    /// <returns></returns>
    public bool HasKitchenObject();
}

