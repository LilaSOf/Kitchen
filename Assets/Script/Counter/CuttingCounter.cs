using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CuttingCounter : BaseCounter,IHasProgress
{
    [Header("���и���Ʒ������")]
    public KitchenObjectSlices_SO kitchenObjectSlices;
    /// <summary>
    /// �и�ʱ�������ֵ��Ӿ�Ч��
    /// </summary>
    public event EventHandler<IHasProgress.OnProgressChangedEvnetArgs> OnProgressChanged;

    public event EventHandler<float> cuttingVisualEvent;
    [Header("�и��״̬")]
    private int cuttingProgerss = 0;
    private bool isCutted;

    //��Ч�¼�
    public static event EventHandler OnAnyCut;
    /// <summary>
    /// ���潻��������Ʒ���õ�̨���ϣ�
    /// </summary>
    /// <param name="player"></param>
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenCounter(this);
                player.ClearKitchenObject();
                isCutted = false;
            }
        }
        else if(HasKitchenObject())
        {
            if (player.HasKitchenObject() && isCutted)
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
            else if (!player.HasKitchenObject() && cuttingProgerss == 0)
            {
                kitchenObject.SetKitchenCounter(player);
                ClearKitchenObject();
            }
        }
    }

    /// <summary>
    /// ���⽻��
    /// </summary>
    /// <param name="player"></param>
    public override void InteractAlt(Player player)
    {
        int index = CanKitchenObjectCutting(kitchenObject);
        if (index != -1)//�������ܹ����и�
        {
            //��ȡ�и���Ʒ��Ӧ��������
            KitchenObjectSlicesData kitchenObjectSlicesData = new KitchenObjectSlicesData();
            kitchenObjectSlicesData = kitchenObjectSlices.kitchenObjectSlicesDatas[index];

            float cuttingProgressNum = (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax;

            if (cuttingProgerss <kitchenObjectSlicesData.cuttingProgressMax-1)//�Ƿ�ﵽ�и����
            {
                cuttingProgerss++;
                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEvnetArgs {progressNormalized = (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax });//�����Ӿ��¼�
                //�����¼�
                cuttingVisualEvent?.Invoke(this, (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax);

                //��Ч�¼�
                OnAnyCut?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                //���ٵ�ǰ����
                Destroy(kitchenObject.gameObject);
                //����һ�����и��������
                KitchenObject cuttingObject = Instantiate(kitchenObjectSlicesData.outputObject);
                kitchenObject = cuttingObject;
                kitchenObject.SetKitchenCounter(this);

                //����ʱ����״̬
                cuttingProgerss = 0;
                isCutted = true;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvnetArgs { progressNormalized = (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax });
                //�����¼�
                cuttingVisualEvent?.Invoke(this, (float)cuttingProgerss / kitchenObjectSlicesData.cuttingProgressMax);

                //��Ч�¼�
                OnAnyCut?.Invoke(this, EventArgs.Empty);
            }
           
        }
    }


   /// <summary>
   /// ��ȡ��ǰƽ̨��������и�����(-1Ϊ�����и�)
   /// </summary>
   /// <param name="kitchenObject"></param>
   /// <returns></returns>
   private int CanKitchenObjectCutting(KitchenObject kitchenObject)
    {
        int index = 0;
        if(kitchenObject == null)
        {
            return -1;
        }
        foreach(var kitObj in kitchenObjectSlices.kitchenObjectSlicesDatas)
        {
           if(kitObj.inputObject.NameOutput() == kitchenObject.NameOutput())
            {
                return index; 
            }
           index++;
        }
        return -1;
    }
    

}
