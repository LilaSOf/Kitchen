using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlateUIVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlatesKitchenObject platesKitchenObject;

    private GameObject iconTemplete;
    private List<GameObject> iconObject = new List<GameObject>();
    private void OnEnable()
    {
        platesKitchenObject.PlateCompleteVisualEvent += OnPlateCompleteVisualEvent;
    }
    private void Start()
    {
        iconTemplete = transform.Find("IconTemplete").gameObject;
        iconTemplete.SetActive(false);
    }
    private void OnPlateCompleteVisualEvent(object sender, PlatesKitchenObject.OnPlateVisual e)
    {
        if(e.kitchenDataSO != null)
        {
            GameObject icon = Instantiate(iconTemplete, transform);
            icon.SetActive(true);
            icon.transform.GetChild(1).GetComponent<Image>().sprite = e.kitchenDataSO.sprite;
            iconObject.Add(icon);
        }
        else
        {
            foreach(var obj in iconObject)
            {
                Destroy(obj);
            }
            iconObject.Clear();
        }
    }

    private void OnDisable()
    {
        platesKitchenObject.PlateCompleteVisualEvent -= OnPlateCompleteVisualEvent;
    }
}
