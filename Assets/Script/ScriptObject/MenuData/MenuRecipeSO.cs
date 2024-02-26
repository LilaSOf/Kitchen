using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "menu",menuName = "MenuData/MenuRecipeSO")]
public class MenuRecipeSO : ScriptableObject
{
    // Start is called before the first frame update
    public List<FoodData_SO> kichenObjectDataList = new List<FoodData_SO>();

    //public Sprite foodSprite;
    public string name;
}
