using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlatesKitchenObject : KitchenObject
{
    // Start is called before the first frame update
    [Header("��ǰ���Ӵ���Ķ���")]
    [SerializeField]private List<FoodData_SO> kitchenObjSO = new List<FoodData_SO>();

    [Header("�ܹ�����Ķ���")]
    [SerializeField]private List<FoodData_SO> vildKitchenObjectSO = new List<FoodData_SO>();

    //����ʳ���Ӿ�Ч�����¼�
    public event EventHandler<OnPlateVisual> PlateCompleteVisualEvent;
    public class OnPlateVisual: EventArgs
    {
        public FoodData_SO kitchenDataSO;
    }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="kitchenObjectSO"></param>
   /// <returns></returns>
   public bool TryAddIngredient(FoodData_SO kitchenObjectSO)
    {
        //�ж��ܷ����
        if(!vildKitchenObjectSO.Contains(kitchenObjectSO))
        {
            Debug.LogWarning("���ڷ����嵥��");
            return false;
        }
        //�����ظ�����
        if(kitchenObjSO.Contains(kitchenObjectSO))
        {
            Debug.LogWarning("��ǰ�������д�����");
            return false;
         }
        else
        {
            kitchenObjSO.Add(kitchenObjectSO);
            PlateCompleteVisualEvent?.Invoke(this, new OnPlateVisual { kitchenDataSO = kitchenObjectSO });
            return true;
        }
    }

    /// <summary>
    /// ����Ӿ�Ч��
    /// </summary>
    public void ClearVisual()
    {
        kitchenObjSO.Clear();
        PlateCompleteVisualEvent?.Invoke(this, new OnPlateVisual { kitchenDataSO = null });
    }

    /// <summary>
    /// ����嵥�б�
    /// </summary>
    public List<FoodData_SO> GetKitchenObjectList()
    {
        return kitchenObjSO;
    }
}
