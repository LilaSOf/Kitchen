using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KeyGrounp : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("��ť����")]
    public KeyAttribute keyAttribute;

    private Button interactButton;//���İ����İ�ť

    private TextMeshProUGUI visualText;//��ʾ�������ı�

    void Start()
    {
        Hide();
        interactButton = transform.Find("Button").GetComponent<Button>();
        visualText = interactButton.GetComponentInChildren<TextMeshProUGUI>();
        interactButton.onClick.AddListener(() =>
        {
            Show();
            GameInputManager.Instance.SetNewKey(keyAttribute,()=> { Hide(); UpDateVisual(); });
        });
        UpDateVisual();
    }

    // Update is called once per frame

   /// <summary>
   /// ˢ�°�����ʾ
   /// </summary>
   private void UpDateVisual()
    {
        visualText.text = GameInputManager.Instance.GetKeyName(keyAttribute);
    }

    private void Show()
    {
        transform.parent.Find("Mask").gameObject.SetActive(true);
    }

    private void Hide()
    {
        transform.parent.Find("Mask").gameObject.SetActive(false);
    }
}
