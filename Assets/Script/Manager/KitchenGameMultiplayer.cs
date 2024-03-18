using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private KitchenObjectMultiData multiData;

    [SerializeField] private FoodData_SO food;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }


    #region 销毁网络对象
    public void DestoryKitchenNetObject(IKitchenObjectParent kitchenObject)
    {
        DestoryNetObjectServerRpc(kitchenObject.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestoryNetObjectServerRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject netObject);
        IKitchenObjectParent kitchenObject = netObject.GetComponent<IKitchenObjectParent>();

        kitchenObject.GetKitchenObject().DestoryNetObject(kitchenObject);
    }
    [ClientRpc]
    public void ClearNetObjectParentClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject netObject);
        IKitchenObjectParent kitchenObject = netObject.GetComponent<IKitchenObjectParent>();

        kitchenObject.GetKitchenObject().ClearNetObjectParent(kitchenObject);
    }
    #endregion


    #region 生成网络对象
    /// <summary>
    /// 在食物箱子中生成网络厨房对象
    /// </summary>
    /// <param name="kitchenObject">对象的父物体</param>
    /// <param name="kichenData">物体数据</param>
    public void SpawnKichenObjectInContainer(IKitchenObjectParent kitchenObject,FoodData_SO kichenData)
    {
        // Debug.Log(kitchenObject.GetNetworkObject().ToString() + GetIndexForData(kichenData));
         SpawnKichenObjectInContainerServerRpc(GetIndexForData(kichenData), kitchenObject.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKichenObjectInContainerServerRpc(int kitchenObjectIndex,NetworkObjectReference parentObjectNetReference)
    {
        ///通过传入的NetworkObjectReference获取接口
        parentObjectNetReference.TryGet(out NetworkObject networkObject);
        FoodData_SO kichenData = GetDataForIndex(kitchenObjectIndex);
        IKitchenObjectParent kitchenObject = networkObject.GetComponent<IKitchenObjectParent>();
        Transform kichenObjecttrans = Instantiate(kichenData.prefab, kitchenObject.GetKitchenObjectFollowTransform()).transform;

        //网络物体生成
        kichenObjecttrans.GetComponent<NetworkObject>().Spawn(true);
        //设定父对象
        kichenObjecttrans.GetComponent<KitchenObject>().SpawKichenObject(kitchenObject);
    }
    #endregion
    /// <summary>
    /// 通过数据获得序号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public FoodData_SO GetDataForIndex(int index)
    {
        return multiData.kitchenObjectList[index];
    }
    /// <summary>
    /// 通过序号获得数据
    /// </summary>
    /// <param name="kitchenSO"></param>
    /// <returns></returns>
    public int GetIndexForData(FoodData_SO kitchenSO)
    {
        return multiData.kitchenObjectList.IndexOf(kitchenSO);
    }
}
