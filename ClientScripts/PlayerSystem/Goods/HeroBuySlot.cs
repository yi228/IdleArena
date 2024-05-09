using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBuySlot : MonoBehaviour
{
    [SerializeField] private BuyHeroSpec panel;

    [SerializeField] private int heroInd;
    [SerializeField] private int diaPrice;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK"));
        button.onClick.AddListener(() => GoodsSystem.instance.OpenConfirmPanel(BuyHero, 4));
    }
    private void BuyHero()
    {
        if (GoodsSystem.instance.UseGoods(GoodsSystem.GoodsType.Diamond, diaPrice))
        {
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_BUY");
            HeroSystem.instance.heroList[heroInd].own = true;
            panel.InitButtons();
        }
    }
    public void SetButtonInteractable()
    {
        if (HeroSystem.instance.heroList[heroInd].own ||
            GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.Diamond) < diaPrice)
            button.interactable = false;
        else
            button.interactable = true;
    }
}
