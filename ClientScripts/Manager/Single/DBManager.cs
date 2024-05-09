using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using static DBManager;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DBManager : MonoBehaviour
{
    public class PlayerData
    {
        public string name;
        //플레이어 스탯
        public float maxHp;
        public float hp;
        public float speed;
        public float attack;
        public float attackCool;
        public float exp;
        public float maxExp;
        //레벨 데이터
        public int playerLevel;
        public int stageLevel;
        public int killCount;
        public int statUpPoint;
        //보석 데이터
        public List<int> gemType;
        public List<int> gemProb;
        public int curGemType;
        public int curGemProb;
        //영웅 데이터
        public int curHeroInd;
        public List<int> ownHeroIndex;
        //메일 데이터
        public List<int> seenMailIndex;
        public List<int> receivedMailIndex;
        public List<int> deletedMailIndex;
        //재화 데이터
        public int gold;
        public int diamond;
        public int heroTicket;
        public int equipmentTicket;
        public int usedGold;
        //퀘스트 데이터
        public int stageQuestInd;
        public int levelQuestInd;
        public int goldQuestInd;
        //스킬 데이터
        public int curSkillInd;
        public List<int> skillIndex;
        //장비 데이터
        public List<int> rigIndex;
        public int curArmor;
        public int curWeapon;
        //아레나 데이터
        public int arenaScore;
    }

    public static DBManager instance;
    public string clientUserName;
    public int userCount = 0;
    public bool loadComplete { get;  private set; } = false;
    public bool isFirst { get; private set; }

    private DataTransfer dataTrans;

    DatabaseReference reference;

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
        Init();
        //LoadUserData();
    }
    private void Init()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //clientUserName = "Guest";
        dataTrans = GetComponent<DataTransfer>();
    }
    public void SaveUserInfo()
    {
        WriteNewUser("PlayerStat", clientUserName, SetPlayerData());
    }
    private void WriteNewUser(string _type, string _name, string _json)
    {
        reference.Child(_type).Child(_name).SetRawJsonValueAsync(_json);
    }
    public void LoadUserData()
    {
        Debug.Log("Load Data");
        reference.Child("PlayerStat").Child(clientUserName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
                Debug.Log("DB data load error");
            else if (task.IsCompleted)
            {
                DataSnapshot _snapshot = task.Result;

                string _json = _snapshot.GetRawJsonValue();
                dataTrans.playerData = JsonUtility.FromJson<PlayerData>(_json);

                if(dataTrans.playerData == null)
                    isFirst = true;
                else 
                    isFirst = false;

                loadComplete = true;
            }
        });
    }
    private string SetPlayerData()
    {
        PlayerData _user = new PlayerData();
        BaseIdleController.Stat _stat = GameManager.instance.player.stat;
        _user.name = clientUserName;
        //스탯 저장
        _user.hp = _stat.hp;
        _user.maxHp = _stat.maxHp;
        _user.attack = _stat.attack;
        _user.attackCool = _stat.attackCool;
        _user.exp = _stat.exp;
        _user.maxExp = _stat.maxExp;
        _user.speed = _stat.speed;
        //레벨 저장
        _user.playerLevel = GameManager.instance.playerLevel;
        _user.stageLevel = GameManager.instance.stageLevel;
        _user.killCount = GameManager.instance.killCount;
        _user.statUpPoint = GameManager.instance.statUpPoint;
        //보석 저장
        _user.gemType = new List<int>();
        _user.gemProb = new List<int>();
        int _size = GemSystem.instance.gemList.Count;
        for(int i=0; i<_size; i++)
        {
            _user.gemType.Add((int)GemSystem.instance.gemList[i].gemType);
            _user.gemProb.Add(GemSystem.instance.gemList[i].effectProb);
        }
        if(GemSystem.instance.curGem != null)
        {
            _user.curGemType = (int)GemSystem.instance.curGem.gemType;
            _user.curGemProb = GemSystem.instance.curGem.effectProb;
        }
        //영웅 저장
        _user.curHeroInd = GameManager.instance.curHeroInd;
        //_user.heroTicket = HeroSystem.instance.GetComponentInChildren<HeroGacha>().gachaTicket;
        _user.ownHeroIndex = new List<int>();
        for (int i = 0; i < HeroSystem.instance.heroList.Count; i++)
            if (HeroSystem.instance.heroList[i].own)
                _user.ownHeroIndex.Add(i);
        //메일 저장
        _user.seenMailIndex = MailSystem.instance.seenMailIndex;
        _user.receivedMailIndex = MailSystem.instance.receivedMailIndex;
        _user.deletedMailIndex = MailSystem.instance.deletedMailIndex;
        //재화 저장
        _user.gold = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.Gold);
        _user.diamond = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.Diamond);
        _user.heroTicket = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.HeroTicket);
        _user.equipmentTicket = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.EquipmentTicket);
        _user.usedGold = GoodsSystem.instance.usedGold;
        //퀘스트 저장
        _user.stageQuestInd = QuestSystem.instance.stageInd;
        _user.levelQuestInd = QuestSystem.instance.levelInd;
        _user.goldQuestInd = QuestSystem.instance.goldInd;
        //스킬 저장
        _user.curSkillInd = SkillManager.instance.curSkillInd;
        _user.skillIndex = SkillManager.instance.ownSkillIndex;
        //장비 저장
        _user.rigIndex = EquipmentSystem.instance.ownRigNum;
        if(EquipmentSystem.instance.curArmor != null)
            _user.curArmor = EquipmentSystem.instance.curArmor.index;
        else
            _user.curArmor = -1;
        if(EquipmentSystem.instance.curWeapon != null)
            _user.curWeapon = EquipmentSystem.instance.curWeapon.index;
        else 
            _user.curWeapon = -1;
        //아레나 저장
        _user.arenaScore = ArenaSystem.instance.score;

        string _json = JsonUtility.ToJson(_user);
        return _json;
    }
    public void LoadRankingData()
    {
        reference.Child("PlayerStat").OrderByChild("arenaScore").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
                Debug.Log("DB data load error");
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                userCount = (int)snapshot.ChildrenCount;
                int _rank = 0;
                foreach (DataSnapshot data in snapshot.Children.Reverse<DataSnapshot>())
                {
                    IDictionary personInfo = (IDictionary)data.Value;

                    string _name = personInfo["name"].ToString();
                    int _score = int.Parse(personInfo["arenaScore"].ToString());
                    _rank++;

                    Debug.Log(_name);

                    if (_name == clientUserName)
                    {
                        ArenaSystem.instance.rank = _rank;
                        ArenaSystem.instance.score = _score;
                    }
                    ArenaSystem.Ranker _temp = new ArenaSystem.Ranker();
                    _temp.name = _name;
                    _temp.rank = _rank;
                    _temp.score = _score;
                    ArenaSystem.instance.rankerList.Add(_temp);
                }
            }
        });
    }
}
