using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyConfirm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public int quantity;

    private Vector3 cancelButtonOriginPos;

    void Awake()
    {
        cancelButtonOriginPos = cancelButton.GetComponent<RectTransform>().anchoredPosition;
    }
    void Start()
    {
        cancelButton.onClick.AddListener(ClosePanel);
    }
    private void ClosePanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        gameObject.SetActive(false);
    }
    private void CompletePurchase()
    {
        mainText.text = "���� �Ϸ�Ǿ����ϴ�!";
        confirmButton.gameObject.SetActive(false);
        cancelButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, cancelButtonOriginPos.y, cancelButtonOriginPos.z);
        cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "�ݱ�";
    }
    public void SetConfirmPanel(UnityAction _event, int _itemInd)
    {
        cancelButton.GetComponent<RectTransform>().anchoredPosition = cancelButtonOriginPos;
        cancelButton.GetComponentInChildren<TextMeshProUGUI>().text = "���";

        confirmButton.gameObject.SetActive(true);
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(_event);
        confirmButton.onClick.AddListener(CompletePurchase);

        switch (_itemInd)
        {
            case 0:
                mainText.text = $"100 ��� X{quantity} �����Ͻðڽ��ϱ�?";
                break;
            case 2:
                mainText.text = $"������ȯ�� 1��X{quantity} �����Ͻðڽ��ϱ�?";
                break;
            case 3:
                mainText.text = $"����ȯ�� 1��X{quantity} �����Ͻðڽ��ϱ�?";
                break;
            case 4:
                mainText.text = $"���� ����Ͻðڽ��ϱ�?";
                break;
        }
    }
}
