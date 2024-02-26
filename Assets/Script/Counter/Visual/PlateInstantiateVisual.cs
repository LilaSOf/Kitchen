using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateInstantiateVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlatesCounter platesCounter;

    [Header("����Ԥ����")]
    [SerializeField] private PlatesKitchenObject platesKitchenObj;

    [Header("�������ɵ��ӵ��б�")]
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
        //����Ԥ����
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
