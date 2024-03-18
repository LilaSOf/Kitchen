using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class TrashCounter : BaseCounter
{
    // Start is called before the first frame update
    //À¬»øÒôÐ§ÊÂ¼þ
    public static event EventHandler TranshedEvent;
    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject))
            {
                platesKitchenObject.ClearVisual();
            }
            else
            {

                KitchenObject.TrashKitchenObject(player);
               // player.ClearKitchenObject();
            }
            TranshedEvent?.Invoke(this, EventArgs.Empty);
        }
    }

  
}
