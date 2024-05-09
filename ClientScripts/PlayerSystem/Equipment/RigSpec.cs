using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RigSpec : MonoBehaviour
{
    private Rig rig;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI rigNumText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Image rigImage;

    [SerializeField]private Button closeButton;
    [SerializeField]private Button equipButton;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePanel);
        equipButton.onClick.AddListener(EquipRig);
    }
    private void ClosePanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GetComponent<Animator>().SetTrigger("Off");
    }
    private void EquipRig()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GEM_EQUIP");
        EquipmentSystem.instance.EquipRig(rig);
        GetComponent<Animator>().SetTrigger("Off");
    }
    public void SetSpec(Rig _rig)
    {
        rig = _rig;

        titleText.text = rig.name;
        rigNumText.text = $"보유 개수: {EquipmentSystem.instance.ownRigNum[rig.index]}";

        StringBuilder _temp = new StringBuilder();
        _temp.Append(rig.description);
        switch (rig.type)
        {
            case Rig.Type.Armor:
                _temp.Append("\n\n[방어구] ");
                break;
            case Rig.Type.Weapon:
                _temp.Append("\n\n[무기] ");
                break;
        }
        switch (rig.effectType)
        {
            case Rig.EffectType.AttackBuff:
                _temp.Append($"공격력 {rig.effectProb} 증가");
                break;
            case Rig.EffectType.SkillCool:
                _temp.Append($"스킬 쿨타임 감소");
                break;
            case Rig.EffectType.AttackDist:
                _temp.Append($"공격 범위 {rig.effectProb}% 증가");
                break;
            case Rig.EffectType.Defense:
                _temp.Append($"입은 피해 {rig.effectProb}% 감소");
                break;
            case Rig.EffectType.Speed:
                _temp.Append($"이동 속도 {rig.effectProb}% 증가");
                break;
            case Rig.EffectType.Gold:
                _temp.Append($"골드 획득량 {rig.effectProb}% 증가");
                break;
        }
        descriptionText.text = _temp.ToString();

        rigImage.sprite = EquipmentSystem.instance.itemSprites[rig.index];
    }
}
