using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuListTemplete : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text name;
    [Header("ʳ������ʳ��Icon")]
    [SerializeField] private GameObject iconTemplete;
    [SerializeField] private Transform iconParent;
    private void Start()
    {
        iconTemplete.SetActive(false);
    }

    public void UpdateUIShow(MenuRecipeSO menuRecipe)
    {
        name.text = menuRecipe.name;
        //����ͼ��
        foreach(var kitchenObjectData in menuRecipe.kichenObjectDataList)
        {
            GameObject icon = Instantiate(iconTemplete, iconParent);
            icon.SetActive(true);
            icon.transform.GetChild(0).GetComponent<Image>().sprite = kitchenObjectData.sprite;
        }
    }
}
