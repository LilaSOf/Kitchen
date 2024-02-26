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

    [Header("��⿺��ս��ļ�ʱ��")]
    private float cookedTimer;
    private float burnedTimer;

    [Header("�������")]
    [SerializeField]private KitchenObjectCookedData cookedData;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            if(player.HasKitchenObject() && CanCooked(player.GetKitchenObject()))//����Ʒ���ý������
            {
                player.GetKitchenObject().SetKitchenCounter(this);
                player.ClearKitchenObject();
                cookedData = OutPutKitchenObject();
                //���״̬����
                SetCookedStateForFood();
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                //�ж����������û������
                if (player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject))
                {
                    //�������嵽������
                    if (platesKitchenObject.TryAddIngredient(GetKitchenObject().GetFoodData_SO()))
                    {
                        Destroy(kitchenObject.gameObject);
                    }
                }
            }
            else if (!player.HasKitchenObject())//����Ʒ����
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
                    //�������ı�״̬
                    Destroy(kitchenObject.gameObject);
                    kitchenObject.ChangeItem(cookedData.outputObject, this);
                    cookedData = OutPutKitchenObject();
                    state = State.Frided;
                    cookedTimer = 0;
                    //����UI��ʾ
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
    /// �жϸ������Ƿ��ܹ������
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
    /// ����������
    /// </summary>
    /// <returns>������⿲���������</returns>
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
    /// ���ݷ����ʳ�����õ�ǰ���״̬
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
