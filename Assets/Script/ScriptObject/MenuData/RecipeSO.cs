using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "recipe_SO",menuName ="MenuData/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    // Start is called before the first frame update
    public List<MenuRecipeSO> menuRecipeSO = new List<MenuRecipeSO>();
}
