using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailSlot : MonoBehaviour
{
    [SerializeField] private GameObject checkMark;
    [SerializeField] private Button checkButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI senderText;

    private Mail mail;
    private bool check = false;

    void Start()
    {
        AddButtonEvent();
        checkMark.SetActive(false);
    }
    private void AddButtonEvent()
    {
        GetComponent<Button>().onClick.AddListener(OpenMailSpec);
        checkButton.onClick.AddListener(SetCheckMail);
    }
    public void SetSlot(Mail _mail)
    {
        mail = _mail;
        titleText.text = _mail.title;
        senderText.text = _mail.sender;

        if (MailSystem.instance.seenMailIndex.Contains(mail.index))
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
    }
    private void OpenMailSpec()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");

        if (!MailSystem.instance.seenMailIndex.Contains(mail.index))
        {
            MailSystem.instance.seenMailIndex.Add(mail.index);
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        
        MailSystem.instance.OpenSpecPanel(mail);
    }
    private void SetCheckMail()
    {
        if (!check)
        {
            check = true;
            MailSystem.instance.AddCheckMail(mail);
            checkMark.SetActive(true);
        }
        else
        {
            check = false;
            MailSystem.instance.RemoveCheckMail(mail);
            checkMark.SetActive(false);
        }
    }
}
