using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSystem : SystemPanel
{
    public static HeroSystem instance;

    [SerializeField] private GameObject gachaPanel;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button openGachaButton;
    [SerializeField] private Button closeGachaButton;
    [SerializeField] private Image curHeroImage;
    [SerializeField] private TextMeshProUGUI curHeroText;
    //0:기본 1:암살자 2:비스트 3:탐험가 4:근위대 5:로마군 6:검투사 7:사무라이
    public List<Hero> heroList = new List<Hero>();
    [SerializeField] private List<HeroSlot> slotList = new List<HeroSlot>();

    public Hero curHero;
    public HeroSlot selectedSlot;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Init();
        AddButtonListener();
    }
    private void Init()
    {
         for(int i=0; i<heroList.Count; i++)
        {
            slotList[i].hero = heroList[i];
            slotList[i].index = i;
        }
    }
    private void AddButtonListener()
    {
        equipButton.onClick.AddListener(EquipHero);
        closeButton.onClick.AddListener(ClosePanel);
        openGachaButton.onClick.AddListener(OpenGachaPanel);
        closeGachaButton.onClick.AddListener(CloseGachaPanel);
    }
    private void EquipHero()
    {
        if (selectedSlot != null)
        {
            SetHero(selectedSlot.hero);
            GameManager.instance.player.ChangeHero(selectedSlot.index);
            GameManager.instance.curHeroInd = selectedSlot.index;

            //DBManager.instance.SaveUserInfo();
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GEM_EQUIP");
        }
    }
    private void SetHero(Hero _hero)
    {
        curHero = _hero;
        curHeroImage.sprite = _hero.image;
        curHeroText.text = _hero.name;
    }
    protected override void ClosePanel()
    {
        base.ClosePanel();
    }
    private void OpenGachaPanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        gachaPanel.GetComponent<Animator>().SetTrigger("On");
    }
    private void CloseGachaPanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        gachaPanel.GetComponent<Animator>().SetTrigger("Off");
        GetComponentInChildren<HeroGacha>().ResetResult();
    }
    public void UpdateOwnStatus()
    {
        SetHero(heroList[GameManager.instance.curHeroInd]);

        GetComponentInChildren<HeroGacha>().SetTicketText();

        for (int i = 0; i< slotList.Count; i++)
            slotList[i].GetComponent<Button>().interactable = slotList[i].hero.own;
    }
}
