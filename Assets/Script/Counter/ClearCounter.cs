using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    // Start is called before the first frame update
    [SerializeField] private FoodData_SO foodData;
  /// <summary>
  /// 与当前物体交互
  /// </summary>
    public override void Interact(Player player)
    {
       if(kitchenObject != null)//桌子有东西
        {
            if(player.HasKitchenObject())
            {
                //判断玩家身上有没有盘子
                if (player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject))
                {
                    //传输物体到盘子上
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetFoodData_SO()))
                    {
                        GetKitchenObject().DestoryMySelf(this);
                    }
                }
                else
                {
                    if(GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject1))
                    {
                        if(platesKitchenObject1.TryAddIngredient(player.GetKitchenObject().GetFoodData_SO()))
                        {
                          player.GetKitchenObject().DestoryMySelf(player);
                        }
                    }
                }
            }
            else
            {
                //传输物品到人物身上
                kitchenObject.SetKitchenCounter(player);
                ClearKitchenObject();
            }
          
        }
       else if(!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                //人物身上传输到台子上(人物身上有，柜台上没有)
                player.GetKitchenObject().SetKitchenCounter(this);
                player.ClearKitchenObject();
            }
            else
            {
                //人物什么都不做（人物身上没有，柜台上没有）
            }
        }
    }

}
