using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [SerializeField] private UI_Match matchUi;
    public static bool PlayerReady;

    private bool countDone;

    private PlayerController[] allPlayers;
    private Coroutine coLoading;

    void Start()
    {
        PlayerReady = false;
        countDone = false;
    }
    void Update()
    {
        if (!PlayerReady)
        {
            allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

            foreach (PlayerController p in allPlayers)
            {
                if (p.GetComponent<MyPlayerController>() != null)
                    Managers.Match.MyPlayer = p.GetComponent<MyPlayerController>();
                else
                    Managers.Match.Enemy = p;
            }

            if (!countDone && allPlayers.Length >= 2)
                StartCoroutine(CoCountdown());
            //��Ī �� UI ����
            else if(allPlayers.Length < 2)
            {
                matchUi.SetMatchPanel(true);
                if(coLoading == null)
                    coLoading = StartCoroutine(CoLoadingTextDot());
            }
        }
    }
    private IEnumerator CoCountdown()
    {
        countDone = true;
        ArenaManager.instance.scoreUpdated = false;
        //��Ī �Ϸ� UI ����
        matchUi.SetMatchText("    ��Ī �Ϸ�!");
        //���� �Ұ� UI ����
        yield return new WaitForSeconds(0.5f);
        matchUi.SetMatchText("������ ���� ��...");
        yield return new WaitForSeconds(0.5f);
        matchUi.SetMatchPanel(false);
        matchUi.SetIngamePanel(true);
        yield return new WaitForSeconds(2f);
        matchUi.VsOff();
        //yield return new WaitForSeconds(3f);
        PlayerReady = true;
    }
    private IEnumerator CoLoadingTextDot()
    {
        string _base = "�Ʒ��� ��Ī ��";
        StringBuilder _dots = new StringBuilder();
        float _matchTimer = 0f;

        while (!countDone)
        {
            if (_dots.Length >= 3)
                _dots.Clear();
            else
                _dots.Append(".");

            matchUi.SetMatchText(_base + _dots.ToString());
            int _min = (int)_matchTimer / 60;
            string timerText = _min >= 10 ? $"{_min}:" : $"0{_min}:";
            timerText += Mathf.FloorToInt(_matchTimer - _min * 60) >= 10 ? $"{Mathf.FloorToInt(_matchTimer - _min * 60)}" :
                $"0{Mathf.FloorToInt(_matchTimer - _min * 60)}";
            matchUi.SetMatchStatusText(timerText);

            yield return new WaitForSeconds(0.5f);
            _matchTimer += 0.5f;
        }
    }
}
