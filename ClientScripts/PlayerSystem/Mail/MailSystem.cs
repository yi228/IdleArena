using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailSystem : SystemPanel
{
    public static MailSystem instance;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotPanel;
    [SerializeField] private GameObject emptyText;
    [SerializeField] private GameObject specPanel;
    [SerializeField] private GameObject notifyIcon;

    private float timer = 0f;
    private float updateTick = 5f;

    public enum RewardType
    {
        None,
        Gold,
        Diamond,
        HeroTicket,
        EquipmentTicket
    }

    public List<Mail> mailList;
    private List<Mail> checkedMailList;

    public List<int> seenMailIndex;
    public List<int> receivedMailIndex;
    public List<int> deletedMailIndex;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        AddButtonEvent();
        mailList = new List<Mail>();
        checkedMailList = new List<Mail>();
    }
    private void AddButtonEvent()
    {
        closeButton.onClick.AddListener(ClosePanel);
        deleteButton.onClick.AddListener(DeleteMail);
    }
    protected override void ClosePanel()
    {
        base.ClosePanel();
        DBManager.instance.SaveUserInfo();
    }
    private void DeleteMail()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        
        foreach (Mail _mail in checkedMailList)
        {
            mailList.Remove(_mail);
            deletedMailIndex.Add(_mail.index);
        }

        SetMailSlot();
        DBManager.instance.SaveUserInfo();
    }
    void Update()
    {
        if(timer > 0f)
            timer -= Time.deltaTime;
        else
        {
            timer = updateTick;
            GetMail();
        }

        if (seenMailIndex.Count < mailList.Count || receivedMailIndex.Count < mailList.Count)
            notifyIcon.SetActive(true);
        else 
            notifyIcon.SetActive(false);
    }
    private void GetMail()
    {
        GSSManager.instance.GetMailData();
    }
    public void SetMailSlot()
    {
        ResetMailSlot();
        if(mailList.Count > 0)
        {
            emptyText.SetActive(false);
            for (int i = mailList.Count - 1; i >= 0; i--)
                if (!deletedMailIndex.Contains(mailList[i].index))
                {
                    GameObject _go = Instantiate(slotPrefab, slotPanel.transform);
                    _go.GetComponent<MailSlot>().SetSlot(mailList[i]);
                }
        }
        else
            emptyText.SetActive(true);
    }
    private void ResetMailSlot()
    {
        foreach (MailSlot _slot in slotPanel.GetComponentsInChildren<MailSlot>())
            Destroy(_slot.gameObject);
    }
    public void OpenSpecPanel(Mail _mail)
    {
        specPanel.GetComponent<Animator>().SetTrigger("On");
        specPanel.GetComponent<MailSpec>().SetSpec(_mail);
    }
    public void AddCheckMail(Mail _mail)
    {
        checkedMailList.Add(_mail);
    }
    public void RemoveCheckMail(Mail _mail)
    {
        checkedMailList.Remove(_mail);
    }
}
