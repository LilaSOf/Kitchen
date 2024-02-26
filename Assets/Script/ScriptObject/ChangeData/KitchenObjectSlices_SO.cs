using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlicesSO",menuName ="KitchenObject/SlicesSO")]
public class KitchenObjectSlices_SO : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]public List<KitchenObjectSlicesData> kitchenObjectSlicesDatas = new List<KitchenObjectSlicesData> ();   
}
