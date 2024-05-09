using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
	private PlayerController target;
	private GameObject cam;

    private int attackCount = 0;
	private int maxAttackCount = 0;

	protected override void Init()
	{
		base.Init();
		cam = GameObject.Find("MyPlayerCamera");
		maxAttackCount = ArenaManager.instance.maxAttackCount;
    }
	protected override void UpdateController()
	{
		if (PlayerSensor.PlayerReady && !Managers.Match.matchOver)
		{
			switch (State)
            {
                case CreatureState.Idle:
                    if (target == null)
                        FindTarget();
                    CheckDistance();
                    break;
                case CreatureState.Moving:
                    SetDir();
                    CheckDistance();
                    break;
            }

            base.UpdateController();
        }

		if(Managers.Match.matchOver && Stat.Hp > 0)
			curCharacter.DanceAnimation();

		if(Managers.Match.matchOver && Stat.Hp <= 0)
			curCharacter.DeadAnimation();
	}
	private bool FindTarget()
	{
		PlayerController[] playerArray = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

		foreach (PlayerController p in playerArray)
		{
			if (p != GetComponent<PlayerController>())
				target = p;
		}

		if(target == null)
			return false;
		else
		{
			SetDir();
            return true;
        }
	}
	protected override void UpdateIdle()
	{
		// 이동 상태로 갈지 확인
		if (FindTarget())
		{
			if (Vector2.Distance(target.transform.position, transform.position) <= 1.5)
				return;
			State = CreatureState.Moving;
			return;
		}
	}

	Coroutine _coSkillCooltime;
	IEnumerator CoInputCooltime(float time)
	{
		yield return new WaitForSeconds(time);
		_coSkillCooltime = null;
	}
	void LateUpdate()
	{
		//if(target != null)
		//	texts[0].text = Vector2.Distance(target.transform.position, transform.position).ToString();
  //      texts[1].text = State.ToString();
  //      texts[2].text = PlayerSensor.PlayerReady.ToString();

		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
		cam.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, -10);
    }
	private void SetDir()
	{
		Vector2 dir = target.transform.position - transform.position;

		if(dir.x < 0)
            Dir = MoveDir.Left;
		else
			Dir = MoveDir.Right;
    }
	protected override void MoveToNextPos()
	{
		if (target == null)
		{
			State = CreatureState.Idle;
			CheckUpdatedFlag();
			return;
		}

		Vector3Int destPos = CellPos;

		switch (Dir)
		{
			case MoveDir.Up:
				destPos += Vector3Int.up;
				break;
			case MoveDir.Down:
				destPos += Vector3Int.down;
				break;
			case MoveDir.Left:
				destPos += Vector3Int.left;
				break;
			case MoveDir.Right:
				destPos += Vector3Int.right;
				break;
		}

		if (Managers.Map.CanGo(destPos))
		{
			if (Managers.Object.FindCreature(destPos) == null)
			{
				CellPos = destPos;
			}
		}

		CheckUpdatedFlag();
	}
	protected override void CheckUpdatedFlag()
	{
		if (_updated)
		{
			C_Move movePacket = new C_Move();
			movePacket.PosInfo = PosInfo;
			Managers.Network.Send(movePacket);
			_updated = false;
		}
	}
	private void CheckDistance()
	{
		if (target == null)
			return;

		float dist = Vector2.Distance(target.transform.position, transform.position);
		if (dist <= 1.5 && _coSkillCooltime == null)
		{
			State = CreatureState.Idle;
			CheckUpdatedFlag();

            C_Skill skill = new C_Skill() { Info = new SkillInfo() };
			if(attackCount < maxAttackCount || Stat.SkillInd == -1)
			{
                attackCount++;
                skill.Info.SkillId = 8;
				skill.Info.Multiple = 1;
            }
			else
			{
				attackCount = 0;
                skill.Info.SkillId = Stat.SkillInd + 1;
                skill.Info.Multiple = Mathf.CeilToInt(ArenaManager.instance.skillList[Stat.SkillInd].GetComponent<Skill>().attackMultiple);
            }
            Managers.Network.Send(skill);

            _coSkillCooltime = StartCoroutine("CoInputCooltime", 0.2f);
        }
	}
}
