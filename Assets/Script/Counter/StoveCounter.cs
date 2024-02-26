using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StoveCounter : BaseCounter,IHasProgress
{
    // Start is called before the first frame update
    [SerializeField] private KitchenObjectCooked_SO kitchenObjectCooked_SO;
    public event EventHandler<State> OnStateChangedEvent;
    public event EventHandler<IHasProgress.OnProgressChangedEvnetArgs> OnProgressChanged;

    public enum State
    {
        Idle,Friding,Frided,Burning
    }

    [SerializeField]private State state;

    [Header("烹饪和烧焦的计时器")]
    private float cookedTimer;
    private float burnedTimer;

    [Header("烹饪数据")]
    [SerializeField]private KitchenObjectCookedData cookedData;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            if(player.HasKitchenObject() && CanCooked(player.GetKitchenObject()))//将物品放置进行烹饪
            {
                player.GetKitchenObject().SetKitchenCounter(this);
                player.ClearKitchenObject();
                cookedData = OutPutKitchenObject();
                //烹饪状态更改
                SetCookedStateForFood();
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                //判断玩家身上有没有盘子
                if (player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject))
                {
                    //传输物体到盘子上
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetFoodData_SO()))
                    {
                        Destroy(kitchenObject.gameObject);
                    }
                }
            }
            else if (!player.HasKitchenObject())//将物品拿走
            {
                cookedTimer = 0;
                burnedTimer = 0;
                GetKitchenObject().SetKitchenCounter(player);
                ClearKitchenObject();
            }
            state = State.Idle;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvnetArgs { progressNormalized = 0 });
                break;
            case State.Friding:
                if(cookedTimer < cookedData.cookedOrBurned)
                {
                    cookedTimer += Time.deltaTime;
                }
                else
                {
                    //变更物体改变状态
                    Destroy(kitchenObject.gameObject);
                    kitchenObject.ChangeItem(cookedData.outputObject, this);
                    cookedData = OutPutKitchenObject();
                    state = State.Frided;
                    cookedTimer = 0;
                    //更改UI显示
                }
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvnetArgs {progressNormalized= (float)cookedTimer/cookedData.cookedOrBurned });
                break;
            case State.Frided:
                if(burnedTimer < cookedData.cookedOrBurned)
                {
                    burnedTimer += Time.deltaTime;
                }
                else
                {
                    state = State.Burning;
                    Destroy(kitchenObject.gameObject);
                    kitchenObject.ChangeItem(cookedData.outputObject, this);
                    burnedTimer = 0;
                }
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvnetArgs { progressNormalized = (float)burnedTimer / cookedData.cookedOrBurned });
                break;
            case State.Burning:
                break;

        }
        OnStateChangedEvent?.Invoke(this, state);
    }


    /// <summary>
    /// 判断该物体是否能够被烹饪
    /// </summary>
    /// <returns></returns>
    private bool CanCooked(KitchenObject kitchenObject)
    {
        foreach(var kit in kitchenObjectCooked_SO.kitchenObjectCookedDatas)
        {
            if(kitchenObject.NameOutput() == kit.inputObject.NameOutput())
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 获得输出数据
    /// </summary>
    /// <returns>物体烹饪操作的数据</returns>
    private KitchenObjectCookedData OutPutKitchenObject()
    {
        foreach (var kit in kitchenObjectCooked_SO.kitchenObjectCookedDatas)
        {
            if (kitchenObject.NameOutput() == kit.inputObject.NameOutput())
            {
                return kit;
            }
        }
        return null;
    }
    
    /// <summary>
    /// 根据放入的食物设置当前烹饪状态
    /// </summary>
    private void SetCookedStateForFood()
    {
        if(cookedData!=null)
        {
            state = cookedData.cookType switch { 
                CookedChangeState.Raw => State.Friding,
                CookedChangeState.Cooked => State.Frided,
                _ => State.Idle
            }; 
        }
    }
}
