using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SocialPlatforms;
using Unity.Netcode;
public class DeliveryManage :NetworkBehaviour //Singleton<DeliveryManage>
{
    public static DeliveryManage Instance { get; private set; }
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

    //�����ύ�Ľ���¼�
    public event EventHandler DeliverySuccess;
    public event EventHandler DeliveryFailure;
    protected  void Awake()
    {
        //base.Awake();
        Instance = this;
    }
    private void Update()
    {
        if (!IsServer)
            return;
        spawnTime -= Time.deltaTime;
        if(spawnTime < 0)
        {
            spawnTime = maxSpawnTime;
            //���ɲ˵�
            if(recipeList.Count < maxMenuNum)
            {
                //��������
                int rang =UnityEngine.Random.Range(0, recipeSO.menuRecipeSO.Count);

                SpawnNewWaitingRecipeClientRpc(rang);
               // Debug.Log(rang);
                //recipeList.Add(recipeSO.menuRecipeSO[rang]);
                //����UI
                //MenuSpawnEvnet?.Invoke(this,new MenuSpawData { menuRecipe = recipeSO.menuRecipeSO[rang], removeIndex  = -1});
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
                    /*//�Ƴ�����
                    recipeList.RemoveAt(i);
                    //�Ƴ�UI
                    MenuSpawnEvnet?.Invoke(this, new MenuSpawData { menuRecipe = null, removeIndex = i });
                    //���ųɹ���Ч
                    DeliverySuccess?.Invoke(this, EventArgs.Empty);
                    //��Ӽ�¼��������
                    successRecipe++;*/
                    DeliveryCorrectRecipeServerRpc(i);
                    return;
                }
               
            }
        }
        DeliveryFailureServerRpc();
        //DeliveryFailure?.Invoke(this, EventArgs.Empty);
        Debug.Log(DeliveryFailure.GetInvocationList().Length);
    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliveryFailureServerRpc()
    {
        DeliveryFailureClientRpc();
    }
    [ClientRpc]
    private void DeliveryFailureClientRpc()
    {
        DeliveryFailure?.Invoke(this, EventArgs.Empty);
    }


    [ServerRpc(RequireOwnership = false)]
    private void DeliveryCorrectRecipeServerRpc(int i)
    {
        DeliveryCorrectRecipeClientRpc(i);
    }


    [ClientRpc]
    private void DeliveryCorrectRecipeClientRpc(int i)
    {
        //�Ƴ�����
        recipeList.RemoveAt(i);
        //�Ƴ�UI
        MenuSpawnEvnet?.Invoke(this, new MenuSpawData { menuRecipe = null, removeIndex = i });
        //���ųɹ���Ч
        DeliverySuccess?.Invoke(this, EventArgs.Empty);
        //��Ӽ�¼��������
        successRecipe++;
    }


    [ClientRpc]
    /// <summary>
    /// ͬ����ò˵�������Ϣ
    /// </summary>
    private void SpawnNewWaitingRecipeClientRpc(int rang)
    {
        MenuRecipeSO recipe = recipeSO.menuRecipeSO[rang];

        recipeList.Add(recipe);

        MenuSpawnEvnet?.Invoke(this, new MenuSpawData { menuRecipe = recipeSO.menuRecipeSO[rang], removeIndex = -1 });
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
