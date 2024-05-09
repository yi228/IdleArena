using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;

    public List<GameObject> skillList = new List<GameObject>();

    public StatInfo myPlayerStat;
    public int maxAttackCount = 0;
    public int score; // 현재 점수
    public int point; // 얻거나 잃는 점수

    public bool scoreUpdated = true;

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
        myPlayerStat = new StatInfo();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if(!scoreUpdated && ArenaSystem.instance != null)
            {
                scoreUpdated = true;
                ArenaSystem.instance.score = score;
                DBManager.instance.SaveUserInfo();
            }
            SetStatInfo();
        }
    }
    private void  SetStatInfo()
    {
        PlayerIdleController _player = GameManager.instance.player;
        myPlayerStat.MaxHp = (int)_player.stat.maxHp;
        myPlayerStat.Hp = myPlayerStat.MaxHp;
        myPlayerStat.Attack = (int)(_player.stat.attack + _player.extraAttack);
        myPlayerStat.Speed = _player.stat.speed + _player.extraSpeed;
        myPlayerStat.AttackCool = _player.stat.attackCool;
        myPlayerStat.SkillInd = SkillManager.instance.curSkillInd;
        myPlayerStat.Nickname = DBManager.instance.clientUserName;
        myPlayerStat.Score = ArenaSystem.instance.score;

        maxAttackCount = _player.maxAttackCount;

        if(skillList.Count != SkillManager.instance.skillObjectList.Count)
            skillList = SkillManager.instance.skillObjectList.ToList();
    }
    public void UseSkill(int skillInd, AudioSource audio, Vector3 _pos, string _dir = "")
    {
        GameObject _go = Instantiate(skillList[skillInd]);
        SoundManager.instance.PlayClip(audio, _go.GetComponent<Skill>().soundKey);

        if (_go.GetComponent<Skill>().dirType == Skill.DirType.Center)
            _go.transform.position = audio.transform.position;
        else if (_go.GetComponent<Skill>().dirType == Skill.DirType.LR)
            _go.transform.position = _pos;

        if (_dir == "L")
            _go.transform.eulerAngles = new Vector3(_go.transform.eulerAngles.x, 180, _go.transform.eulerAngles.z);
        else if (_dir == "R")
            _go.transform.eulerAngles = new Vector3(_go.transform.eulerAngles.x, 0, _go.transform.eulerAngles.z);
    }
}
