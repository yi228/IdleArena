using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailSpec : MonoBehaviour
{
    private Mail mail;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI senderText;
    [SerializeField] private TextMeshProUGUI contentText;

    [SerializeField] private GameObject rewardFrame;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button rewardReceiveButton;

    [SerializeField] private Image rewardIconImage;
    [SerializeField] private TextMeshProUGUI rewardNumText;

    [SerializeField] private Sprite[] iconSprites;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePanel);
        rewardReceiveButton.onClick.AddListener(ReceiveReward);
    }
    private void ClosePanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GetComponent<Animator>().SetTrigger("Off");
    }
    private void ReceiveReward()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        MailSystem.instance.receivedMailIndex.Add(mail.index);
        rewardReceiveButton.interactable = false;

        GoodsSystem.instance.GetGoods((GoodsSystem.GoodsType)(mail.rewardType - 1), mail.rewardNum);
    }
    public void SetSpec(Mail _mail)
    {
        mail = _mail;

        rewardFrame.SetActive(true);
        rewardReceiveButton.gameObject.SetActive(true);

        titleText.text = mail.title;
        senderText.text = mail.sender;
        string _tempContent = mail.content.Replace("#", "\n");

        string _tempRewardText = "";
        switch (mail.rewardType)
        {
            case MailSystem.RewardType.None:
                rewardFrame.SetActive(false);
                rewardReceiveButton.gameObject.SetActive(false);
                break;
            case MailSystem.RewardType.Gold:
                _tempRewardText = "골드";
                rewardIconImage.sprite = iconSprites[0];
                break;
            case MailSystem.RewardType.Diamond:
                _tempRewardText = "다이아몬드";
                rewardIconImage.sprite = iconSprites[1];
                break;
            case MailSystem.RewardType.HeroTicket:
                _tempRewardText = "영웅 소환권";
                rewardIconImage.sprite = iconSprites[2];
                break;
            case MailSystem.RewardType.EquipmentTicket:
                _tempRewardText = "장비 뽑기권";
                rewardIconImage.sprite = iconSprites[3];
                break;
        }
        contentText.text = $"{_tempContent}\n\n[보상 내용]\n{_tempRewardText}X{mail.rewardNum}";
        rewardNumText.text = mail.rewardNum.ToString();

        if(MailSystem.instance.receivedMailIndex.Contains(mail.index))
            rewardReceiveButton.interactable = false;
        else
            rewardReceiveButton.interactable = true;
    }
}
