using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingSlider;

    private float loadingProgress = 0f;

    private bool storyOn = false;
    private bool dbCheck = false;

    void Start()
    {
        StartCoroutine(CoLoadingTextDot());
        StartCoroutine(CoLoadingSlider());
        loadingSlider.value = loadingProgress;
    }
    void Update()
    {
        if (!dbCheck && DBManager.instance.loadComplete)
            dbCheck = true;

        if (dbCheck)
            loadingProgress = GSSManager.instance.loadingProgress + 1f / 7f;
        else
            loadingProgress = GSSManager.instance.loadingProgress;

        if (loadingSlider.value == 1f)
        {
            if (DBManager.instance.isFirst && !storyOn)
            {
                storyOn = true;
                StoryManager.instance.PanelOn();
            }
            else if (!DBManager.instance.isFirst)
                SceneManager.LoadScene("MainScene");
        }
    }
    private IEnumerator CoLoadingTextDot()
    {
        string _base = "·Îµù Áß";
        StringBuilder _dots = new StringBuilder();

        while (true)
        {
            if(_dots.Length >= 3)
                _dots.Clear();
            else
                _dots.Append(".");

            loadingText.text = _base + _dots.ToString();

            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator CoLoadingSlider()
    {
        while (loadingSlider.value <= 1f)
        {
            if(loadingSlider.value <= loadingProgress)
                loadingSlider.value += 0.3f * Time.deltaTime;
            yield return null;
        }
    }
}
