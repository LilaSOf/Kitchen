using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private void OnEnable()
    {
        cuttingCounter.cuttingVisualEvent += OncuttingVisualEvent;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// …Ë÷√∂Øª≠◊¥Ã¨
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="currentProgress"></param>
    private void OncuttingVisualEvent(object sender, float currentProgress)
    {
       if(currentProgress != 0)
        {
            animator.SetTrigger("Cut");
        }
    }

    private void OnDisable()
    {
        cuttingCounter.cuttingVisualEvent -= OncuttingVisualEvent;
    }
}
