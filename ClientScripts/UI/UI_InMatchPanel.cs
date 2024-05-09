using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InMatchPanel : MonoBehaviour
{
    [SerializeField] private GameObject vsImage;
    [Header("MyPlayer")]
    [SerializeField] private TextMeshProUGUI mNameText;
    [SerializeField] private TextMeshProUGUI mScoreText;
    [Header("Enemy")]
    [SerializeField] private TextMeshProUGUI eNameText;
    [SerializeField] private TextMeshProUGUI eScoreText;

    public void VsOff()
    {
        vsImage.SetActive(false);
    }
    public void PlayNameAnimation()
    {
        mNameText.text = Managers.Match.MyPlayer.name;
        eNameText.text = Managers.Match.Enemy.name;
        mScoreText.text = Managers.Match.MyPlayer.Stat.Score.ToString();
        eScoreText.text = Managers.Match.Enemy.Stat.Score.ToString();
        mNameText.GetComponentInParent<Animator>().SetTrigger("Start");
        eNameText.GetComponentInParent<Animator>().SetTrigger("Start");
    }
}
