using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem instance;

    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject[] questSlots;

    public List<Quest> questList;
    public int stageInd, levelInd, goldInd;

    private bool[] canClear;
    public int clearQuestNum { get; private set; }

    void Awake()
    {
        instance = this;
        stageInd = levelInd = goldInd = 0;
    }
    void Start()
    {
        questList = GSSManager.instance.questData;
        SetSlotsQuestList();
        InitSlots();
        closeButton.onClick.AddListener(ClosePanel);
        canClear = new bool[3] { false, false, false };
        clearQuestNum = 0;
    }
    private void ClosePanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GetComponent<Animator>().SetTrigger("Off");
        DBManager.instance.SaveUserInfo();
    }
    void Update()
    {
        for (int i = 0; i < questSlots.Length; i++)
        {
            if (questSlots[i].GetComponent<QuestSlot>().quest != null && questSlots[i].GetComponent<QuestSlot>().CheckClear())
            {
                questSlots[i].GetComponent<QuestSlot>().SetButtonInteractable(true);
                canClear[i] = true;
            }
            else
            {
                questSlots[i].GetComponent<QuestSlot>().SetButtonInteractable(false);
                canClear[i] = false;
            }     
        }
        bool[] _temp = canClear;
        clearQuestNum = _temp.Where(n => n == true).Count();
    }
    private void SetSlotsQuestList()
    {
        for(int i=0; i<questList.Count; i++)
        {
            int _mod = i % 3;
            questSlots[_mod].GetComponent<QuestSlot>().typeQuestList.Add(questList[i]);
        }
    }
    private void InitSlots()
    {
        if (questSlots[0].GetComponent<QuestSlot>().typeQuestList[stageInd] != null)
            questSlots[0].GetComponent<QuestSlot>().SetSlot(questSlots[0].GetComponent<QuestSlot>().typeQuestList[stageInd]);
        else
            questSlots[0].GetComponent<QuestSlot>().SetClearedQuest();

        if (questSlots[1].GetComponent<QuestSlot>().typeQuestList[levelInd] != null)
            questSlots[1].GetComponent<QuestSlot>().SetSlot(questSlots[1].GetComponent<QuestSlot>().typeQuestList[levelInd]);
        else
            questSlots[1].GetComponent<QuestSlot>().SetClearedQuest();

        if (questSlots[2].GetComponent<QuestSlot>().typeQuestList[goldInd] != null)
            questSlots[2].GetComponent<QuestSlot>().SetSlot(questSlots[2].GetComponent<QuestSlot>().typeQuestList[goldInd]);
        else
            questSlots[2].GetComponent<QuestSlot>().SetClearedQuest();
    }
}
