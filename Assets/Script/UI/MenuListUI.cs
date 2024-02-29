using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuListUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MenuListTemplete menuListTemplete;
    [Header("���浱ǰ���ɵ��б�")]
    private List<MenuListTemplete> menuListTempleteList = new List<MenuListTemplete>();
    private void OnEnable()
    {
        
    }
    private void Start()
    {
        menuListTemplete.gameObject.SetActive(false);
        DeliveryManage.Instance.MenuSpawnEvnet += OnMenuSpawnEvnet;
    }
    private void OnMenuSpawnEvnet(object sender, DeliveryManage.MenuSpawData e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            //��Ӳ˵�
            if (e.removeIndex < 0)
            {
                // Debug.Log("Add");
                MenuListTemplete temp = Instantiate(menuListTemplete, transform);
                temp.gameObject.SetActive(true);
                temp.UpdateUIShow(e.menuRecipe);
                menuListTempleteList.Add(temp);
            }
            else//�Ƴ��˵� 
            {
                //Debug.Log("Remove");
                Destroy(menuListTempleteList[e.removeIndex].gameObject);
                menuListTempleteList.RemoveAt(e.removeIndex);
            }
        }
    }

    private void OnDisable()
    {
        //DeliveryManage.Instance.MenuSpawnEvnet -= OnMenuSpawnEvnet;
    }
}
