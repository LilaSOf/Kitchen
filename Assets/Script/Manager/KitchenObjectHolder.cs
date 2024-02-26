using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectHolder : MonoBehaviour
{
    // Start is called before the first frame update
    protected Transform holdertTansform;//当前Holder
    protected Transform targetHolder;//目标Holder
    /// <summary>
    /// 获取物品位置
    /// </summary>
    /// <param name="trans">目标transform</param>
    /// <returns></returns>
    protected Transform GetHolderPoint(Transform trans)
    {
       return trans.Find("HolderPoint");
    }
    
    /// <summary>
    /// 转移物体
    /// </summary>
    /// <param name="currentKitchenObj">当前物体</param>
    /// <param name="targetKitchenObj">目标物体</param>
    protected void ChangeKitchenObject(FoodData_SO currentKitchenObj,FoodData_SO targetKitchenObj)
    {
        if(currentKitchenObj == null && targetKitchenObj != null)//目标转移到当前
        {
            currentKitchenObj = targetKitchenObj;//数据传递
            ClearKitchenObject(targetKitchenObj,targetHolder);//清除传递项的数据
            Instantiate(currentKitchenObj.prefab, targetHolder);//生成预制体
        }
        else if (currentKitchenObj != null && targetKitchenObj == null)
        {
            targetKitchenObj = currentKitchenObj;//数据传递
            Instantiate(currentKitchenObj.prefab, targetHolder);//生成预制体
            ClearKitchenObject(currentKitchenObj,holdertTansform);//清除传递项的数据
         
        }
        else//其他情况不转移
        {
            Debug.Log("不执行转移");
        }
    }


    /// <summary>
    /// 传递后清理当前放置台的物体
    /// </summary>
    /// <param name="current">当前存放物体数据</param>
    protected void ClearKitchenObject(FoodData_SO current,Transform transform)
    {
        foreach(var it in transform.GetComponentsInChildren<Transform>()) 
        {
            Destroy(it.gameObject);
        }
        current = null;
    }
}
