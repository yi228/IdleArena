using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    [SerializeField] private Image storyImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI adviceText;
    [SerializeField] private Button skipButton;

    [SerializeField] private List<Sprite> storyImageData;
    public List<string> storyTextData;

    private int index = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        var _panTemp = GetComponent<Image>().color;
        var _imageTemp = storyImage.color;
        var _textTemp = storyText.color;

        _panTemp.a = 0f;
        _imageTemp.a = 0f;
        _textTemp.a = 0f;
        adviceText.gameObject.SetActive(false);

        GetComponent<Image>().color = _panTemp;
        storyImage.color = _imageTemp;
        storyText.color = _textTemp;

        skipButton.onClick.AddListener(SkipStory);
        skipButton.gameObject.SetActive(false);
    }
    private void SkipStory()
    {
        if(index < storyImageData.Count)
        {
            if (storyText.text != storyTextData[index])
            {
                StopAllCoroutines();
                storyText.text = storyTextData[index];
            }
            else
            {
                index++;
                PlayStory();
            }
        }  
    }
    public void PanelOn()
    {
        StartCoroutine(CoPanelOn());
    }
    private void PanelOff()
    {
        Debug.Log("PanelOff");
        StartCoroutine(CoGetDark());
    }
    private IEnumerator CoPanelOn()
    {
        Image _panelImage = GetComponent<Image>();

        while (_panelImage.color.a < 1f)
        {
            var _panTemp = _panelImage.color;
            var _imageTemp = storyImage.color;
            var _textTemp = storyText.color;

            _panTemp.a += (1f * Time.deltaTime);
            _imageTemp.a += (1f * Time.deltaTime);
            _textTemp.a += (1f * Time.deltaTime);

            _panelImage.color = _panTemp;
            storyImage.color = _imageTemp;
            storyText.color = _textTemp;
            adviceText.gameObject.SetActive(true);

            yield return null;
        }

        skipButton.gameObject.SetActive(true);
        PlayStory();
    }
    private IEnumerator CoGetDark()
    {
        Image _panelImage = skipButton.GetComponent<Image>();

        while (_panelImage.color.a < 1f)
        {
            var _panTemp = _panelImage.color;
            _panTemp.a += (1f * Time.deltaTime);
            _panelImage.color = _panTemp;
            yield return null;
        }

        SceneManager.LoadScene("MainScene");
    }
    private void PlayStory()
    {
        if (index >= storyImageData.Count)
            PanelOff();
        else
        {
            storyImage.sprite = storyImageData[index];
            StartCoroutine(CoTypeStory(0.04f, storyTextData[index]));
        }
    }
    private IEnumerator CoTypeStory(float interval, string text)
    {
        storyText.text = string.Empty;

        StringBuilder _temp = new StringBuilder();

        for(int i=0; i< text.Length; i++)
        {
            _temp.Append(text[i]);
            storyText.text = _temp.ToString();
            yield return new WaitForSeconds(interval);
        }
    }
}
