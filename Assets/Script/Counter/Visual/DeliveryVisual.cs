using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeliveryVisual : MonoBehaviour
{
    private const string SUCCESS = "�ɹ��ύ";
    private const string FAILURE = "�ύʧ��";
    // Start is called before the first frame update
    [Header("�����ʾ���")]
    [SerializeField] private TextMeshProUGUI submitText;
    [SerializeField] private Image backGround;
    [SerializeField] private Image statusSign;

    [Header("��Ҫ��ʾ��ͼƬ")]
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failureSprite;

    [Header("���ƿ���")]
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
