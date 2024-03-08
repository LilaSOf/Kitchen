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

    [Header("菜单列表")]
    [SerializeField]private List<MenuRecipeSO> recipeList = new List<MenuRecipeSO>();//生成的菜单储存在这里

    [Header("控制生成菜单数量")]
    private int maxMenuNum = 5;
    [Header("生成菜单的时间")]

    private float maxSpawnTime = 8f;
    private float spawnTime ;

    //控制UI显示的事件
    public event EventHandler<MenuSpawData> MenuSpawnEvnet;
    [Header("提交订单数量")]
    private int successRecipe;
    public class MenuSpawData : EventArgs
    {
        /// <summary>
        /// 菜单数据
        /// </summary>
        public MenuRecipeSO menuRecipe;
        /// <summary>
        /// 等于-1时为添加
        /// </summary>
        public int removeIndex;
    }

    //控制提交的结果事件
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
            //生成菜单
            if(recipeList.Count < maxMenuNum)
            {
                //生成数据
                int rang =UnityEngine.Random.Range(0, recipeSO.menuRecipeSO.Count);

                SpawnNewWaitingRecipeClientRpc(rang);
               // Debug.Log(rang);
                //recipeList.Add(recipeSO.menuRecipeSO[rang]);
                //生成UI
                //MenuSpawnEvnet?.Invoke(this,new MenuSpawData { menuRecipe = recipeSO.menuRecipeSO[rang], removeIndex  = -1});
            }
        }
    }

    /// <summary>
    /// 交付订单
    /// </summary>
    /// <param name="platesKitchenObject">盘子的数据</param>
    public void DeliverRecip(PlatesKitchenObject platesKitchenObject)
    {
        for(int i = 0;i < recipeList.Count; i++)
        {
            MenuRecipeSO menuRecipeSO = recipeList[i];
            if(menuRecipeSO.kichenObjectDataList.Count == platesKitchenObject.GetKitchenObjectList().Count)
            {
                //拥有相同的清单长度
                bool plateContentsMatchesRecipe = true;
                foreach(var recipeKitchenObjectSO in menuRecipeSO.kichenObjectDataList)
                {
                    bool ingredientFound = false;
                    //遍历清单内容
                    foreach(var plateKitchenObjectSO in platesKitchenObject.GetKitchenObjectList())
                    {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    //如果没找到该清单
                    if(!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if(plateContentsMatchesRecipe)
                {
                    Debug.Log(i);
                    /*//移除数据
                    recipeList.RemoveAt(i);
                    //移除UI
                    MenuSpawnEvnet?.Invoke(this, new MenuSpawData { menuRecipe = null, removeIndex = i });
                    //播放成功音效
                    DeliverySuccess?.Invoke(this, EventArgs.Empty);
                    //添加记录交付个数
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
        //移除数据
        recipeList.RemoveAt(i);
        //移除UI
        MenuSpawnEvnet?.Invoke(this, new MenuSpawData { menuRecipe = null, removeIndex = i });
        //播放成功音效
        DeliverySuccess?.Invoke(this, EventArgs.Empty);
        //添加记录交付个数
        successRecipe++;
    }


    [ClientRpc]
    /// <summary>
    /// 同步获得菜单数据信息
    /// </summary>
    private void SpawnNewWaitingRecipeClientRpc(int rang)
    {
        MenuRecipeSO recipe = recipeSO.menuRecipeSO[rang];

        recipeList.Add(recipe);

        MenuSpawnEvnet?.Invoke(this, new MenuSpawData { menuRecipe = recipeSO.menuRecipeSO[rang], removeIndex = -1 });
    }

    /// <summary>
    /// 获得订单提交数量
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
