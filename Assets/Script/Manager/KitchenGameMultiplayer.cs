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
        ///ͨ�������NetworkObjectReference��ȡ�ӿ�
        parentObjectNetReference.TryGet(out NetworkObject networkObject);
        IKitchenObjectParent kitchenObject = networkObject.GetComponent<IKitchenObjectParent>();

        Transform kichenObjecttrans = Instantiate(kichenData.prefab, kitchenObject.GetKitchenObjectFollowTransform()).transform;
        kichenObjecttrans.localPosition = Vector3.zero;
        //������������
        kichenObjecttrans.GetComponent<NetworkObject>().Spawn(true);

        kichenObjecttrans.GetComponent<KitchenObject>().SpawKichenObject(kitchenObject);
    }


    /// <summary>
    /// ͨ�����ݻ�����
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public FoodData_SO GetDataForIndex(int index)
    {
        return multiData.kitchenObjectList[index];
    }
    /// <summary>
    /// ͨ����Ż������
    /// </summary>
    /// <param name="kitchenSO"></param>
    /// <returns></returns>
    public int GetIndexForData(FoodData_SO kitchenSO)
    {
        return multiData.kitchenObjectList.IndexOf(kitchenSO);
    }
}
