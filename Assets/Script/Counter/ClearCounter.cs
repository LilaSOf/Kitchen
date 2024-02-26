using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    // Start is called before the first frame update
    [SerializeField] private FoodData_SO foodData;
  /// <summary>
  /// �뵱ǰ���彻��
  /// </summary>
    public override void Interact(Player player)
    {
       if(kitchenObject != null)//�����ж���
        {
            if(player.HasKitchenObject())
            {
                //�ж����������û������
                if (player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject))
                {
                    //�������嵽������
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
                //������Ʒ����������
                kitchenObject.SetKitchenCounter(player);
                ClearKitchenObject();
            }
          
        }
       else if(!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                //�������ϴ��䵽̨����(���������У���̨��û��)
                player.GetKitchenObject().SetKitchenCounter(this);
                player.ClearKitchenObject();
            }
            else
            {
                //����ʲô����������������û�У���̨��û�У�
            }
        }
    }

}
