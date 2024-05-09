using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GemController : MonoBehaviour
{
    public Sprite gemImage {  get; private set; }
    [SerializeField] private Sprite[] hpSprites;
    [SerializeField] private Sprite[] atSprites;
    [SerializeField] private Sprite[] spSprites;

    public enum GemType
    {
        HP,
        AT,
        SP
    }
    public GemType gemType { get; private set; }
    public PlayerIdleController player { get; private set; }
    public int effectProb { get; private set; }

    private float effectTick = 0f;

    private bool removed = false;

    void Start()
    {
        player = GameManager.instance.player;
    }
    void Update()
    {
        if(GemSystem.instance.curGem == this)
        {
            removed = false;

            effectTick += Time.deltaTime;
            if(effectTick >= 1f )
            {
                effectTick = 0f;
                int _probNum = Random.Range(0, 100);
                if(_probNum <= effectProb)
                {
                    switch (gemType)
                    {
                        case GemType.HP:
                            player.stat.hp += player.stat.maxHp * 0.1f;
                            if (player.stat.hp > player.stat.maxHp)
                                player.stat.hp = player.stat.maxHp;
                            break;
                        case GemType.AT:
                            player.gemAttack = player.stat.attack * 0.2f;
                            break;
                        case GemType.SP:
                            player.gemSpeed = player.stat.speed * 0.5f;
                            break;
                    }
                }
                else
                    switch (gemType)
                    {
                        case GemType.HP:
                            break;
                        case GemType.AT:
                            player.gemAttack  = 0;
                            break;
                        case GemType.SP:
                            player.gemSpeed = 0;
                            break;
                    }
            }
        }
        else if(!removed)
        {
            removed = true;
            switch (gemType)
            {
                case GemType.HP:
                    break;
                case GemType.AT:
                    player.gemAttack = 0;
                    break;
                case GemType.SP:
                    player.gemSpeed = 0;
                    break;
            }
        }
    }
    //생성될 때 호출
    public void InitGem()
    {
        int _ind = Random.Range(0, 3);
        int _level = Random.Range(0, 4);
        switch(_ind)
        {
            case 0:
                gemType = GemType.HP;
                gemImage = hpSprites[_level];
                break;
            case 1:
                gemType = GemType.AT;
                gemImage = atSprites[_level];
                break; 
            case 2:
                gemType = GemType.SP;
                gemImage = spSprites[_level];
                break;
        }
        switch (_level)
        {
            case 0:
                effectProb = Random.Range(10, 25);
                break;
            case 1:
                effectProb = Random.Range(35, 50);
                break;
            case 2:
                effectProb = Random.Range(60, 75);
                break;
            case 3:
                effectProb = Random.Range(85, 100);
                break;
        }
    }
    //데이터 로드할 때 호출
    public void LoadGem(int _type, int _prob)
    {
        gemType = (GemType)_type;
        effectProb = (int)_prob;

        int _level = 0;
        if (effectProb < 25)
            _level = 0;
        else if(effectProb < 50)
            _level = 1;
        else if(effectProb < 75)
            _level = 2;
        else
            _level = 3;

        switch (gemType)
        {
            case GemType.HP:
                gemImage = hpSprites[_level];
                break;
            case GemType.AT:
                gemImage = atSprites[_level];
                break; 
            case GemType.SP:
                gemImage = spSprites[_level];
                break;
        }
    }
}
