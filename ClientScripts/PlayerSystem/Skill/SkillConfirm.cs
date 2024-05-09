using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillConfirm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    void Start()
    {
        cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
        cancelButton.onClick.AddListener(() => SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK"));
    }
    public void SetConfirmPanel(UnityAction _event, Skill _skill, bool _have)
    {
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(_event);
        confirmButton.onClick.AddListener(() => gameObject.SetActive(false));

        if (!_have)
        {
            mainText.text = $"{_skill.skillName}��(��) ���ðڽ��ϱ�?";
            confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";
        }
        else
        {
            mainText.text = $"{_skill.skillName}��(��) �����Ͻðڽ��ϱ�?";
            confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "�����ϱ�";
        }
    }
}
