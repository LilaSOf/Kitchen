using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms;

public class DeliveryManage : Singleton<DeliveryManage>
{
    [SerializeField] private RecipeSO recipeSO;

    [Header("�˵��б�")]
    [SerializeField]private List<MenuRecipeSO> recipeList = new List<MenuRecipeSO>();//���ɵĲ˵�����������

    [Header("�������ɲ˵�����")]
    private int maxMenuNum = 5;
    [Header("���ɲ˵���ʱ��")]

    private float maxSpawnTime = 8f;
    private float spawnTime ;

    //����UI��ʾ���¼�
    public event EventHandler<MenuSpawData> MenuSpawnEvnet;
    [Header("�ύ��������")]
    private int successRecipe;
    public class MenuSpawData : EventArgs
    {
        /// <summary>
        /// �˵�����
        /// </summary>
        public MenuRecipeSO menuRecipe;
        /// <summary>
        /// ����-1ʱΪ���
        /// </summary>
        public int removeIndex;
    }

    //������Ч���ɵ��¼�
    public event EventHandler DeliverySuccess;
    public event EventHandler DeliveryFailure;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if(spawnTime < 0)
        {
            spawnTime = maxSpawnTime;
            //���ɲ˵�
            if(recipeList.Count < maxMenuNum)
            {
                //��������
                int rang =UnityEngine.Random.Range(0, recipeSO.menuRecipeSO.Count);
               // Debug.Log(rang);
                recipeList.Add(recipeSO.menuRecipeSO[rang]);
                //����UI
                MenuSpawnEvnet?.Invoke(this,new MenuSpawData { menuRecipe = recipeSO.menuRecipeSO[rang], removeIndex  = -1});
            }
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="platesKitchenObject">���ӵ�����</param>
    public void DeliverRecip(PlatesKitchenObject platesKitchenObject)
    {
        for(int i = 0;i < recipeList.Count; i++)
        {
            MenuRecipeSO menuRecipeSO = recipeList[i];
            if(menuRecipeSO.kichenObjectDataList.Count == platesKitchenObject.GetKitchenObjectList().Count)
            {
                //ӵ����ͬ���嵥����
                bool plateContentsMatchesRecipe = true;
                foreach(var recipeKitchenObjectSO in menuRecipeSO.kichenObjectDataList)
                {
                    bool ingredientFound = false;
                    //�����嵥����
                    foreach(var plateKitchenObjectSO in platesKitchenObject.GetKitchenObjectList())
                    {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    //���û�ҵ����嵥
                    if(!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if(plateContentsMatchesRecipe)
                {
                    Debug.Log(i);
                    //�Ƴ�����
                    recipeList.RemoveAt(i);
                    //�Ƴ�UI
                    MenuSpawnEvnet?.Invoke(this, new MenuSpawData { menuRecipe = null, removeIndex = i });
                    //���ųɹ���Ч
                    DeliverySuccess?.Invoke(this, EventArgs.Empty);
                    //��Ӽ�¼��������
                    successRecipe++;
                    return;
                }
                Debug.LogError("����Ĳ˵�");
            }
        }
        DeliveryFailure?.Invoke(this, EventArgs.Empty);
        Debug.Log(DeliveryFailure.GetInvocationList().Length);
    }
    /// <summary>
    /// ��ö����ύ����
    /// </summary>
    /// <returns></returns>
    public int GetSuccessRecipe()
    {
        return successRecipe;
    }

    public void ReSetSuccessRecipe()
    {
        successRecipe = 0;
    }
}
