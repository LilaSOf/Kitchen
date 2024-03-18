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


    #region �����������
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


    #region �����������
    /// <summary>
    /// ��ʳ�����������������������
    /// </summary>
    /// <param name="kitchenObject">����ĸ�����</param>
    /// <param name="kichenData">��������</param>
    public void SpawnKichenObjectInContainer(IKitchenObjectParent kitchenObject,FoodData_SO kichenData)
    {
        // Debug.Log(kitchenObject.GetNetworkObject().ToString() + GetIndexForData(kichenData));
         SpawnKichenObjectInContainerServerRpc(GetIndexForData(kichenData), kitchenObject.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKichenObjectInContainerServerRpc(int kitchenObjectIndex,NetworkObjectReference parentObjectNetReference)
    {
        ///ͨ�������NetworkObjectReference��ȡ�ӿ�
        parentObjectNetReference.TryGet(out NetworkObject networkObject);
        FoodData_SO kichenData = GetDataForIndex(kitchenObjectIndex);
        IKitchenObjectParent kitchenObject = networkObject.GetComponent<IKitchenObjectParent>();
        Transform kichenObjecttrans = Instantiate(kichenData.prefab, kitchenObject.GetKitchenObjectFollowTransform()).transform;

        //������������
        kichenObjecttrans.GetComponent<NetworkObject>().Spawn(true);
        //�趨������
        kichenObjecttrans.GetComponent<KitchenObject>().SpawKichenObject(kitchenObject);
    }
    #endregion
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
