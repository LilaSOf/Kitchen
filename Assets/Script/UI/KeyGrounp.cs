using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KeyGrounp : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("按钮属性")]
    public KeyAttribute keyAttribute;

    private Button interactButton;//更改按键的按钮

    private TextMeshProUGUI visualText;//显示按键的文本

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
   /// 刷新按键显示
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
