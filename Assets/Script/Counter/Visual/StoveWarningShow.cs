using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveWarningShow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private StoveCounter stoveCounter;

    [SerializeField] private GameObject waringSign;

    private bool isFired;
    [Header("音效计数器")]
    private float audioTimer;
    private float audioMaxTimer = 0.5f;
    void Start()
    {
        stoveCounter.OnProgressChanged += OnProgressChangedEvent;
    }

    private void OnProgressChangedEvent(object sender, IHasProgress.OnProgressChangedEvnetArgs e)
    {
         isFired = e.progressNormalized > 0.5 && stoveCounter.IsFiredWarning();

        if (isFired)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        audioTimer -= Time.deltaTime;
        if(audioTimer < 0)
        {
            audioTimer = audioMaxTimer;
            //播放音效
            if(isFired)
            {
                AudioManager.Instance.PlayStoveWaringSource(transform.position);
            }
        }
    }
    private void Hide()
    {
        waringSign.SetActive(false);
    }
    private void Show()
    {
        waringSign.SetActive(true);
    }
    // Update is called once per frame
}