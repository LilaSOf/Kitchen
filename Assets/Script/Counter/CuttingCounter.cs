using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CuttingCounter : BaseCounter,IHasProgress
{
    [Header("可切割物品的数据")]
    public KitchenObjectSlices_SO kitchenObjectSlices;
    /// <summary>
    /// 切割时其他部分的视觉效果
    /// </summary>
    public event EventHandler<IHasProgress.OnProgressChangedEvnetArgs> OnProgressChanged;

    public event EventHandler<float> cuttingVisualEvent;
    [Header("切割的状态")]
    private int cuttingProgerss = 0;
    private bool isCutted;

    //音效事件
    public static event EventHandler OnAnyCut;
    /// <summary>
    /// 常规交互（将物品放置到台子上）
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenCounter(this);
                player.ClearKitchenObject();
                isCutted = false;
            }
        }
        else if(HasKitchenObject())
        {
            if (player.HasKitchenObject() && isCutted)
            {
                //判断玩家身上有没有盘子
                if (player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject))
                {
                    //传输物体到盘子上
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetFoodData_SO()))
                    {
                        Destroy(kitchenObject.gameObject);
                    }
                }
            }
            else if (!player.HasKitchenObject() && cuttingProgerss == 0)
            {
                kitchenObject.SetKitchenCounter(player);
                ClearKitchenObject();
            }
        }
    }

    /// <summary>
    /// 特殊交互
    /// </summary>
    /// <param name="player"></param>
    public override void InteractAlt(Player player)
    {
        int index = CanKitchenObjectCutting(kitchenObject);
        if (index != -1)//该物体能够被切割
        {
            //获取切割物品对应的数据类
            KitchenObjectSlicesData kitchenObjectSlicesData = new KitchenObjectSlicesData();
            kitchenObjectSlicesData = kitchenObjectSlices.kitchenObjectSlicesDatas[index];

            float cuttingProgressNum = (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax;

            if (cuttingProgerss <kitchenObjectSlicesData.cuttingProgressMax-1)//是否达到切割次数
            {
                cuttingProgerss++;
                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEvnetArgs {progressNormalized = (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax });//调用视觉事件
                //动画事件
                cuttingVisualEvent?.Invoke(this, (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax);

                //音效事件
                OnAnyCut?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                //销毁当前物体
                Destroy(kitchenObject.gameObject);
                //生成一个被切割过的物体
                KitchenObject cuttingObject = Instantiate(kitchenObjectSlicesData.outputObject);
                kitchenObject = cuttingObject;
                kitchenObject.SetKitchenCounter(this);

                //结束时重置状态
                cuttingProgerss = 0;
                isCutted = true;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvnetArgs { progressNormalized = (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax });
                //动画事件
                cuttingVisualEvent?.Invoke(this, (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax);

                //音效事件
                OnAnyCut?.Invoke(this, EventArgs.Empty);
            }
           
        }
    }


   /// <summary>
   /// 获取当前平台上物体的切割数据(-1为不可切割)
   /// </summary>
   /// <param name="kitchenObject"></param>
   /// <returns></returns>
   private int CanKitchenObjectCutting(KitchenObject kitchenObject)
    {
        int index = 0;
        if(kitchenObject == null)
        {
            return -1;
        }
        foreach(var kitObj in kitchenObjectSlices.kitchenObjectSlicesDatas)
        {
           if(kitObj.inputObject.NameOutput() == kitchenObject.NameOutput())
            {
                return index; 
            }
           index++;
        }
        return -1;
    }
    

}
