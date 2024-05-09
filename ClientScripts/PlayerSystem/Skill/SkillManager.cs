using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillManager : SystemPanel
{
    public static SkillManager instance;

    [SerializeField] private Button closeButton;
    public List<GameObject> skillObjectList;
    [SerializeField] private Transform slotPanel;
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private GameObject confirmPanel;

    public List<int> ownSkillIndex = new List<int>();

    private List<SkillSlot> skillSlotList = new List<SkillSlot>();
    private GameObject curSkillObject;
    public int curSkillInd { get; private set; } = -1;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for(int i= 0; i< skillObjectList.Count; i++)
        {
            GameObject _go = Instantiate(skillSlotPrefab, slotPanel);
            _go.GetComponent<SkillSlot>().Init(skillObjectList[i].GetComponent<Skill>(), i);
            skillSlotList.Add(_go.GetComponent<SkillSlot>());
        }
        closeButton.onClick.AddListener(ClosePanel);
    }
    protected override void ClosePanel()
    {
        base.ClosePanel();
        DBManager.instance.SaveUserInfo();
    }
    public void SetCurrentSkill(int _ind)
    {
        curSkillInd = _ind;
        if(curSkillInd != -1)
            curSkillObject = skillObjectList[curSkillInd];
    }
    public void UseSkill(Vector3 _pos, string _dir = "")
    {
        GameObject _go = Instantiate(curSkillObject);
        SoundManager.instance.PlayClip(GetComponentInParent<AudioSource>(), _go.GetComponent<Skill>().soundKey);

        if (_go.GetComponent<Skill>().dirType == Skill.DirType.Center)
            _go.transform.position = GameManager.instance.player.transform.position;
        else if (_go.GetComponent<Skill>().dirType == Skill.DirType.LR)
            _go.transform.position = _pos;

        if (_dir == "L")
            _go.transform.eulerAngles = new Vector3(_go.transform.eulerAngles.x, 180, _go.transform.eulerAngles.z);
        else if (_dir == "R")
            _go.transform.eulerAngles = new Vector3(_go.transform.eulerAngles.x, 0, _go.transform.eulerAngles.z);
    }
    public void OpenConfirmPanel(UnityAction _event, Skill _skill, bool _have)
    {
        confirmPanel.SetActive(true);
        confirmPanel.GetComponent<SkillConfirm>().SetConfirmPanel(_event, _skill, _have);
    }
    public void UpdateSkillSlots()
    {
        for (int i = 0; i < skillSlotList.Count; i++)
            skillSlotList[i].SetSlotStatus();
    }
}
