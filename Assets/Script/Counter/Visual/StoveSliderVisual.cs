using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSliderVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        stoveCounter.OnProgressChanged += OnProgressChangedEvent;
    }

    private void OnProgressChangedEvent(object sender, IHasProgress.OnProgressChangedEvnetArgs e)
    {
        if(e.progressNormalized > 0.5f && stoveCounter.IsFiredWarning())
        {
            animator.SetBool("IsWarning", true);
        }
        else
        {
            animator.SetBool("IsWarning", false);
        }
    }
}
