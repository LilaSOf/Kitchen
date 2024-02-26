using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookedSO",menuName ="KitchenObject/CookedSO")]
public class KitchenObjectCooked_SO : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]public List<KitchenObjectCookedData> kitchenObjectCookedDatas = new List<KitchenObjectCookedData> ();   
}
