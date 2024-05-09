using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GSSManager : MonoBehaviour //Google Spread Sheet
{
    public static GSSManager instance;

    public readonly string address = "https://docs.google.com/spreadsheets/d/1ghzQoHks4gnjF18lkP47xkxBn2jvBQU_J4vlS6kgWmI";
    public readonly string playerRange = "B2:B";
    public readonly long playerSheetId = 80843049;
    public readonly string goblinRange = "B2:J";
    public readonly long goblinSheetId = 1702115314;
    public readonly string stageRange = "B2:D";
    public readonly long stageSheetId = 844374999;
    public readonly string storyRange = "B2:D";
    public readonly long storySheetId = 1632756921;
    public readonly string mailRange = "A2:G";
    public readonly long mailSheetId = 1106445648;
    public readonly string questRange = "B2:D";
    public readonly long questSheetId = 252932656;
    public readonly string itemRange = "B2:F";
    public readonly long itemSheetId = 328763521;

    private UnityWebRequest playerReq;
    private UnityWebRequest goblinReq;
    private UnityWebRequest stageReq;
    private UnityWebRequest storyReq;
    private UnityWebRequest mailReq;
    private UnityWebRequest questReq;
    private UnityWebRequest itemReq;

    public struct GoblinGoldRange
    {
        public int maxGold;
        public int minGold;
    }

    public List<float> playerMaxExp {  get; private set; }
    public List<BaseIdleController.Stat> goblinStat { get; private set; }
    public List<GoblinGoldRange> goblinGold { get; private set; }
    public List<GoblinSpawnManager.TypePercent> goblinSpawnRatio { get; private set; }
    public List<List<string>> storyData { get; private set; }
    public List<Mail> mailData { get; private set; }
    public List<Quest> questData { get; private set; }
    public List<Rig> rigData { get; private set; }

    public float loadingProgress = 0f;

    void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion
    }
    void Start()
    {
        playerMaxExp = new List<float>();
        goblinStat = new List<BaseIdleController.Stat>();
        goblinGold = new List<GoblinGoldRange>();
        goblinSpawnRatio = new List<GoblinSpawnManager.TypePercent>();
        storyData = new List<List<string>>();
        mailData = new List<Mail>();
        questData = new List<Quest>();
        rigData = new List<Rig>();
        StartCoroutine(CoLoadData());
    }
    private IEnumerator CoLoadData()
    {
        playerReq = UnityWebRequest.Get(GetTSVAddress(address, playerRange, playerSheetId));
        goblinReq = UnityWebRequest.Get(GetTSVAddress(address, goblinRange, goblinSheetId));
        stageReq = UnityWebRequest.Get(GetTSVAddress(address, stageRange, stageSheetId));
        storyReq = UnityWebRequest.Get(GetTSVAddress(address, storyRange, storySheetId));
        questReq = UnityWebRequest.Get(GetTSVAddress(address, questRange, questSheetId));
        itemReq = UnityWebRequest.Get(GetTSVAddress(address, itemRange, itemSheetId));

        yield return playerReq.SendWebRequest();
        ParsePlayerData();
        loadingProgress += 1f / 7f;

        yield return goblinReq.SendWebRequest();
        ParseGoblinData();
        loadingProgress += 1f / 7f;

        yield return stageReq.SendWebRequest();
        ParseStageData();
        loadingProgress += 1f / 7f;

        yield return storyReq.SendWebRequest();
        ParseStoryData();
        loadingProgress += 1f / 7f;

        yield return questReq.SendWebRequest();
        ParseQuestData();
        loadingProgress += 1f / 7f;

        yield return itemReq.SendWebRequest();
        ParseItemData();
        loadingProgress += 1f / 7f;

        yield return null;
    }
    private string GetTSVAddress(string _address, string _range, long _id)
    {
        return $"{_address}/export?format=tsv&range={_range}&gid={_id}";
    }
    private void ParsePlayerData()
    {
        string[] _tempPlayer = playerReq.downloadHandler.text.Split('\n');
        for (int i = 0; i < _tempPlayer.Length; i++)
            playerMaxExp.Add(float.Parse(_tempPlayer[i]));
    }
    private void ParseGoblinData()
    {
        string[] _tempGoblin = goblinReq.downloadHandler.text.Split('\n');
        for (int i = 0; i < _tempGoblin.Length; i++)
        {
            string[] _tempData = _tempGoblin[i].Split("\t");
            BaseIdleController.Stat _tempGob = new BaseIdleController.Stat();
            _tempGob.maxHp = float.Parse(_tempData[0]);
            _tempGob.hp = float.Parse(_tempData[1]);
            _tempGob.speed = float.Parse(_tempData[2]);
            _tempGob.attack = float.Parse(_tempData[3]);
            _tempGob.attackCool = float.Parse(_tempData[4]);
            _tempGob.exp = float.Parse(_tempData[5]);
            _tempGob.maxExp = float.Parse(_tempData[6]);
            goblinStat.Add(_tempGob);

            GoblinGoldRange _tempRange = new GoblinGoldRange();
            _tempRange.minGold = int.Parse(_tempData[7]);
            _tempRange.maxGold = int.Parse(_tempData[8]);
            goblinGold.Add(_tempRange);
        }
    }
    private void ParseStageData()
    {
        string[] _tempStage = stageReq.downloadHandler.text.Split('\n');
        for (int i = 0; i < _tempStage.Length; i++)
        {
            string[] _tempData = _tempStage[i].Split("\t");
            GoblinSpawnManager.TypePercent _tempSta = new GoblinSpawnManager.TypePercent();
            _tempSta.normal = float.Parse(_tempData[0]);
            _tempSta.elite = float.Parse(_tempData[1]);
            _tempSta.boss = float.Parse(_tempData[2]);
            goblinSpawnRatio.Add(_tempSta);
        }
    }
    private void ParseStoryData()
    {
        string[] _tempStory = storyReq.downloadHandler.text.Split('\n');
        for (int i = 0; i < _tempStory.Length; i++)
        {
            string[] _tempData = _tempStory[i].Split("\t");
            storyData.Add(new List<string>());
            for(int j = 0; j < _tempData.Length; j++)
                storyData[i].Add(_tempData[j]);
        }
        StoryManager.instance.storyTextData = storyData[0];
    }
    private void ParseQuestData()
    {
        string[] _tempQuest = questReq.downloadHandler.text.Split('\n');
        for(int i=0; i < _tempQuest.Length; i++)
        {
            string[] _tempData = _tempQuest[i].Split("\t");
            Quest _quest = new Quest();
            _quest.type = (Quest.Type)int.Parse(_tempData[0]);
            _quest.goal = int.Parse(_tempData[1]);
            _quest.reward = int.Parse(_tempData[2]);
            questData.Add(_quest);
        }
    }
    private void ParseItemData()
    {
        string[] _tempItem = itemReq.downloadHandler.text.Split('\n');
        for(int i=0; i< _tempItem.Length; i++)
        {
            string[] _tempRig = _tempItem[i].Split("\t");
            Rig _rig = new Rig();
            _rig.index = i;
            _rig.type = (Rig.Type)int.Parse(_tempRig[0]);
            _rig.name = _tempRig[1];
            _rig.effectType = (Rig.EffectType)int.Parse(_tempRig[2]);
            _rig.effectProb = int.Parse(_tempRig[3]);
            _rig.description = _tempRig[4];
            rigData.Add(_rig);
        }
    }
    public void GetMailData()
    {
        StartCoroutine(CoGetMailData());
    }
    private IEnumerator CoGetMailData()
    {
        mailReq = UnityWebRequest.Get(GetTSVAddress(address, mailRange, mailSheetId));
        yield return mailReq.SendWebRequest();

        ParseMailData();
    }
    private void ParseMailData()
    {
        mailData.Clear();
        string[] _tempMail = mailReq.downloadHandler.text.Split("\n");
        for(int i=0; i < _tempMail.Length; i++)
        {
            string[] _tempData = _tempMail[i].Split("\t");
            char[] _confirm = _tempData[6].ToCharArray();

            if (_confirm[0].Equals('O'))
            {
                Mail _temp = new Mail();
                _temp.index = int.Parse(_tempData[0]);
                _temp.title = _tempData[1];
                _temp.content = _tempData[2];
                _temp.sender = _tempData[3];
                _temp.rewardType = (MailSystem.RewardType)int.Parse(_tempData[4]);
                _temp.rewardNum = int.Parse(_tempData[5]);

                mailData.Add(_temp);
            }
        }
        if(MailSystem.instance != null)
            MailSystem.instance.mailList = mailData;
    }
}
