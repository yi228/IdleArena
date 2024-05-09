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
        rigNumText.text = $"���� ����: {EquipmentSystem.instance.ownRigNum[rig.index]}";

        StringBuilder _temp = new StringBuilder();
        _temp.Append(rig.description);
        switch (rig.type)
        {
            case Rig.Type.Armor:
                _temp.Append("\n\n[��] ");
                break;
            case Rig.Type.Weapon:
                _temp.Append("\n\n[����] ");
                break;
        }
        switch (rig.effectType)
        {
            case Rig.EffectType.AttackBuff:
                _temp.Append($"���ݷ� {rig.effectProb} ����");
                break;
            case Rig.EffectType.SkillCool:
                _temp.Append($"��ų ��Ÿ�� ����");
                break;
            case Rig.EffectType.AttackDist:
                _temp.Append($"���� ���� {rig.effectProb}% ����");
                break;
            case Rig.EffectType.Defense:
                _temp.Append($"���� ���� {rig.effectProb}% ����");
                break;
            case Rig.EffectType.Speed:
                _temp.Append($"�̵� �ӵ� {rig.effectProb}% ����");
                break;
            case Rig.EffectType.Gold:
                _temp.Append($"��� ȹ�淮 {rig.effectProb}% ����");
                break;
        }
        descriptionText.text = _temp.ToString();

        rigImage.sprite = EquipmentSystem.instance.itemSprites[rig.index];
    }
}
