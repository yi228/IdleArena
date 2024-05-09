using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSystem : SystemPanel
{
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI atSpeedText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button hpButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button atSpeedButton;

    void Start()
    {
        AddButtonListener();
        ApplyText();
    }
    private void AddButtonListener()
    {
        closeButton.onClick.AddListener(ClosePanel);
        hpButton.onClick.AddListener(IncreaseHp);
        attackButton.onClick.AddListener(IncreaseAttack);
        atSpeedButton.onClick.AddListener(DecreaseAttackCool);
    }
    public void ApplyText()
    {
        nameText.text = DBManager.instance.clientUserName;
        pointText.text = $"스탯 포인트: {GameManager.instance.statUpPoint}";
        hpText.text = $"최대 체력 {GameManager.instance.player.stat.maxHp}";
        attackText.text = $"공격력 {GameManager.instance.player.stat.attack}";

        float _truncAttackCool = Mathf.Floor(GameManager.instance.player.stat.attackCool * 100f) / 100f;
        atSpeedText.text = $"공격 속도 1회/{_truncAttackCool}초";
    }
    protected override void ClosePanel()
    {
        base.ClosePanel();
    }
    private void IncreaseHp()
    {
        if (GameManager.instance.statUpPoint >= 1)
        {
            GameManager.instance.statUpPoint--;
            GameManager.instance.player.stat.maxHp += 10f;
            GameManager.instance.player.stat.hp += 10f;
            ApplyText();
            DBManager.instance.SaveUserInfo();
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_STAT_SUCCESS");
            Debug.Log("Hp Up: " + GameManager.instance.player.stat.maxHp);
        }
        else
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_STAT_FAIL");
    }
    private void IncreaseAttack()
    {
        if (GameManager.instance.statUpPoint >= 1)
        {
            GameManager.instance.statUpPoint--;
            GameManager.instance.player.stat.attack += 3f; 
            ApplyText();
            DBManager.instance.SaveUserInfo();
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_STAT_SUCCESS");
            Debug.Log("Attack Up: " + GameManager.instance.player.stat.attack);
        }
        else
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_STAT_FAIL");
    }
    private void DecreaseAttackCool()
    {
        if (GameManager.instance.statUpPoint >= 1 && GameManager.instance.player.stat.attackCool >= 0.65f)
        {
            GameManager.instance.statUpPoint--;
            GameManager.instance.player.stat.attackCool -= 0.05f;
            ApplyText();
            DBManager.instance.SaveUserInfo();
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_STAT_SUCCESS");
            Debug.Log("AttackSpeed Up: " + GameManager.instance.player.stat.attackCool);
        }
        else
        {
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_STAT_FAIL"); 
            Debug.Log("AttackSpeed limited");
        } 
    }
}
