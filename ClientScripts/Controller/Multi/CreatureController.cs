using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class CreatureController : BaseController
{
	public override StatInfo Stat
	{
		get { return base.Stat; }
		set { base.Stat = value; }
	}

	public override int Hp
	{
		get { return Stat.Hp; }
		set {
			base.Hp = value;
		}
	}
	protected override void Init()
	{
		base.Init();
	}

	public virtual void OnDamaged()
	{

	}

	public virtual void OnDead()
	{
		State = CreatureState.Dead;

		//GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
		//effect.transform.position = transform.position;
		//effect.GetComponent<Animator>().Play("START");
		//Destroy(effect, 0.5f);

		//if (GetComponent<MyPlayerController>() != null)
		//	FindAnyObjectByType<UI_MatchingStatus>().t.text = "Lose";
		//else
		//	FindAnyObjectByType<UI_MatchingStatus>().t.text = "Win";
    }

    public virtual void UseSkill(int skillId)
	{

	}
}
