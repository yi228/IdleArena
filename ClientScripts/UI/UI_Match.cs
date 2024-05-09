using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Match : MonoBehaviour
{
    [SerializeField] private GameObject matchPanel;
    [SerializeField] private GameObject ingamePanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI matchText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private Button cancelButton;

    public Slider MyPlayerHpBar;
    public Slider EnemyPlayerHpBar;

    private UI_MatchingStatus matchStatus;
    private Coroutine coTip;
    private string[] tips =
    {
        "아레나에서는 다른 용사님들과의 우열을 가릴 수 있습니다.",
        "보석은 아레나에서 적용되지 않습니다.",
        "용사님의 순수 능력으로 승부를 가려보세요!"
    };

    void Start()
    {
        matchStatus = FindAnyObjectByType<UI_MatchingStatus>();
        cancelButton.onClick.AddListener(CancelMatch);
    }
    private void CancelMatch()
    {
        SoundManager.instance.PlayClip(GetComponent<AudioSource>(), "UI_CLICK");
        Managers.Object.Remove(Managers.Object.MyPlayerId);
        SceneManager.LoadScene("LoadingScene");
    }
    public void SetMatchPanel(bool active)
    {
        matchPanel.SetActive(active);
        if (active && coTip == null)
            coTip = StartCoroutine(CoCycleTips());
        else if (!active)
        {
            StopCoroutine(coTip);
            coTip = null;
        }
    }
    public void SetIngamePanel(bool active)
    {
        ingamePanel.SetActive(active);

        if (active)
        {
            ingamePanel.GetComponent<UI_InMatchPanel>().PlayNameAnimation();
            SoundManager.instance.PlayClip(GetComponent<AudioSource>(), "AR_MATCH");
        }
    }
    public void SetResultPanel(bool win)
    {
        resultPanel.SetActive(true);

        UI_ArenaResult _result = resultPanel.GetComponent<UI_ArenaResult>();
        if (win)
            _result.Win();
        else
            _result.Lose();
    }
    public void SetMatchText(string text)
    {
        matchText.text = text;
    }
    public void SetMatchStatusText(string text)
    {
        matchStatus.t.text = text;
    }
    public void VsOff()
    {
        ingamePanel.GetComponent<UI_InMatchPanel>().VsOff();
    }
    private IEnumerator CoCycleTips()
    {
        int ind = 0;

        while (matchPanel.gameObject.activeInHierarchy)
        {
            tipText.text = tips[ind++];
            if (ind == 3)
                ind = 0;
            yield return new WaitForSeconds(3f);
        }
    }
}
