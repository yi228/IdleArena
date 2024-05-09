using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public Quest quest { get; private set; }

    public List<Quest> typeQuestList = new List<Quest>();

    [SerializeField] private TextMeshProUGUI mainText;
    private Button clearButton;

    void Start()
    {
        clearButton = GetComponentInChildren<Button>();
    }
    public void SetButtonInteractable(bool _able)
    {
        clearButton.interactable = _able;
    }
    public bool CheckClear()
    {
        bool _ret;
        switch (quest.type)
        {
            case Quest.Type.Stage:
                if (GameManager.instance.stageLevel >= quest.goal)
                    _ret = true;
                else
                    _ret = false;
                break;
            case Quest.Type.Level:
                if(GameManager.instance.playerLevel >= quest.goal - 1)
                    _ret = true;
                else 
                    _ret = false;
                break;
            case Quest.Type.Gold:
                if(GoodsSystem.instance.usedGold >= quest.goal)
                    _ret = true;
                else
                    _ret = false;
                break;
            default:
                _ret = false;
                break;
        }
        return _ret;
    }
    public void SetSlot(Quest _quest)
    {
        quest = _quest;

        string _tempText = "";
        switch (quest.type)
        {
            case Quest.Type.Stage:
                _tempText = $"스테이지 {quest.goal} 클리어";
                break;
            case Quest.Type.Level:
                _tempText = $"레벨 {quest.goal} 달성";
                break;
            case Quest.Type.Gold:
                _tempText = $"골드 {quest.goal} 사용";
                break;
        }
        mainText.text = _tempText;

        clearButton.GetComponentInChildren<TextMeshProUGUI>().text = quest.reward.ToString();
        clearButton.onClick.RemoveAllListeners();
        clearButton.onClick.AddListener(ClearQuest);
    }
    private void ClearQuest()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GoodsSystem.instance.GetGoods(GoodsSystem.GoodsType.Diamond, quest.reward);
        SetNextQuest();
    }
    private void SetNextQuest()
    {
        switch (quest.type)
        {
            case Quest.Type.Stage:
                QuestSystem.instance.stageInd++;
                if (typeQuestList.Count > QuestSystem.instance.stageInd && 
                    typeQuestList[QuestSystem.instance.stageInd] != null)
                    SetSlot(typeQuestList[QuestSystem.instance.stageInd]);
                else
                    SetClearedQuest();
                break;
            case Quest.Type.Level:
                QuestSystem.instance.levelInd++;
                if(typeQuestList.Count > QuestSystem.instance.levelInd && 
                    typeQuestList[QuestSystem.instance.levelInd] != null)
                    SetSlot(typeQuestList[QuestSystem.instance.levelInd]);
                else
                    SetClearedQuest();
                break;
            case Quest.Type.Gold:
                QuestSystem.instance.goldInd++;
                if (typeQuestList.Count > QuestSystem.instance.goldInd &&
                    typeQuestList[QuestSystem.instance.goldInd] != null)
                    SetSlot(typeQuestList[QuestSystem.instance.goldInd]);
                else
                    SetClearedQuest();
                break;
        }
    }
    public void SetClearedQuest()
    {
        clearButton.GetComponentInChildren<TextMeshProUGUI>().text = "0";
        mainText.text = "퀘스트 클리어!";
        SetButtonInteractable(false);
    }
}
