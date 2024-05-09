using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSystem : SystemPanel
{
    public static EquipmentSystem instance;

    public List<Rig> rigList;
    public List<int> ownRigNum;

    [Header("Button")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button overallButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button gachaButton;
    [SerializeField] private Button synthesisButton;
    [Header("Panel")]
    [SerializeField] private GameObject overallPanel;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private GameObject gachaPanel;
    [SerializeField] private GameObject synthesisPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject synthesisItemPanel;
    [SerializeField] private GameObject rigSpecPanel;
    [Header("Image")]
    [SerializeField] private Image armorImage;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image itemArmorImage;
    [SerializeField] private Image itemWeaponImage;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI overallEffectText;
    [SerializeField] private TextMeshProUGUI itemArmorText;
    [SerializeField] private TextMeshProUGUI itemWeaponText;
    [Header("Sprite")]
    [SerializeField] private Sprite[] panelSprites;
    public Sprite[] itemSprites;
    [Header("Prefab")]
    [SerializeField] private GameObject rigSlotPrefab;
    [SerializeField] private GameObject syntSlotPrefab;

    private List<RigSlot> rigSlotList = new List<RigSlot>();
    private List<SyntSlot> syntSlotList = new List<SyntSlot>();

    public Rig curArmor { get; private set; } = null;
    public Rig curWeapon { get; private set; } = null;

    public int initArmorInd = -1;
    public int initWeaponInd = -1;

    private float armorEffectNum;
    private float weaponEffectNum;

    void Awake()
    {
        instance = this;
        ownRigNum = new List<int>();
    }
    void Start()
    {
        rigList = GSSManager.instance.rigData;
        Init();
        InitCurRig();
        AddButtonEvent();
    }
    private void Init()
    {
        for (int i=0; i<rigList.Count; i++)
        {
            GameObject _go1 = Instantiate(rigSlotPrefab, inventoryPanel.transform);
            GameObject _go2 = Instantiate(syntSlotPrefab, synthesisItemPanel.transform);

            if(ownRigNum.Count< i + 1)
                ownRigNum.Add(0);
            _go1.GetComponent<RigSlot>().SetSlot(rigList[i], ownRigNum[i]);
            _go2.GetComponent<SyntSlot>().SetSyntSlot(rigList[i], ownRigNum[i]);

            rigSlotList.Add(_go1.GetComponent<RigSlot>());
            syntSlotList.Add(_go2.GetComponent<SyntSlot>());
        }
    }
    private void InitCurRig()
    {
        if (initArmorInd >= 0)
        {
            curArmor = rigList[initArmorInd];
            itemArmorText.text = curArmor.name;
            ApplyEffect(curArmor);
        } 
        if (initWeaponInd >= 0)
        {
            curWeapon = rigList[initWeaponInd];
            itemWeaponText.text = curWeapon.name;
            ApplyEffect(curWeapon);
        }
    }
    private void AddButtonEvent()
    {
        closeButton.onClick.AddListener(ClosePanel);

        overallButton.onClick.AddListener(() => ShiftPanel(0));
        overallButton.onClick.AddListener(SetEffectText);

        itemButton.onClick.AddListener(() => ShiftPanel(1));

        gachaButton.onClick.AddListener(() => ShiftPanel(2));
        gachaButton.onClick.AddListener(() => gachaPanel.GetComponent<RigGacha>().SetTicketText());

        synthesisButton.onClick.AddListener(() => ShiftPanel(3));
        synthesisButton.onClick.AddListener(() => synthesisPanel.GetComponent<RigSynt>().ResetRig());
    }
    protected override void ClosePanel()
    {
        base.ClosePanel();
        DBManager.instance.SaveUserInfo();
    }
    private void ShiftPanel(int _ind)
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");

        if(GetComponentInChildren<RigGacha>() != null)
            GetComponentInChildren<RigGacha>().ResetResult();

        switch (_ind)
        {
            case 0:
                overallPanel.SetActive(true);
                itemPanel.SetActive(false);
                gachaPanel.SetActive(false);
                synthesisPanel.SetActive(false);
                break;
            case 1:
                overallPanel.SetActive(false);
                itemPanel.SetActive(true);
                gachaPanel.SetActive(false);
                synthesisPanel.SetActive(false);
                break;
            case 2:
                overallPanel.SetActive(false);
                itemPanel.SetActive(false);
                gachaPanel.SetActive(true);
                synthesisPanel.SetActive(false);
                break;
            case 3:
                overallPanel.SetActive(false);
                itemPanel.SetActive(false);
                gachaPanel.SetActive(false);
                synthesisPanel.SetActive(true);
                break;
        }
        GetComponent<Image>().sprite = panelSprites[_ind];
    }
    public void EquipRig(Rig _rig)
    {
        if(_rig.type == Rig.Type.Armor)
        {
            if(curArmor != null)
                RemoveEffect(curArmor);

            curArmor = _rig;
            ApplyEffect(curArmor);

            armorImage.sprite = itemSprites[curArmor.index];
            itemArmorImage.sprite = itemSprites[curArmor.index];

            itemArmorText.text = curArmor.name;
        }
        else if(_rig.type == Rig.Type.Weapon)
        {
            if(curWeapon != null)
                RemoveEffect(curWeapon);
            
            curWeapon = _rig;
            ApplyEffect(curWeapon);

            weaponImage.sprite = itemSprites[curWeapon.index];
            itemWeaponImage.sprite = itemSprites[curWeapon.index];

            itemWeaponText.text = curWeapon.name;
        }
    }
    private void ApplyEffect(Rig _rig)
    {
        switch (_rig.effectType)
        {
            case Rig.EffectType.AttackBuff:
                if(_rig.type == Rig.Type.Armor)
                    armorEffectNum = _rig.effectProb;
                else
                    weaponEffectNum = _rig.effectProb;
                GameManager.instance.player.extraAttack += _rig.effectProb;
                break;
            case Rig.EffectType.SkillCool:
                if (_rig.type == Rig.Type.Armor)
                    armorEffectNum = 1;
                else
                    weaponEffectNum = 1;
                GameManager.instance.player.maxAttackCount--;
                break;
            case Rig.EffectType.AttackDist:
                if (_rig.type == Rig.Type.Armor)
                    armorEffectNum = GameManager.instance.player.attackDist * (float)_rig.effectProb / 100;
                else
                    weaponEffectNum = GameManager.instance.player.attackDist * (float)_rig.effectProb / 100;
                GameManager.instance.player.attackDist += GameManager.instance.player.attackDist * (float)_rig.effectProb / 100;
                break;
            case Rig.EffectType.Defense:
                if (_rig.type == Rig.Type.Armor)
                    armorEffectNum = (float)_rig.effectProb / 100;
                else
                    weaponEffectNum = (float)_rig.effectProb / 100;
                GameManager.instance.player.defense += (float)_rig.effectProb / 100;
                break;
            case Rig.EffectType.Speed:
                if (_rig.type == Rig.Type.Armor)
                    armorEffectNum = GameManager.instance.player.stat.speed * ((float)_rig.effectProb / 100);
                else
                    weaponEffectNum = GameManager.instance.player.stat.speed * ((float)_rig.effectProb / 100);
                GameManager.instance.player.extraSpeed += GameManager.instance.player.stat.speed * ((float)_rig.effectProb / 100);
                break;
            case Rig.EffectType.Gold:
                if (_rig.type == Rig.Type.Armor)
                    armorEffectNum = (float)_rig.effectProb / 100;
                else
                    weaponEffectNum = (float)_rig.effectProb / 100;
                GameManager.instance.player.goldBonus += (float)_rig.effectProb / 100;
                break;
        }
    }
    private void RemoveEffect(Rig _rig)
    {
        switch (_rig.effectType)
        {
            case Rig.EffectType.AttackBuff:
                if (_rig.type == Rig.Type.Armor)
                    GameManager.instance.player.extraAttack -= armorEffectNum;
                else
                    GameManager.instance.player.extraAttack -= weaponEffectNum;
                break;
            case Rig.EffectType.SkillCool:
                GameManager.instance.player.maxAttackCount++;
                break;
            case Rig.EffectType.AttackDist:
                if (_rig.type == Rig.Type.Armor)
                    GameManager.instance.player.attackDist -= armorEffectNum;
                else
                    GameManager.instance.player.attackDist -= weaponEffectNum;                
                break;
            case Rig.EffectType.Defense:
                if (_rig.type == Rig.Type.Armor)
                    GameManager.instance.player.defense -= armorEffectNum;
                else
                    GameManager.instance.player.defense -= weaponEffectNum;                
                break;
            case Rig.EffectType.Speed:
                if (_rig.type == Rig.Type.Armor)
                    GameManager.instance.player.extraSpeed -= armorEffectNum;
                else
                    GameManager.instance.player.extraSpeed -= weaponEffectNum;                
                break;
            case Rig.EffectType.Gold:
                if (_rig.type == Rig.Type.Armor)
                    GameManager.instance.player.goldBonus -= armorEffectNum;
                else
                    GameManager.instance.player.goldBonus -= weaponEffectNum;
                break;
        }
    }
    public void OpenRigSpec(Rig _rig)
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        rigSpecPanel.GetComponent<Animator>().SetTrigger("On");
        rigSpecPanel.GetComponent<RigSpec>().SetSpec(_rig);
    }
    public void SetImageSprite()
    {
        if (curArmor != null)
        {
            armorImage.sprite = itemSprites[curArmor.index];
            itemArmorImage.sprite = itemSprites[curArmor.index];
        }
        else
        {
            armorImage.sprite = UIManager.instance.nullSprite;
            itemArmorImage.sprite = UIManager.instance.nullSprite;
        }
        if (curWeapon != null)
        {
            weaponImage.sprite = itemSprites[curWeapon.index];
            itemWeaponImage.sprite = itemSprites[curWeapon.index];
        }
        else
        {
            weaponImage.sprite = UIManager.instance.nullSprite;
            itemWeaponImage.sprite = UIManager.instance.nullSprite;
        }
    }
    public void SetEffectText()
    {
        StringBuilder _temp = new StringBuilder();
        _temp.Append("[방어구] ");
        if (curArmor != null)
            switch (curArmor.effectType)
            {
                case Rig.EffectType.AttackBuff:
                    _temp.Append($"공격력 {curArmor.effectProb} 증가");
                    break;
                case Rig.EffectType.SkillCool:
                    _temp.Append($"스킬 쿨타임 감소");
                    break;
                case Rig.EffectType.AttackDist:
                    _temp.Append($"공격 범위 {curArmor.effectProb}% 증가");
                    break;
                case Rig.EffectType.Defense:
                    _temp.Append($"입은 피해 {curArmor.effectProb}% 감소");
                    break;
                case Rig.EffectType.Speed:
                    _temp.Append($"이동 속도 {curArmor.effectProb}% 증가");
                    break;
                case Rig.EffectType.Gold:
                    _temp.Append($"골드 획득량 {curArmor.effectProb}% 증가");
                    break;
            }
        _temp.Append("\n[무기] ");
        if (curWeapon != null)
            switch (curWeapon.effectType)
            {
                case Rig.EffectType.AttackBuff:
                    _temp.Append($"공격력 {curWeapon.effectProb} 증가");
                    break;
                case Rig.EffectType.SkillCool:
                    _temp.Append($"스킬 쿨타임 감소");
                    break;
                case Rig.EffectType.AttackDist:
                    _temp.Append($"공격 범위 {curWeapon.effectProb}% 증가");
                    break;
                case Rig.EffectType.Defense:
                    _temp.Append($"입은 피해 {curWeapon.effectProb}% 감소");
                    break;
                case Rig.EffectType.Speed:
                    _temp.Append($"이동 속도 {curWeapon.effectProb}% 증가");
                    break;
                case Rig.EffectType.Gold:
                    _temp.Append($"골드 획득량 {curWeapon.effectProb}% 증가");
                    break;
            }

        overallEffectText.text = _temp.ToString();
    }
    public void UpdateRigSlot()
    {
        for (int i = 0; i < rigSlotList.Count; i++)
        {
            rigSlotList[i].SetNum(ownRigNum[i]);
            syntSlotList[i].SetNum(ownRigNum[i]);
        }
    }
    public void SetSacRig(Rig _rig)
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GetComponentInChildren<RigSynt>().SetSacrifice(_rig);
    }
}
