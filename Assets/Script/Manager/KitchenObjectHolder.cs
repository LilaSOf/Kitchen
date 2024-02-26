using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectHolder : MonoBehaviour
{
    // Start is called before the first frame update
    protected Transform holdertTansform;//��ǰHolder
    protected Transform targetHolder;//Ŀ��Holder
    /// <summary>
    /// ��ȡ��Ʒλ��
    /// </summary>
    /// <param name="trans">Ŀ��transform</param>
    /// <returns></returns>
    protected Transform GetHolderPoint(Transform trans)
    {
       return trans.Find("HolderPoint");
    }
    
    /// <summary>
    /// ת������
    /// </summary>
    /// <param name="currentKitchenObj">��ǰ����</param>
    /// <param name="targetKitchenObj">Ŀ������</param>
    protected void ChangeKitchenObject(FoodData_SO currentKitchenObj,FoodData_SO targetKitchenObj)
    {
        if(currentKitchenObj == null && targetKitchenObj != null)//Ŀ��ת�Ƶ���ǰ
        {
            currentKitchenObj = targetKitchenObj;//���ݴ���
            ClearKitchenObject(targetKitchenObj,targetHolder);//��������������
            Instantiate(currentKitchenObj.prefab, targetHolder);//����Ԥ����
        }
        else if (currentKitchenObj != null && targetKitchenObj == null)
        {
            targetKitchenObj = currentKitchenObj;//���ݴ���
            Instantiate(currentKitchenObj.prefab, targetHolder);//����Ԥ����
            ClearKitchenObject(currentKitchenObj,holdertTansform);//��������������
         
        }
        else//���������ת��
        {
            Debug.Log("��ִ��ת��");
        }
    }


    /// <summary>
    /// ���ݺ�����ǰ����̨������
    /// </summary>
    /// <param name="current">��ǰ�����������</param>
    protected void ClearKitchenObject(FoodData_SO current,Transform transform)
    {
        foreach(var it in transform.GetComponentsInChildren<Transform>()) 
        {
            Destroy(it.gameObject);
        }
        current = null;
    }
}
