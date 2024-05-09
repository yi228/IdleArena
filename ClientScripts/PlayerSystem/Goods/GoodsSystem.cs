using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GoodsSystem : SystemPanel
{
    public static GoodsSystem instance;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI diaText;
    [SerializeField] private TextMeshProUGUI heroTicketText;
    [SerializeField] private TextMeshProUGUI equipmentTicketText;
    [Header ("Button")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button buyGoldButton;
    [SerializeField] private Button buyHeroTicketButton;
    [SerializeField] private Button buyEquipmentTicketButton;
    [SerializeField] private Button buyHeroButton;
    [Header("GameObject")]
    [SerializeField] private GameObject storeSpecPanel;
    [SerializeField] private GameObject buyHeroPanel;
    [SerializeField] private GameObject confirmPanel;

    public int usedGold;

    public enum GoodsType
    {
        Gold,
        Diamond,
        HeroTicket,
        EquipmentTicket
    }
    private Dictionary<GoodsType, int> goodsStatus = new Dictionary<GoodsType, int>()
    {
        {GoodsType.Gold, 0},
        {GoodsType.Diamond, 0},
        {GoodsType.HeroTicket, 0}, 
        {GoodsType.EquipmentTicket, 0}
    };

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        AddButtonEvent();
    }
    protected override void ClosePanel()
    {
        base.ClosePanel();
        DBManager.instance.SaveUserInfo();
    }
    private void AddButtonEvent()
    {
        closeButton.onClick.AddListener(ClosePanel);
        buyGoldButton.onClick.AddListener(() => OpenSpecPanel(GoodsType.Gold));
        buyHeroTicketButton.onClick.AddListener(() => OpenSpecPanel(GoodsType.HeroTicket));
        buyEquipmentTicketButton.onClick.AddListener(() => OpenSpecPanel(GoodsType.EquipmentTicket));
        buyHeroButton.onClick.AddListener(OpenHeroBuyPanel);
    }
    private void OpenSpecPanel(GoodsType _type)
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        storeSpecPanel.GetComponent<Animator>().SetTrigger("On");
        storeSpecPanel.GetComponent<StoreSpec>().SetSpec(_type);
    }
    private void OpenHeroBuyPanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        buyHeroPanel.GetComponent<Animator>().SetTrigger("On");
        buyHeroPanel.GetComponent<BuyHeroSpec>().InitButtons();
    }
    public void InitUI()
    {
        goldText.text = GetGoodsValue(GoodsType.Gold).ToString();
        diaText.text = GetGoodsValue(GoodsType.Diamond).ToString();
        heroTicketText.text = GetGoodsValue(GoodsType.HeroTicket).ToString();
        equipmentTicketText.text = GetGoodsValue(GoodsType.EquipmentTicket).ToString();
    }
    public bool UseGoods(GoodsType _type, int amount)
    {
        if (goodsStatus[_type] >= amount)
        {
            goodsStatus[_type] -= amount;
            if (_type == GoodsType.Gold)
                usedGold += amount;
            InitUI();
            return true;
        }
        else
            return false;
    }
    public void GetGoods(GoodsType _type, int amount)
    {
        goodsStatus[_type] += amount;
        InitUI();
    }
    public int GetGoodsValue(GoodsType _type)
    {
        return goodsStatus[_type];
    }
    public void OpenConfirmPanel(UnityAction _event, int _itemInd)
    {
        confirmPanel.SetActive(true);
        confirmPanel.GetComponent<BuyConfirm>().SetConfirmPanel(_event, _itemInd);
    }
    public void SetConfirmPanelNum(int _quantity)
    {
        confirmPanel.GetComponent<BuyConfirm>().quantity = _quantity;
    }
}
