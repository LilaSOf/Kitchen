using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private FoodData_SO foodData;

    private IKitchenObjectParent kitchenObjectParent;
    /// <summary>
    /// ��õ�ǰ��Ʒ������
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
    /// ���õ�ǰ�������ڹ�̨������ת��ʱʹ�ã�
    /// </summary>
    /// <param name="kitchenObject">���ڹ�̨</param>
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
   /// �����绷���з����������ɻ�ת��
   /// </summary>
   public void SpawKichenObject(IKitchenObjectParent kitchenObject )
    {
        SetNetWorkObjectServerRpc(kitchenObject.GetNetworkObject());
    }
    #region ʳ������ʱ�����������������λ��
    /// <summary>
    /// ʳ����������ʳ��������˵���
    /// </summary>
    /// <param name="networkObjectReference"></param>
    [ServerRpc(RequireOwnership = false)]
    private void SetNetWorkObjectServerRpc(NetworkObjectReference networkObjectReference)
    {
        SetNetWorkObjectClientRpc(networkObjectReference);
    }
    /// <summary>
    /// ʳ����������ʳ��ͻ����߼�
    /// </summary>
    /// <param name="networkObjectReference"></param>
    [ClientRpc]
    private void SetNetWorkObjectClientRpc(NetworkObjectReference networkObjectReference)
    {
        //ͨ���������ӳ���ö���
        networkObjectReference.TryGet(out NetworkObject networkObject);
        IKitchenObjectParent kitchenObject = networkObject.GetComponent<IKitchenObjectParent>();    

        kitchenObjectParent = kitchenObject;

        kitchenObject.SetKitchenObject(this);

        //����ʳ��������
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
    /// ͨ��ָ�������ڶ�Ӧλ����������
    /// </summary>
    /// <param name="kitchenObjectSO">��������</param>
    /// <param name="kitchenObjectParent">����λ��</param>
    public void SpawnKitchenObject(FoodData_SO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent)
    {
        GameObject kitchenObj = Instantiate(kitchenObjectSO.prefab, kitchenObjectParent.GetKitchenObjectFollowTransform());
        kitchenObjectParent.SetKitchenObject(kitchenObj.GetComponent<KitchenObject>());
        kitchenObj.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// ��õ�ǰ�������ڹ�̨����
    /// </summary>
    /// <returns></returns>
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }


    /// <summary>
    /// �����µ����壨����ı�ʱʹ�ã�
    /// </summary>
    /// <param name="newObject">���ɵ�����</param>
    /// <param name="kitchenObjectParent">���������λ��</param>
    /// <param name="instantiateObject">������ɵ�����</param>
    public void ChangeItem(KitchenObject newObject,IKitchenObjectParent kitchenObjectParent )
    {
        KitchenObject kitchenObject = Instantiate(newObject, kitchenObjectParent.GetKitchenObjectFollowTransform());
        kitchenObject.transform.localPosition = Vector3.zero;
        kitchenObjectParent.SetKitchenObject(kitchenObject);
        //Destroy(this.gameObject);
    }

    /// <summary>
    /// ���Է��������е�����
    /// </summary>
    /// <returns></returns>
    public string NameOutput()
    {
        return foodData.name;
    }

    /// <summary>
    /// ���Ի��������ϵ�����
    /// </summary>
    /// <param name="platesKitchenObject">out �����������</param>
    /// <returns>��������Ƿ�������</returns>
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
    /// ����������ݰ�����ʾ
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void DestoryMySelf(IKitchenObjectParent kitchenObject)
    {
        Destroy(kitchenObject.GetKitchenObject().gameObject);
        kitchenObject.ClearKitchenObject();
    }
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void DestoryNetObject(IKitchenObjectParent kitchenObject)
    {
        Destroy(kitchenObject.GetKitchenObject().gameObject);
    }
    /// <summary>
    /// �����������
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
