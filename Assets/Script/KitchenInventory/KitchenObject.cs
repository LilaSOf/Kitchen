using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private FoodData_SO foodData;

    private IKitchenObjectParent kitchenObjectParent;
    /// <summary>
    /// 获得当前物品的数据
    /// </summary>
    /// <returns></returns>
    /// 
    //[SerializeField]private KitchenObjectFollow follow;
    private void Start()
    {

    }
    public FoodData_SO GetFoodData_SO()
    {
        return foodData;
    }

    /// <summary>
    /// 设置当前物体所在柜台（物体转移时使用）
    /// </summary>
    /// <param name="kitchenObject">所在柜台</param>
    public void SetKitchenCounter(IKitchenObjectParent kitchenObject)
    {
        /*kitchenObjectParent = kitchenObject;
      //  transform.SetParent(kitchenObject.GetKitchenObjectFollowTransform());
        //transform.localPosition = new Vector3(0,0,0);
        kitchenObject.SetKitchenObject(this);
        GetComponent<KitchenObjectFollow>().SetTargetTransfrom(kitchenObject.GetKitchenObjectFollowTransform());*/
        SpawKichenObject(kitchenObject);
    }
   /// <summary>
   /// 在网络环境中发生物体生成或转移
   /// </summary>
   public void SpawKichenObject(IKitchenObjectParent kitchenObject )
    {
        SetNetWorkObjectServerRpc(kitchenObject.GetNetworkObject());
    }
    #region 食物生成时在网络对象上设置其位置
    /// <summary>
    /// 食物箱子生成食物服务器端调用
    /// </summary>
    /// <param name="networkObjectReference"></param>
    [ServerRpc(RequireOwnership = false)]
    private void SetNetWorkObjectServerRpc(NetworkObjectReference networkObjectReference)
    {
        SetNetWorkObjectClientRpc(networkObjectReference);
    }
    /// <summary>
    /// 食物箱子生成食物客户端逻辑
    /// </summary>
    /// <param name="networkObjectReference"></param>
    [ClientRpc]
    private void SetNetWorkObjectClientRpc(NetworkObjectReference networkObjectReference)
    {
        //通过网络对象映射获得对象
        networkObjectReference.TryGet(out NetworkObject networkObject);
        IKitchenObjectParent kitchenObject = networkObject.GetComponent<IKitchenObjectParent>();    

        kitchenObjectParent = kitchenObject;

        kitchenObject.SetKitchenObject(this);

        //设置食物跟随对象
        Transform followtrans = kitchenObject.GetKitchenObjectFollowTransform();
       // Debug.Log(followtrans.gameObject.name);
        if (followtrans != null)
        {
            GetComponent<KitchenObjectFollow>().SetTargetTransfrom(followtrans);
        }
        else
        {
            Debug.LogWarning("null");
        }
    }
    #endregion

  
    /// <summary>
    /// 通过指定数据在对应位置生成物体
    /// </summary>
    /// <param name="kitchenObjectSO">物体数据</param>
    /// <param name="kitchenObjectParent">生成位置</param>
    public void SpawnKitchenObject(FoodData_SO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent)
    {
        GameObject kitchenObj = Instantiate(kitchenObjectSO.prefab, kitchenObjectParent.GetKitchenObjectFollowTransform());
        kitchenObjectParent.SetKitchenObject(kitchenObj.GetComponent<KitchenObject>());
        kitchenObj.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 获得当前物体所在柜台数据
    /// </summary>
    /// <returns></returns>
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }


    /// <summary>
    /// 生成新的物体（物体改变时使用）
    /// </summary>
    /// <param name="newObject">生成的物体</param>
    /// <param name="kitchenObjectParent">生成物体的位置</param>
    /// <param name="instantiateObject">获得生成的物体</param>
    public void ChangeItem(KitchenObject newObject,IKitchenObjectParent kitchenObjectParent )
    {
        KitchenObject kitchenObject = Instantiate(newObject, kitchenObjectParent.GetKitchenObjectFollowTransform());
        kitchenObject.transform.localPosition = Vector3.zero;
        kitchenObjectParent.SetKitchenObject(kitchenObject);
        //Destroy(this.gameObject);
    }

    /// <summary>
    /// 测试返回数据中的名字
    /// </summary>
    /// <returns></returns>
    public string NameOutput()
    {
        return foodData.name;
    }

    /// <summary>
    /// 尝试获得玩家身上的盘子
    /// </summary>
    /// <param name="platesKitchenObject">out 参数输出盘子</param>
    /// <returns>玩家身上是否有盘子</returns>
    public bool TryGetPlate(out PlatesKitchenObject platesKitchenObject)
    {
        if(this is  PlatesKitchenObject)
        {
            platesKitchenObject = this as PlatesKitchenObject;
            return true;
        }
        else
        {
            platesKitchenObject = null;
            return false;
        }
    }

    /// <summary>
    /// 清除自身数据包括显示
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void DestoryMySelf(IKitchenObjectParent kitchenObject)
    {
        Destroy(kitchenObject.GetKitchenObject().gameObject);
        kitchenObject.ClearKitchenObject();
    }
    /// <summary>
    /// 销毁网络对象
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void DestoryNetObject(IKitchenObjectParent kitchenObject)
    {
        Destroy(kitchenObject.GetKitchenObject().gameObject);
    }
    /// <summary>
    /// 解绑网络物体
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void ClearNetObjectParent(IKitchenObjectParent kitchenObject)
    {
        kitchenObject.ClearKitchenObject();
    }
    public static void SpanNetWorkKitchenObject(IKitchenObjectParent kitchenObjectParent, FoodData_SO data)
    {
        KitchenGameMultiplayer.Instance.SpawnKichenObjectInContainer(kitchenObjectParent, data);
    }
    public static void TrashKitchenObject(IKitchenObjectParent kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestoryKitchenNetObject(kitchenObject);
    }
}
