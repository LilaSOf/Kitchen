using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Transform targetTransfrom;

    private void Start()
    {
      
    }

    public void SetTargetTransfrom(Transform targetTrans)
    {
        this.targetTransfrom = targetTrans;
    }

    private void LateUpdate()
    {
        if (targetTransfrom != null)
        {
            transform.position = targetTransfrom.position;
            transform.rotation = targetTransfrom.rotation;
        }
    }
}
