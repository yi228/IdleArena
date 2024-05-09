using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
	public MyPlayerController MyPlayer { get; set; }
	public int MyPlayerId { get; set; }
	Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
	
	public static GameObjectType GetObjectTypeById(int id)
	{
		int type = (id >> 24) & 0x7F;
		return (GameObjectType)type;
	}

	public void Add(ObjectInfo info, bool myPlayer = false)
	{
		GameObjectType objectType = GetObjectTypeById(info.ObjectId);
		if (objectType == GameObjectType.Player)
		{
            if (myPlayer)
			{
				GameObject go = Managers.Resource.Instantiate("Creatures/MultiPlayerSet");
				go.name = info.Name;
				_objects.Add(info.ObjectId, go);

				MyPlayer = go.GetComponent<MyPlayerController>();
				MyPlayer.Id = info.ObjectId;
                MyPlayerId = info.ObjectId;
                MyPlayer.PosInfo = info.PosInfo;
				MyPlayer.Stat.Hp = info.StatInfo.Hp;
				MyPlayer.Stat.MaxHp = info.StatInfo.MaxHp;
				MyPlayer.Stat.Attack = info.StatInfo.Attack;
				MyPlayer.Stat.AttackCool = info.StatInfo.AttackCool;
				MyPlayer.Stat.Speed = info.StatInfo.Speed;
				MyPlayer.Stat.SkillInd = info.StatInfo.SkillInd;
				MyPlayer.Stat.Score = info.StatInfo.Score;

				int maxLevInd = 2;
				int curLevInd = (GameManager.instance.playerLevel + 1) / 10;
                if (curLevInd > maxLevInd)
                    curLevInd = maxLevInd;
                MyPlayer.SetCharacter((GameManager.instance.curHeroInd * 3)
					+ curLevInd);
				MyPlayer.SyncPos();
			}
			else
			{
				GameObject go = Managers.Resource.Instantiate("Creatures/EnemyPlayerSet");
				Debug.Log(info.Name);
				go.name = info.Name;
				_objects.Add(info.ObjectId, go);

				PlayerController pc = go.GetComponent<PlayerController>();
				pc.Id = info.ObjectId;
				pc.PosInfo = info.PosInfo;
				pc.Stat = info.StatInfo;
				pc.Stat.Score = info.StatInfo.Score;

                int maxLevInd = 2;
                int curLevInd = (info.Level + 1) / 10;
                if (curLevInd > maxLevInd)
                    curLevInd = maxLevInd;
                pc.SetCharacter(curLevInd);
				pc.SyncPos();
			}
		}
		else if (objectType == GameObjectType.Monster)
		{
		}
		else if (objectType == GameObjectType.Projectile)
		{
		}
	}

	public void Remove(int id)
	{
		GameObject go = FindById(id);
		if (go == null)
			return;

		_objects.Remove(id);
		Managers.Resource.Destroy(go);
	}

	public GameObject FindById(int id)
	{
		GameObject go = null;
		_objects.TryGetValue(id, out go);
		return go;
	}

	public GameObject FindCreature(Vector3Int cellPos)
	{
		foreach (GameObject obj in _objects.Values)
		{
			CreatureController cc = obj.GetComponent<CreatureController>();
			if (cc == null)
				continue;

			if (cc.CellPos == cellPos)
				return obj;
		}

		return null;
	}

	public GameObject Find(Func<GameObject, bool> condition)
	{
		foreach (GameObject obj in _objects.Values)
		{
			if (condition.Invoke(obj))
				return obj;
		}

		return null;
	}

	public void Clear()
	{
		foreach (GameObject obj in _objects.Values)
			Managers.Resource.Destroy(obj);
		_objects.Clear();
		MyPlayer = null;
	}
}
