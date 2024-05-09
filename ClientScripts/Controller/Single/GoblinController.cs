using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinController : BaseIdleController
{
    [SerializeField] private GameObject fieldGem;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Slider hpBar;

    private float _dir = 0f;
    private bool setComplete = false;
    private bool canAttack = true;
    
    private int level;
    private float hp;

    private int gold;

    private CharacterController curCharacter;
    private PlayerIdleController target;
    private AudioSource audioSource;

    void Init()
    {
        target = FindAnyObjectByType<PlayerIdleController>();
        state = CreatureState.IDLE;
        stat = new Stat();
        audioSource = GetComponent<AudioSource>();
        canvas.worldCamera = Camera.main;
    }
    void Update()
    {
        if (!dead && setComplete && target.stat.hp > 0)
        {
            curCharacter.target = target;
            _dir = transform.position.x - target.transform.position.x;
            SetDir();
            Chase();
            UpdateState();
        }
        hpBar.value = hp / stat.maxHp;
    }
    protected override void SetDir()
    {
        base.SetDir();
        if (_dir > 0)
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
    public void SetCharacter(int _charInd)
    {
        Init();

        level = _charInd;
        CharacterController[] charList = GetComponentsInChildren<CharacterController>();
        curCharacter = charList[_charInd];
        curCharacter.creatureController = this;
        for (int i = 0; i < charList.Length; i++)
            if (i != _charInd)
                charList[i].gameObject.SetActive(false);
        stat = GSSManager.instance.goblinStat[_charInd];
        hp = stat.hp;
        gold = Random.Range(GSSManager.instance.goblinGold[_charInd].minGold, GSSManager.instance.goblinGold[_charInd].maxGold + 1);
        setComplete = true;
    }
    protected override void Chase()
    {
        base.Chase();
        if(target.stat.hp > 0 && Vector2.Distance(target.transform.position, transform.position) > 1)
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, stat.speed * Time.deltaTime);
        else if (Vector2.Distance(transform.position, target.transform.position) <= 1)
        {
            curCharacter.IdleAnimation();
            state = CreatureState.ATTACK;
        }
    }
    protected override void Attack()
    {
        base.Attack();
        if (canAttack)
        {
            canAttack = false;
            curCharacter.AttackAnimation();
            StartCoroutine(CoAttack());
        }
    }
    private IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(stat.attackCool);
        canAttack = true;
    }
    public override void OnDamaged(float _damage)
    {
        if (!dead && Vector2.Distance(target.transform.position, transform.position) <=1.3f)
        {
            hp -= _damage;
            if(hp <= 0)
                OnDead();
            curCharacter.DamageAnimation();
        }
    }
    protected override void OnDead()
    {
        base.OnDead();
        audioSource.mute = SoundManager.instance.effectMute;
        SoundManager.instance.PlayClip(audioSource, "GOB_DIE", false, 50);
        GameManager.instance.killCount++;

        GameManager.instance.player.stat.exp += stat.exp;
        EarnedItemPanel.instance.InstantiateSlot(EarnedItemPanel.ItemType.Exp, stat.exp.ToString());

        if (GoodsSystem.instance != null)
        {
            GoodsSystem.instance.GetGoods(GoodsSystem.GoodsType.Gold, gold + (int)(gold * target.goldBonus));
            EarnedItemPanel.instance.InstantiateSlot(EarnedItemPanel.ItemType.Gold, gold.ToString());
        }

        curCharacter.DeadAnimation();
        Destroy(gameObject, 1f);

        if (GemSystem.instance.canAdd)
        {
            int _gemProb = Random.Range(0, 100);
            int _probNum;
            switch (level)
            {
                case 0:
                    _probNum = -1;
                    //_probNum = 89;
                    break;
                case 1:
                    _probNum = 69;
                    break;
                case 2:
                    _probNum = 49;
                    break;
                default:
                    _probNum = 89;
                    break;
            }
            if (_gemProb > _probNum)
            {
                fieldGem.SetActive(true);
                StartCoroutine(CoMoveFieldGem());
                GameObject _tempGem = Instantiate(GameManager.instance.gemPrefab, GameManager.instance.gemStorePos);
                _tempGem.GetComponent<GemController>().InitGem();
                GemSystem.instance.gemList.Add(_tempGem.GetComponent<GemController>());
                string _temp = $"{_tempGem.GetComponent<GemController>().gemType} {_tempGem.GetComponent<GemController>().effectProb} X1";
                EarnedItemPanel.instance.InstantiateSlot(EarnedItemPanel.ItemType.Gem, _temp);
            }
        }
        DBManager.instance.SaveUserInfo();
    }
    private float fieldGemSpeed = 3f;
    private IEnumerator CoMoveFieldGem()
    {
        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            Vector3 _dir = target.gemDest.position - transform.position;
            fieldGem.transform.Translate(_dir * fieldGemSpeed * Time.deltaTime);

            if (Vector3.Distance(fieldGem.transform.position, target.gemDest.position) < 0.5f)
                fieldGem.SetActive(false);

            yield return null;
        }
    }
}
