using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    #region references
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI statPointText;
    [SerializeField] private TextMeshProUGUI questPointText;
    [SerializeField] private TextMeshProUGUI overTimerText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI diaText;
    [Header("Slider")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider expSlider;
    [Header("DockButton")]
    [SerializeField] private Button statButton;
    [SerializeField] private Button gemButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button skillButton;
    [SerializeField] private Button heroButton;
    [SerializeField] private Button questButton;
    [SerializeField] private Button arenaButton;
    [SerializeField] private Button storeButton;
    [SerializeField] private Button mailButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [Header("SettingButton")]
    [SerializeField] private Button settingCloseButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button effectSoundButton;
    [Header("Image")]
    [SerializeField] private Image evolutionPanel;
    [SerializeField] private Image stageClearPanel;
    [Header("Object")]
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private GameObject statPanel;
    [SerializeField] private GameObject gemPanel;
    [SerializeField] private GameObject mailPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject storePanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject equipPanel;
    [SerializeField] private GameObject arenaPanel;
    [SerializeField] private GameObject dockBar;
    [Header("Sprite")]
    public Sprite nullSprite;
    [SerializeField] private Sprite[] soundSprite;
    #endregion

    private GameManager gameManager;
    public AudioSource audioSource {  get; private set; }   

    void Start()
    {
        instance = this;
        gameManager = GameManager.instance;
        audioSource = GetComponent<AudioSource>();
        AddButtonEvent();
    }
    void Update()
    {
        UpdateSlider();
        UpdateText();
    }
    private void UpdateSlider()
    {
        hpSlider.value = gameManager.player.stat.hp / gameManager.player.stat.maxHp;
        expSlider.value = gameManager.player.stat.exp / gameManager.player.stat.maxExp;
    }
    private void UpdateText()
    {
        levelText.text = (GameManager.instance.playerLevel + 1).ToString();
        stageText.text = $"스테이지 {gameManager.stageLevel + 1} <color #00FF18> {gameManager.killCount}/{gameManager.maxKillCount}</color>";
        statPointText.text = GameManager.instance.statUpPoint.ToString();
        questPointText.text = QuestSystem.instance.clearQuestNum.ToString();
        goldText.text = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.Gold).ToString();
        diaText.text = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.Diamond).ToString();
    }
    private void AddButtonEvent()
    {
        statButton.onClick.AddListener(() => OpenPanel(statPanel));
        statButton.onClick.AddListener(() => statPanel.GetComponent<StatSystem>().ApplyText());

        gemButton.onClick.AddListener(() => OpenPanel(gemPanel));
        gemButton.onClick.AddListener(() => GemSystem.instance.InitGemSystem());

        equipButton.onClick.AddListener(() => OpenPanel(equipPanel));
        equipButton.onClick.AddListener(() => EquipmentSystem.instance.SetImageSprite());
        equipButton.onClick.AddListener(() => EquipmentSystem.instance.SetEffectText());

        heroButton.onClick.AddListener(() => OpenPanel(heroPanel));
        heroButton.onClick.AddListener(() => HeroSystem.instance.UpdateOwnStatus());

        arenaButton.onClick.AddListener(() => OpenPanel(arenaPanel));
        arenaButton.onClick.AddListener(() => ArenaSystem.instance.SetRank());

        mailButton.onClick.AddListener(() => OpenPanel(mailPanel));
        mailButton.onClick.AddListener(() => MailSystem.instance.SetMailSlot());

        storeButton.onClick.AddListener(() => OpenPanel(storePanel));
        storeButton.onClick.AddListener(() => GoodsSystem.instance.InitUI());

        questButton.onClick.AddListener(() => OpenPanel(questPanel));

        skillButton.onClick.AddListener(() => OpenPanel(skillPanel));

        settingButton.onClick.AddListener(() => OpenPanel(settingPanel));
        settingCloseButton.onClick.AddListener(() => settingPanel.GetComponent<Animator>().SetTrigger("Off"));
        settingCloseButton.onClick.AddListener(() => SoundManager.instance.PlayClip(audioSource, "UI_CLICK"));
        bgmButton.onClick.AddListener(ShiftBGM);
        effectSoundButton.onClick.AddListener(ShiftEffect);

        rightButton.onClick.AddListener(() => ShiftDock(false));
        leftButton.onClick.AddListener(() => ShiftDock(true));
    }
    private void OpenPanel(GameObject _panel)
    {
        SoundManager.instance.PlayClip(audioSource, "UI_CLICK");
        _panel.GetComponent<Animator>().SetTrigger("On");
    }
    public void EvoPanelOn()
    {
        SoundManager.instance.PlayClip(audioSource, "UI_EVOLUTION");
        StartCoroutine(CoLerpAlpha(evolutionPanel, evolutionPanel.GetComponentInChildren<TextMeshProUGUI>()));
    }
    public void ClearPanelOn()
    {
        SoundManager.instance.PlayClip(audioSource, "UI_STAGE_CLEAR");
        StartCoroutine(CoLerpAlpha(stageClearPanel, stageClearPanel.GetComponentInChildren<TextMeshProUGUI>()));
    }
    private IEnumerator CoLerpAlpha(Image _panel, TextMeshProUGUI _text)
    {
        while (_panel.color.a < 1f)
        {
            var _panTemp = _panel.color;
            var _texTemp = _text.color;
            _panTemp.a += (1f * Time.deltaTime);
            _texTemp.a += (1f * Time.deltaTime);
            _panel.color = _panTemp;
            _text.color = _texTemp;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        while (_panel.color.a > 0f)
        {
            var _panTemp = _panel.color;
            var _texTemp = _text.color;
            _panTemp.a -= (0.5f * Time.deltaTime);
            _texTemp.a -= (0.5f * Time.deltaTime);
            _panel.color = _panTemp;
            _text.color = _texTemp;
            yield return null;
        }
    }
    private void ShiftBGM()
    {
        SoundManager.instance.ShiftOnOffBGM();
        bgmButton.image.sprite = SoundManager.instance.bgmSource.mute ? soundSprite[1] : soundSprite[0];
    }
    private void ShiftEffect()
    {
        SoundManager.instance.ShiftOnOffEffect();
        effectSoundButton.image.sprite = SoundManager.instance.effectMute ? soundSprite[1] : soundSprite[0];
    }
    public void SetOverUI(bool _active)
    {
        gameOverPanel.SetActive(_active);
    }
    public void SetOverTimer(int _sec)
    {
        overTimerText.text = $"부활까지 {_sec}초";
    }
    private void ShiftDock(bool _left)
    {
        SoundManager.instance.PlayClip(audioSource, "UI_CLICK");
        string _trigger = "";
        if (_left)
            _trigger = "Left";
        else
            _trigger = "Right";

        dockBar.GetComponent<Animator>().SetTrigger(_trigger);
    }
}
