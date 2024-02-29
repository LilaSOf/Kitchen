using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoriaUI : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameInputManager.Instance.InteractAction += OnInteractAction;
    }

    private void OnInteractAction(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
}
