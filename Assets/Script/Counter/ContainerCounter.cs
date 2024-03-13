using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    // Start is called before the first frame update
    [SerializeField] private FoodData_SO foodData;
    [SerializeField] private Animator animator; 
    public override void Interact(Player player)
    {
       if(!player.HasKitchenObject())
        {
            //������Ʒ���������
            KitchenGameMultiplayer.Instance.SpawnKichenObjectInContainer(player, foodData);
           //KitchenObject kit =  Instantiate(kitchenObject, player.GetKitchenObjectFollowTransform());
            animator.SetTrigger("OpenClose");
           //kit.SetKitchenCounter(player);
        }
        else
        {
            //Debug.Log("��������");
        }
    }
}
