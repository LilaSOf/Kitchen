using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; set; }
    private void Awake()
    {
        Instance = this;    
    }
    // Start is called before the first frame update
    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            if(player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject))
            {
                DeliveryManage.Instance.DeliverRecip(platesKitchenObject);
                player.GetKitchenObject().DestoryMySelf(player);
            }
        }
    }
}
