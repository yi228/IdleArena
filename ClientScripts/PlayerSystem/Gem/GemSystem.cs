using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemSystem : SystemPanel
{
    public static GemSystem instance;
    [SerializeField] private GameObject gemSlotPrefab;
    [SerializeField] private GameObject scrollContent;
    public List<GemController> gemList;

    public GemController curGem;
    public GemSlot selectedSlot;

    [SerializeField] private GameObject gemPanel;
    [SerializeField] private Image curGemImage;
    [SerializeField] private TextMeshProUGUI curProbText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button discardButton;

    public bool canAdd { get; private set; }

    void Awake()
    {
        instance = this;
        gemList = new List<GemController>();
        canAdd = true;
    }
    void Start()
    {
        //gemPanel.SetActive(false);
        AddButtonListener();
    }
    private void AddButtonListener()
    {
        closeButton.onClick.AddListener(ClosePanel);
        useButton.onClick.AddListener(UseGem);
        discardButton.onClick.AddListener(DiscardGem);
    }
    protected override void ClosePanel()
    {
        base.ClosePanel();
    }
    private void UseGem()
    {
        if(selectedSlot != null)
        {
            if (curGem != null)
                gemList.Add(curGem);
            SetGem(selectedSlot.gem);
            gemList.Remove(curGem);
            Destroy(selectedSlot.gameObject);

            InitGemSystem();
            DBManager.instance.SaveUserInfo();
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GEM_EQUIP");
        }
    }
    public void SetGem(GemController _gem)
    {
        curGem = _gem;
        curGemImage.sprite = curGem.gemImage;
        curProbText.text = curGem.effectProb.ToString();
    }
    private void DiscardGem()
    {
        if (selectedSlot != null)
        {
            gemList.Remove(selectedSlot.gem);
            Destroy(selectedSlot.gameObject);
            SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GEM_DISCARD");
            DBManager.instance.SaveUserInfo();
        }
    }
    public void InitGemSystem()
    {
        if (curGem != null)
        {
            curGemImage.sprite = curGem.gemImage;
            curProbText.text = curGem.effectProb.ToString();
        }  
        else
        {
            curGemImage.sprite = UIManager.instance.nullSprite;
            curProbText.text = " ";
        }

        for(int i=0; i<gemList.Count; i++)
        {
            GameObject _go = Instantiate(gemSlotPrefab, scrollContent.transform);
            _go.GetComponent<GemSlot>().gem = gemList[i];
            _go.GetComponent<GemSlot>().ApplyGem();
        }
    }
}
