using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    private Skill skill;
    private int index;
    private GoodsSystem.GoodsType paymentType;

    private Button slotButton;
    private Image iconImage;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private GameObject goldImage;
    [SerializeField] private GameObject equipImage;

    void Update()
    {
        if(SkillManager.instance.curSkillInd == index)
            equipImage.SetActive(true);
        else 
            equipImage.SetActive(false);
    }
    public void Init(Skill _skill, int _index)
    {
        skill = _skill;
        index = _index;

        slotButton = GetComponent<Button>();

        iconImage = GetComponent<Image>();
        iconImage.sprite = skill.icon;
        
        nameText.text = skill.skillName;

        SetSlotStatus();
    }
    public void SetSlotStatus()
    {
        if (GameManager.instance.playerLevel < skill.unlockLevel - 1)
        {
            lockImage.SetActive(true);
            lockImage.GetComponentInChildren<TextMeshProUGUI>().text = $"Lv.{skill.unlockLevel}";

            goldImage.SetActive(false);

            slotButton.interactable = false;
        }
        else if (!SkillManager.instance.ownSkillIndex.Contains(index))
        {
            lockImage.SetActive(false);

            goldImage.SetActive(true);
            goldImage.GetComponentInChildren<TextMeshProUGUI>().text = skill.price.ToString();

            slotButton.interactable = true;
            slotButton.onClick.AddListener(() => SkillManager.instance.OpenConfirmPanel(BuySkill, skill, false));
        }
        else
        {
            lockImage.SetActive(false);
            goldImage.SetActive(false);

            slotButton.interactable = true;
            slotButton.onClick.AddListener(() => SkillManager.instance.OpenConfirmPanel(EquipSkill, skill, true));
        }
    }
    private void BuySkill()
    {
        if (GoodsSystem.instance.UseGoods(paymentType, skill.price))
        {
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_BUY");
            SkillManager.instance.ownSkillIndex.Add(index);
            SetSlotStatus();
        }
    }
    private void EquipSkill()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GEM_EQUIP");
        SkillManager.instance.SetCurrentSkill(index);
    }
}
