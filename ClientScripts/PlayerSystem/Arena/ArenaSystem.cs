using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaSystem : SystemPanel
{
    public static ArenaSystem instance;

    [SerializeField] private TextMeshProUGUI rankingText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button matchButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject slotPanel;
    [SerializeField] private GameObject rankingSlotPrefab;
    public Sprite[] topRankIcons;

    public int rank;
    public int score;

    public struct Ranker
    {
        public string name;
        public int rank;
        public int score;
    }

    public List<Ranker> rankerList = new List<Ranker>();

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
    private void GoArena()
    {
        ArenaManager.instance.score = score;
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        SceneManager.LoadScene("ArenaScene");
    }
    private void AddButtonEvent()
    {
        closeButton.onClick.AddListener(ClosePanel);
        matchButton.onClick.AddListener(GoArena);
    }
    public void SetRank()
    {
        StartCoroutine(CoSetRankingUI());
    }
    private IEnumerator CoSetRankingUI()
    {
        ResetRankingSlot();
        DBManager.instance.LoadRankingData();

        rankingText.text = "";
        scoreText.text = "";

        yield return new WaitForSeconds(0.5f);

        rankingText.text = $"{rank}À§";
        scoreText.text = score.ToString();

        for (int i = 0; i < rankerList.Count; i++)
        {
            Ranker _temp = rankerList[i];
            GameObject _go = Instantiate(rankingSlotPrefab, slotPanel.transform);
            _go.GetComponent<RankingSlot>().SetSlot(_temp.name, _temp.rank, _temp.score);
        }
    }
    private void ResetRankingSlot()
    {
        foreach (RankingSlot _slot in slotPanel.GetComponentsInChildren<RankingSlot>())
            Destroy(_slot.gameObject);

        rankerList.Clear();
    }
}
