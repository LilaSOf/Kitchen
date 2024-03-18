using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class PlatesCounter : BaseCounter
{
    // Start is called before the first frame update
    [Header("生成碟子的间隔")]
    private float initPlateTime;
    private float initPlateMaxTime = 4f;
    [Header("生成碟子最大数量")]
    private int platesNum;
    private int maxPlatesNum = 4;
    //[SerializeField] private Transform holderPoint;

    [Header("碟子数据")]
    [SerializeField] private FoodData_SO kitchenObjSO;

    

    //视觉事件
    public event EventHandler<int> OnPlateSpawn;
    public event EventHandler OnPlateRemove;
    // Update is called once per frame
    void Update()
    {
        if(!IsServer) return;
        PlateSpawnServerRpc();
    }
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(platesNum > 0)
            {
                KitchenObject.SpanNetWorkKitchenObject(player, kitchenObjSO);
                PlateInteractServerRpc();
                platesNum--;
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void PlateSpawnServerRpc()
    {
        PlateSpawnClientRpc();
    }
    [ClientRpc]
    private void PlateSpawnClientRpc()
    {
        if (initPlateTime < initPlateMaxTime)
        {
            initPlateTime += Time.deltaTime;
        }
        else if (platesNum < maxPlatesNum && GameManager.Instance.IsGamePlaying())
        {
            initPlateTime = 0f;
            OnPlateSpawn?.Invoke(this, platesNum);
            platesNum++;
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void PlateInteractServerRpc()
    {
        PlateInteractClientRpc();
    }

    [ClientRpc]
    private void PlateInteractClientRpc()
    {
        OnPlateRemove?.Invoke(this, EventArgs.Empty);
    }
}
