using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBarUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private IHasProgress cuttingCounter;

    [SerializeField]private Image image;
    [SerializeField] private GameObject cuttingSlider;
    private void OnEnable()
    {
        cuttingCounter = GetComponentInParent<IHasProgress>();
        cuttingCounter.OnProgressChanged += OnProgressVisualEvent;
    }

   /// <summary>
   /// �����¼���ʾ��ӦUI
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="currentCuttingProgress">UI��Ӧ��ʾ��ֵ</param>
   private void OnProgressVisualEvent(object sender, IHasProgress.OnProgressChangedEvnetArgs e)
    {
        cuttingSlider.SetActive(e.progressNormalized != 0);
        image.fillAmount = e.progressNormalized;
    }

    private void OnDisable()
    {
        cuttingCounter.OnProgressChanged -= OnProgressVisualEvent;
    }
}
