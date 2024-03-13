using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private KitchenObjectMultiData multiData;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

   public void SpawnKichenObjectInContainer(IKitchenObjectParent kitchenObject,FoodData_SO kichenData)
    {
        SpawnKichenObjectInContainerServerRPC(GetIndexForData(kichenData), kitchenObject.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKichenObjectInContainerServerRPC(int kitchenObjectIndex,NetworkObjectReference parentObjectNetReference)
    {

        FoodData_SO kichenData = GetDataForIndex(kitchenObjectIndex);
        ///通过传入的NetworkObjectReference获取接口
        parentObjectNetReference.TryGet(out NetworkObject networkObject);
        IKitchenObjectParent kitchenObject = networkObject.GetComponent<IKitchenObjectParent>();

        Transform kichenObjecttrans = Instantiate(kichenData.prefab, kitchenObject.GetKitchenObjectFollowTransform()).transform;
        kichenObjecttrans.localPosition = Vector3.zero;
        //网络物体生成
        kichenObjecttrans.GetComponent<NetworkObject>().Spawn(true);

        kichenObjecttrans.GetComponent<KitchenObject>().SpawKichenObject(kitchenObject);
    }


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
