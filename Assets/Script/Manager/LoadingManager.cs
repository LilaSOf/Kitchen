using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class LoadingManager 
{
    // Start is called before the first frame update
    public static int targetScene;
    public static void LoadScene()
    {
        SceneManager.LoadScene(targetScene);
    }
     public static void LoadScene(int targetSceneIndex)
    {
        targetScene = targetSceneIndex;
        SceneManager.LoadScene(1);
    }
}
