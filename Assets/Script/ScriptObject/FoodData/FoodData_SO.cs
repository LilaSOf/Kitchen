using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Food/FoodData_SO",menuName = "KitchenObject/FoodSO")]
public class FoodData_SO : ScriptableObject
{
    //public List<Food_Data> foodData = new List<Food_Data>();
    public string name;
    public GameObject prefab;
    public Sprite sprite;

}
