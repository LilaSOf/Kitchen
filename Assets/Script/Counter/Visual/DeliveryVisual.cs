using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeliveryVisual : MonoBehaviour
{
    private const string SUCCESS = "成功提交";
    private const string FAILURE = "提交失败";
    // Start is called before the first frame update
    [Header("面板显示组件")]
    [SerializeField] private TextMeshProUGUI submitText;
    [SerializeField] private Image backGround;
    [SerializeField] private Image statusSign;

    [Header("需要显示的图片")]
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failureSprite;

    [Header("控制开关")]
    [SerializeField]private GameObject deliveryVisual;
    void Start()
    {
        DeliveryManage.Instance.DeliverySuccess += OnDeliverSuccessEvent;
        DeliveryManage.Instance.DeliveryFailure += OnDeliveryFailureEvent;

        Hide();
    }

    private void OnDeliveryFailureEvent(object sender, EventArgs e)
    {
        Show();
        submitText.text = FAILURE;
        backGround.color = Color.red;

        statusSign.sprite = failureSprite;

        Invoke("Hide", 1f);
    }

    private void OnDeliverSuccessEvent(object sender, EventArgs e)
    {
        Show();
        submitText.text = SUCCESS;
        backGround.color =Color.green;

        statusSign.sprite = successSprite;

        Invoke("Hide", 1f);
    }


    private void Show()
    {
        deliveryVisual.SetActive(true);
    }

    private void Hide()
    {
        deliveryVisual.SetActive(false);
    }
    // Update is called once per frame
}
