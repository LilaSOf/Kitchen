using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject particleObject;
    [SerializeField] private GameObject stoveOnVisual;

    [SerializeField] private StoveCounter stoveCounter;
    private void Start()
    {
        stoveCounter.OnStateChangedEvent += OnStateChangedVisual;
    }

    private void OnStateChangedVisual(object sender, StoveCounter.State e)
    {
        bool isShow = e == StoveCounter.State.Frided || e == StoveCounter.State.Friding;
        particleObject.SetActive(isShow);
        stoveOnVisual.SetActive(isShow);
    }
}
