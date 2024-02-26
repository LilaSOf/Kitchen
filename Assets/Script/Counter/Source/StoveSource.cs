using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSource : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource source;
    private StoveCounter.State state = StoveCounter.State.Idle;
    void Start()
    {
        source = GetComponent<AudioSource>();
        stoveCounter.OnStateChangedEvent += OnSourceChangeEvent;
    }

    private void OnSourceChangeEvent(object sender, StoveCounter.State e)
    {
        bool isPlay = e == StoveCounter.State.Frided || e == StoveCounter.State.Friding;
        if(e != state)
        {
            if (isPlay)
            {
                source.Play();
            }
            else
            {
                source.Pause();
            }
        }
        state = e;
    }

    // Update is called once per frame
}
