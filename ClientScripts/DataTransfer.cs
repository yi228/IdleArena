using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DBManager;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DataTransfer : MonoBehaviour
{
    public PlayerData playerData;

    void Awake()
    {
        playerData = new PlayerData();
    }
    public void ApplyData()
    {
        if(playerData != null)
        {
            //스탯 적용
            GameManager.instance.player.stat.hp = playerData.hp;
            GameManager.instance.player.stat.maxHp = playerData.maxHp;
            GameManager.instance.player.stat.exp = playerData.exp;
            GameManager.instance.player.stat.maxExp = playerData.maxExp;
            GameManager.instance.player.stat.attack = playerData.attack;
            GameManager.instance.player.stat.attackCool = playerData.attackCool;
            GameManager.instance.player.stat.speed = playerData.speed;
            //레벨 적용
            GameManager.instance.playerLevel = playerData.playerLevel;
            GameManager.instance.stageLevel = playerData.stageLevel;
            GameManager.instance.killCount = playerData.killCount;
            GameManager.instance.statUpPoint = playerData.statUpPoint;
            //보석 적용
            for (int i = 0; i < playerData.gemType.Count; i++)
            {
                GameObject _go = Instantiate(GameManager.instance.gemPrefab, GameManager.instance.gemStorePos);
                _go.GetComponent<GemController>().LoadGem(playerData.gemType[i], playerData.gemProb[i]);
                GemSystem.instance.gemList.Add(_go.GetComponent<GemController>());
            }
            if (playerData.curGemProb != 0)
            {
                GameObject _curGem = Instantiate(GameManager.instance.gemPrefab, GameManager.instance.gemStorePos);
                _curGem.GetComponent<GemController>().LoadGem(playerData.curGemType, playerData.curGemProb);
                GemSystem.instance.SetGem(_curGem.GetComponent<GemController>());
            }
            //영웅 적용
            GameManager.instance.curHeroInd = playerData.curHeroInd;
            HeroSystem.instance.curHero = HeroSystem.instance.heroList[playerData.curHeroInd];
            //HeroSystem.instance.GetComponentInChildren<HeroGacha>().gachaTicket = playerData.gachaTicket;
            for (int i = 0; i < HeroSystem.instance.heroList.Count; i++)
            {
                if (playerData.ownHeroIndex.Contains(i) || i == 0)
                    HeroSystem.instance.heroList[i].own = true;
                else
                    HeroSystem.instance.heroList[i].own = false;
            }
            //메일 적용
            if (playerData.seenMailIndex == null)
                MailSystem.instance.seenMailIndex = new List<int>();
            else
                MailSystem.instance.seenMailIndex = playerData.seenMailIndex;
            if(playerData.receivedMailIndex == null)
                MailSystem.instance.receivedMailIndex = new List<int>();
            else
                MailSystem.instance.receivedMailIndex = playerData.receivedMailIndex;
            if(playerData.deletedMailIndex == null)
                MailSystem.instance.deletedMailIndex = new List<int>();
            else
                MailSystem.instance.deletedMailIndex = playerData.deletedMailIndex;
            //재화 적용
            GoodsSystem.instance.GetGoods(GoodsSystem.GoodsType.Gold, playerData.gold);
            GoodsSystem.instance.GetGoods(GoodsSystem.GoodsType.Diamond, playerData.diamond);
            GoodsSystem.instance.GetGoods(GoodsSystem.GoodsType.HeroTicket, playerData.heroTicket);
            GoodsSystem.instance.GetGoods(GoodsSystem.GoodsType.EquipmentTicket, playerData.equipmentTicket);
            GoodsSystem.instance.usedGold = playerData.usedGold;
            //퀘스트 적용
            QuestSystem.instance.stageInd = playerData.stageQuestInd;
            QuestSystem.instance.levelInd = playerData.levelQuestInd;
            QuestSystem.instance.goldInd = playerData.goldQuestInd;
            //스킬 적용
            SkillManager.instance.SetCurrentSkill(playerData.curSkillInd);
            SkillManager.instance.ownSkillIndex = playerData.skillIndex;
            //장비 적용
            EquipmentSystem.instance.ownRigNum = playerData.rigIndex;
            EquipmentSystem.instance.initArmorInd = playerData.curArmor;
            EquipmentSystem.instance.initWeaponInd = playerData.curWeapon;
            //아레나 적용
            ArenaSystem.instance.score = playerData.arenaScore;
        }
        else
        {
            //스탯 적용
            GameManager.instance.player.stat.hp = 100;
            GameManager.instance.player.stat.maxHp = 100;
            GameManager.instance.player.stat.exp = 0;
            GameManager.instance.player.stat.maxExp = GSSManager.instance.playerMaxExp[0];
            GameManager.instance.player.stat.attack = 10;
            GameManager.instance.player.stat.attackCool = 1;
            GameManager.instance.player.stat.speed = 7.5f;
            //레벨 적용
            GameManager.instance.playerLevel = 0;
            GameManager.instance.stageLevel = 0;
            GameManager.instance.killCount = 0;
            GameManager.instance.statUpPoint = 0;
            //영웅 적용
            HeroSystem.instance.curHero = HeroSystem.instance.heroList[0];
            for (int i = 0; i < HeroSystem.instance.heroList.Count; i++)
            {
                if (i == 0)
                    HeroSystem.instance.heroList[i].own = true;
                else
                    HeroSystem.instance.heroList[i].own = false;
            }
            //메일 적용
            MailSystem.instance.seenMailIndex = new List<int>();
            MailSystem.instance.receivedMailIndex = new List<int>();
            MailSystem.instance.deletedMailIndex = new List<int>();
            //재화 적용
            GoodsSystem.instance.usedGold = 0;
            //아레나 적용
            ArenaSystem.instance.score = 0;
        }
    }
}
