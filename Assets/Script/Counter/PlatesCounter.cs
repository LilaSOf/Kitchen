using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlatesCounter : BaseCounter
{
    // Start is called before the first frame update
    [Header("���ɵ��ӵļ��")]
    private float initPlateTime;
    private float initPlateMaxTime = 4f;
    [Header("���ɵ����������")]
    private int platesNum;
    private int maxPlatesNum = 4;
    //[SerializeField] private Transform holderPoint;

    [Header("��������")]
    [SerializeField] private FoodData_SO kitchenObjSO;

    

    //�Ӿ��¼�
    public event EventHandler<int> OnPlateSpawn;
    public event EventHandler OnPlateRemove;
    // Update is called once per frame
    void Update()
    {
        if(initPlateTime < initPlateMaxTime)
        {
            initPlateTime += Time.deltaTime;
        }
        else if (platesNum < maxPlatesNum)
        {
            initPlateTime = 0f;
            OnPlateSpawn?.Invoke(this, platesNum);
            platesNum++;
        }
    }
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(platesNum > 0)
            {
                KitchenObject kitchenObject = new KitchenObject();
                kitchenObject.SpawnKitchenObject(kitchenObjSO, player);
                OnPlateRemove?.Invoke(this,EventArgs.Empty);
                platesNum--;
            }
          
        }
    }
}
