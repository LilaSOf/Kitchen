using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlatesKitchenObject : KitchenObject
{
    // Start is called before the first frame update
    [Header("当前盘子储存的东西")]
    [SerializeField]private List<FoodData_SO> kitchenObjSO = new List<FoodData_SO>();

    [Header("能够储存的东西")]
    [SerializeField]private List<FoodData_SO> vildKitchenObjectSO = new List<FoodData_SO>();

    //控制食物视觉效果的事件
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
        //判断能否放置
        if(!vildKitchenObjectSO.Contains(kitchenObjectSO))
        {
            Debug.LogWarning("不在放置清单内");
            return false;
        }
        //避免重复放置
        if(kitchenObjSO.Contains(kitchenObjectSO))
        {
            Debug.LogWarning("当前盘子已有此物体");
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
    /// 清空视觉效果
    /// </summary>
    public void ClearVisual()
    {
        kitchenObjSO.Clear();
        PlateCompleteVisualEvent?.Invoke(this, new OnPlateVisual { kitchenDataSO = null });
    }

    /// <summary>
    /// 获得清单列表
    /// </summary>
    public List<FoodData_SO> GetKitchenObjectList()
    {
        return kitchenObjSO;
    }
}
