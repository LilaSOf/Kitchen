using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private PlatesKitchenObject kitchenObject;
    [Serializable]
    private struct GetKitchenObject
    {
        public FoodData_SO kitchenDataSO;
        public GameObject gameObject;
    }
    [Header("储存数据与对应的预制体")]
    [SerializeField]private List<GetKitchenObject> kitchenObjectList = new List<GetKitchenObject>();
    void Start()
    {
        foreach(var obj in kitchenObjectList)
        {
            obj.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        kitchenObject.PlateCompleteVisualEvent += OnPlateCompleteVisualEvent;
    }

    private void OnPlateCompleteVisualEvent(object sender, PlatesKitchenObject.OnPlateVisual e)
    {
        if(e.kitchenDataSO != null)//判断是否清空
        {
            foreach (var obj in kitchenObjectList)
            {
                if (obj.kitchenDataSO == e.kitchenDataSO)
                {
                    obj.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var obj in kitchenObjectList)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        kitchenObject.PlateCompleteVisualEvent -= OnPlateCompleteVisualEvent;
    }
}
