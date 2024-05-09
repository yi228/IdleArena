using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class PlayerController : CreatureController
{
	private Slider _hpBar;

    public override StatInfo Stat
    {
        get { return base.Stat; }
        set 
		{ 
			base.Stat = value;
			UpdateHpBar();
        }
    }
    public override int Hp
    {
        get { return Stat.Hp; }
        set
        {
			//InstantiateDamageText(Mathf.Abs(value - base.Hp));
            base.Hp = value;
            UpdateHpBar();
        }
    }

    protected Coroutine _coSkill;
	protected bool _rangedSkill = false;

    protected CharacterMultiController curCharacter;
    private CharacterMultiController[] charList;

	protected AudioSource audioSource;
	protected AudioSource charAudioSource;


    [SerializeField] private List<Transform> textEffectPos;
    [SerializeField] private List<Transform> effectPos;

    [SerializeField] private GameObject damageTextEffect;
    [SerializeField] private GameObject skillTextEffect;

	private void FindHpBar()
	{
        UI_Match matchUi = FindAnyObjectByType<UI_Match>();
        _hpBar = GetComponent<MyPlayerController>() != null ?
            matchUi.MyPlayerHpBar :
            matchUi.EnemyPlayerHpBar;

        _hpBar.value = 1;
    }
    private void UpdateHpBar()
	{
		if (_hpBar == null)
			return;
        
		float ratio = 0.0f;
		if (Stat.MaxHp > 0)
			ratio = ((float)Hp) / Stat.MaxHp;

        _hpBar.value = ratio;
	}
	protected override void Init()
	{
		base.Init();
		FindHpBar();
        audioSource = GetComponent<AudioSource>();
	}
	public void SetCharacter(int ind)
	{
		charList = GetComponentsInChildren<CharacterMultiController>();
		curCharacter = charList[ind];
		curCharacter.playerController = this;
        for (int i = 0; i < charList.Length; i++)
            if (i != ind)
                charList[i].gameObject.SetActive(false);

		charAudioSource = curCharacter.GetComponent<AudioSource>();
    }
	protected override void UpdateAnimation()
	{
		if (curCharacter == null)
			return;

        switch (Dir)
        {
            case MoveDir.Left:
                curCharacter.ChangeSide(true);
                break;
            case MoveDir.Right:
                curCharacter.ChangeSide(false);
                break;
        }

        if (State == CreatureState.Idle)
            curCharacter.IdleAnimation();
		else if (State == CreatureState.Moving)
			curCharacter.MoveAnimation();
        else
		{
		}
	}

	protected override void UpdateController()
	{		
		base.UpdateController();
	}

	public override void UseSkill(int skillId)
	{
		switch (skillId)
		{
			case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                _coSkill = StartCoroutine(CoSkillAttack(skillId - 1));
                break;
            case 8:
                _coSkill = StartCoroutine(CoNormalAttack());
                break;
        }
	}
	protected virtual void CheckUpdatedFlag()
	{

	}
	IEnumerator CoNormalAttack()
	{
		// 대기 시간
		_rangedSkill = false;
		State = CreatureState.Skill;
        curCharacter.AttackAnimation(true);
		InstantiateAttackEffect();
        SoundManager.instance.PlayClip(audioSource, "ATTACK");
        yield return new WaitForSeconds(Stat.AttackCool);
		State = CreatureState.Idle;
		_coSkill = null;
		CheckUpdatedFlag();
	}
    IEnumerator CoSkillAttack(int ind)
    {
        // 대기 시간
        _rangedSkill = false;
        State = CreatureState.Skill;
        curCharacter.SkillAnimation();
        ArenaManager.instance.UseSkill(ind, audioSource, Dir == MoveDir.Left ? 
            effectPos[0].position : effectPos[1].position, Dir == MoveDir.Left ? "L" : "R");
        yield return new WaitForSeconds(Stat.AttackCool);
        State = CreatureState.Idle;
        _coSkill = null;
        CheckUpdatedFlag();
    }
	public override void OnDamaged()
	{
		Debug.Log("Player HIT !");
        curCharacter.DamageAnimation();
        SoundManager.instance.PlayClip(charAudioSource, "HIT");
    }
    public override void OnDead()
    {
        base.OnDead();
        SoundManager.instance.PlayClip(audioSource, "DIE");
        curCharacter.DeadAnimation();

		if (GetComponent<MyPlayerController>() != null)
			FindAnyObjectByType<UI_Match>().SetResultPanel(false);
		else
            FindAnyObjectByType<UI_Match>().SetResultPanel(true);

        Managers.Match.matchOver = true;
    }
    public void InstantiateDamageText(float _damage, bool _skill = false)
    {
        GameObject _go = Instantiate(_skill ? skillTextEffect : damageTextEffect);
        _go.transform.position = Dir == MoveDir.Left ? textEffectPos[0].position : textEffectPos[1].position;
        _go.GetComponent<UI_DamageText>().damage = _damage;
        Destroy(_go, 1f);
    }
    public void InstantiateAttackEffect()
    {
        EffectManager.instance.InstantiateEffect("NORMAL_ATTACK", Dir == MoveDir.Left ? effectPos[0].position : effectPos[1].position, Dir == MoveDir.Left ? "L" : "R", 1f);
    }
}
