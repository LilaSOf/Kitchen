using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    void Start()
    {
        playButton.onClick.AddListener(() => {
            LoadingManager.LoadScene(2);
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    // Update is called once per frame
    
}
