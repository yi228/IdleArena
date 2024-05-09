using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleController : BaseIdleController
{
    [SerializeField] private LayerMask monsterLayer;
    private int prevLevInd = 0;
    private int curLevInd = 0;
    private int maxLevInd = 2;
    private int heroInd;

    private float _dir = 0f;

    private bool initComplete = false;
    private bool doScan = true;
    private bool doChase = false;
    private bool canAttack = true;

    public float extraAttack = 0f;
    public float gemAttack = 0f;
    
    public float extraSpeed = 0f;
    public float gemSpeed = 0f;

    public float attackDist = 1f;

    private int attackCount = 0;
    public int maxAttackCount = 3;

    public float defense = 0;
    public float goldBonus = 0;

    private GameObject target;
    public GameObject Target { get { return target; } }
    private CharacterController curCharacter;
    private CharacterController[] charList;

    private AudioSource audioSource;
    private AudioSource charAudioSource;

    [SerializeField] private List<Transform> textEffectPos;
    [SerializeField] private List<Transform> effectPos;
    public Transform gemDest;
    [SerializeField] private GameObject damageTextEffect;
    [SerializeField] private GameObject skillTextEffect;

    void Start()
    {
        Init();
    }
    private void Init()
    {
        state = CreatureState.IDLE;
        moveDir = Dir.LEFT;

        GameManager.instance.player = this;
        stat = new Stat();
        DBManager.instance.GetComponent<DataTransfer>().ApplyData();

        heroInd = GameManager.instance.curHeroInd;
        curLevInd = (GameManager.instance.playerLevel + 1) / 10;
        if(curLevInd > maxLevInd)
            curLevInd = maxLevInd;
        prevLevInd = curLevInd;
        charList = GetComponentsInChildren<CharacterController>();
        curCharacter = charList[heroInd * 3 + curLevInd];
        curCharacter.creatureController = this;
        for (int i = 0; i < charList.Length; i++)
            if (i != heroInd * 3 + curLevInd)
                charList[i].gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        charAudioSource = curCharacter.GetComponent<AudioSource>();

        initComplete = true;
    }
    void Update()
    {
        if (initComplete && !dead)
        {
            SetDir();
            UpdateState();
            if (doScan)
                ScanMonster();
            if (doChase)
                DoChase();
            if(stat.exp >= stat.maxExp)
                LevelUp();
        }
    }
    protected override void SetDir()
    {
        base.SetDir();
        if (_dir < 0)
        {
            moveDir = Dir.LEFT;
            curCharacter.ChangeSide(true);
        }
        else
        {
            moveDir = Dir.RIGHT;
            curCharacter.ChangeSide(false);
        }
    }
    protected override void UpdateState()
    {
        base.UpdateState();

        switch (state)
        {
            case CreatureState.IDLE:
                audioSource.Stop();
                curCharacter.IdleAnimation();
                break;
            case CreatureState.MOVE:
                curCharacter.MoveAnimation();
                break;
            case CreatureState.ATTACK:
                Attack();
                break;
        }
    }
    private void ScanMonster()
    {
        state = CreatureState.IDLE;
        RaycastHit2D[] _scanned = Physics2D.CircleCastAll(transform.position, 50f, Vector2.up, 0f, monsterLayer);
        
        if (_scanned.Length > 0)
        {
            float _minDist = 50f;
            for (int i = 0; i < _scanned.Length; i++)
            {
                float _tempDist = Vector2.Distance(transform.position, _scanned[i].transform.position);
                if (!_scanned[i].transform.GetComponentInParent<BaseIdleController>().dead && _tempDist < _minDist)
                {
                    _minDist = _tempDist;
                    target = _scanned[i].transform.gameObject;
                }
            }
            if(target != null)
            {
                doScan = false;
                Chase();
            }
            else
                doScan = true;
        }
        else
            doScan = true;
    }
    protected override void Chase()
    {
        base.Chase();
        _dir = target.transform.position.x - transform.position.x;
        doChase = true;
        //SoundManager.instance.PlayClip(audioSource, "WALK", true);
    }
    private void DoChase()
    {
        if (Vector2.Distance(transform.position, target.transform.position) > attackDist)
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, (stat.speed + extraSpeed + gemSpeed) * Time.deltaTime);
        else if (Vector2.Distance(transform.position, target.transform.position) <= attackDist)
        {
            curCharacter.IdleAnimation();
            state = CreatureState.ATTACK;
        }
        if (target != null && target.GetComponentInParent<BaseIdleController>() != null)
            curCharacter.target = target.GetComponentInParent<BaseIdleController>();
    }
    protected override void Attack()
    {
        doChase = false;
        if (target != null && target.GetComponentInParent<BaseIdleController>().dead)
        {
            target = null;
            doScan = true;
            return;
        }
        if (canAttack)
        {
            base.Attack();
            canAttack = false;
            if (attackCount < maxAttackCount || SkillManager.instance.curSkillInd == -1)
            {
                attackCount++;
                curCharacter.AttackAnimation();
            }
            else
            {
                attackCount = 0;
                curCharacter.SkillAnimation();
                SkillManager.instance.UseSkill(moveDir == Dir.LEFT ? effectPos[0].position : effectPos[1].position, moveDir == Dir.LEFT ? "L" : "R");
            }
            StartCoroutine(CoAttack());
        }
    }
    private IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(stat.attackCool);
        canAttack = true;
    }
    private void LevelUp()
    {
        if (stat.exp >= stat.maxExp)
        {
            GameManager.instance.playerLevel++;
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "LEVEL_UP");
            curLevInd = (GameManager.instance.playerLevel + 1) / 10;
            if (curLevInd > maxLevInd)
                curLevInd = maxLevInd;
            if (curLevInd > prevLevInd)
            {                
                charList[heroInd * 3 + curLevInd].gameObject.SetActive(true);
                curCharacter = charList[heroInd * 3 + curLevInd];
                curCharacter.creatureController = this;
                curCharacter.target = charList[heroInd * 3 + prevLevInd].target;
                charList[heroInd * 3 + prevLevInd].gameObject.SetActive(false);
                prevLevInd = curLevInd;
                UIManager.instance.EvoPanelOn();
                StatEvolution();
                charAudioSource = curCharacter.GetComponent<AudioSource>();
                charAudioSource.mute = SoundManager.instance.effectMute;
            }
            stat.hp = stat.maxHp;
            stat.maxExp = GSSManager.instance.playerMaxExp[GameManager.instance.playerLevel];
            stat.exp = 0;
            GameManager.instance.statUpPoint ++;
            SkillManager.instance.UpdateSkillSlots();
            DBManager.instance.SaveUserInfo();
        }
    }
    private void StatEvolution()
    {
        stat.maxHp += 50;
        stat.speed -= 0.5f;
        stat.attack += 10;
    }
    public override void OnDamaged(float _damage)
    {
        if (!dead)
        {
            base.OnDamaged(_damage * (1 - defense));
            curCharacter.DamageAnimation();
            SoundManager.instance.PlayClip(charAudioSource, "HIT");
        }
    }
    protected override void OnDead()
    {
        base.OnDead();
        SoundManager.instance.PlayClip(audioSource, "DIE");
        curCharacter.DeadAnimation();

        StartCoroutine(CoGameOver());
    }
    private IEnumerator CoGameOver()
    {
        UIManager.instance.SetOverUI(true);

        for(int i = 5; i > 0; i--)
        {
            UIManager.instance.SetOverTimer(i);
            yield return new WaitForSeconds(1f);
        }

        UIManager.instance.SetOverUI(false);
        RestartGame();
    }
    private void RestartGame()
    {
        GameManager.instance.killCount = 0;
        GameManager.instance.stageLevel--;
        if (GameManager.instance.stageLevel < 0)
            GameManager.instance.stageLevel = 0;
        stat.exp = 0;
        stat.hp = stat.maxHp;
        curCharacter.IdleAnimation();
        GemSystem.instance.curGem = null;
        dead = false;
    }
    public void ChangeHero(int _heroInd)
    {
        int _prevInd = heroInd;
        heroInd = _heroInd;

        if(heroInd != _prevInd)
        {
            charList[heroInd * 3 + curLevInd].gameObject.SetActive(true);
            curCharacter = charList[heroInd * 3 + curLevInd];
            curCharacter.creatureController = this;
            charAudioSource = curCharacter.GetComponent<AudioSource>();
            curCharacter.target = charList[_prevInd * 3 + curLevInd].target;

            charList[_prevInd * 3 + curLevInd].gameObject.SetActive(false);
        }
    }
    public void InstantiateDamageText(float _damage, bool _skill = false)
    {
        GameObject _go = Instantiate(_skill ? skillTextEffect : damageTextEffect);
        _go.transform.position = moveDir == Dir.LEFT ? textEffectPos[0].position : textEffectPos[1].position;
        _go.GetComponent<UI_DamageText>().damage = _damage;
        Destroy(_go, 1f);
    }
    public void InstantiateAttackEffect()
    {
        EffectManager.instance.InstantiateEffect("NORMAL_ATTACK", moveDir == Dir.LEFT ? effectPos[0].position : effectPos[1].position, moveDir == Dir.LEFT ? "L" : "R", 1f);
    }
}
