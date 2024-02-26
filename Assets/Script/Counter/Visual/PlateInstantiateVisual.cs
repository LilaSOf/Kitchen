using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateInstantiateVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlatesCounter platesCounter;

    [Header("碟子预制体")]
    [SerializeField] private PlatesKitchenObject platesKitchenObj;

    [Header("储存生成碟子的列表")]
    private List<PlatesKitchenObject> platesKitchenObjectList = new List<PlatesKitchenObject>();

    private int plateNum;
    private void OnEnable()
    {
        platesCounter.OnPlateSpawn += OnPlateSpawnEvent;
        platesCounter.OnPlateRemove += OnPlateRemoveEvent;
       
    }

    private void OnPlateRemoveEvent(object sender, EventArgs e)
    {
        Destroy(platesKitchenObjectList[platesKitchenObjectList.Count - 1].gameObject);
        platesKitchenObjectList.Remove(platesKitchenObjectList[platesKitchenObjectList.Count - 1]);
    }

    private void OnPlateSpawnEvent(object sender, int platesNum)
    {
        //生成预制体
        plateNum = platesNum;
        PlatesKitchenObject pko = Instantiate(platesKitchenObj, platesCounter.GetKitchenObjectFollowTransform());
        pko.transform.localPosition = Vector3.up * 0.1f * platesNum;
        platesKitchenObjectList.Add(pko);
    }

    private void OnDisable()
    {
        platesCounter.OnPlateSpawn -= OnPlateSpawnEvent;
        platesCounter.OnPlateRemove -= OnPlateRemoveEvent;
    }
}
