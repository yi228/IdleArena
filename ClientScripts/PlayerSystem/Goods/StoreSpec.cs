using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSpec : MonoBehaviour
{
    private GoodsSystem.GoodsType itemType;
    private int quantity = 1;
    private GoodsSystem.GoodsType paymentType;
    private int price;
    private int appliedPrice;

    [SerializeField] private Image itemImage;
    [SerializeField] private Image paymentImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI paymentText;
    [SerializeField] private TMP_InputField quantityInput;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button closeButton;
    //0: Gold, 1: Diamond, 2: HeroTicket, 3: EquipmentTicket
    [SerializeField] private Sprite[] goodsSprite;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePanel);
        quantityInput.onValueChanged.AddListener(delegate { ChangeQuantity(); });
        quantityInput.text = quantity.ToString();
    }
    private void ClosePanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GetComponent<Animator>().SetTrigger("Off");
    }
    private void ChangeQuantity()
    {
        quantity = int.Parse(quantityInput.text);
        appliedPrice = quantity * price;
        
        while(appliedPrice > GoodsSystem.instance.GetGoodsValue(paymentType))
        {
            quantity--;
            appliedPrice = quantity * price;
        }
        quantityInput.text = quantity.ToString();
        paymentText.text = appliedPrice.ToString();

        GoodsSystem.instance.SetConfirmPanelNum(quantity);
    }
    public void SetSpec(GoodsSystem.GoodsType _type)
    {
        itemType = _type;

        switch(_type)
        {
            case GoodsSystem.GoodsType.Gold:
                itemImage.sprite = goodsSprite[0];
                paymentImage.sprite = goodsSprite[1];
                itemText.text = "100 골드";
                paymentText.text = "10";

                paymentType = GoodsSystem.GoodsType.Diamond;
                price = 10;
                break;
            case GoodsSystem.GoodsType.HeroTicket:
                itemImage.sprite = goodsSprite[2];
                paymentImage.sprite = goodsSprite[1];
                itemText.text = "영웅소환권 1장";
                paymentText.text = "300";

                paymentType = GoodsSystem.GoodsType.Diamond;
                price = 300;
                break;
            case GoodsSystem.GoodsType.EquipmentTicket:
                itemImage.sprite = goodsSprite[3];
                paymentImage.sprite = goodsSprite[0];
                itemText.text = "장비소환권 1장";
                paymentText.text = "1000";

                paymentType = GoodsSystem.GoodsType.Gold;
                price = 1000;
                break;
        }
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK"));
        buyButton.onClick.AddListener(() => GoodsSystem.instance.OpenConfirmPanel(BuyItem, (int)itemType));
        //buyButton.onClick.AddListener((BuyItem));
    }
    private void BuyItem()
    {
        if(GoodsSystem.instance.UseGoods(paymentType, appliedPrice))
        {
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_BUY");
            GoodsSystem.instance.GetGoods(itemType, 
                itemType == GoodsSystem.GoodsType.Gold ? quantity * 100 : quantity);
            quantityInput.text = "1";
            quantity = 1;
            appliedPrice = price;
        }
    }
}
