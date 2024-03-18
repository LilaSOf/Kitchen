using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            KitchenObject.SpanNetWorkKitchenObject(player, foodData);
            //KitchenGameMultiplayer.Instance.SpawnKichenObjectInContainer(player, foodData);
            //KitchenObject kit =  Instantiate(kitchenObject, player.GetKitchenObjectFollowTransform());
            ContainerAnimationServerRpc();
           //kit.SetKitchenCounter(player);
        }
        else
        {
            //Debug.Log("��������");
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void ContainerAnimationServerRpc()
    {
        ContainerAnimationClientRpc();
    }
    [ClientRpc]
    private void ContainerAnimationClientRpc()
    {
        animator.SetTrigger("OpenClose");
    }
}

