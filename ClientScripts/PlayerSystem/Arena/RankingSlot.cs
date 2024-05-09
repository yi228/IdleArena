using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image topRankImage;

    public void SetSlot(string _name, int _rank, long _score)
    {
        nameText.text = _name;
        if(_rank < 3)
        {
            rankText.gameObject.SetActive(false);
            topRankImage.sprite = ArenaSystem.instance.topRankIcons[_rank - 1];
        }
        else
        {
            topRankImage.gameObject.SetActive(false);
            rankText.text = _rank.ToString();
        } 
        scoreText.text = _score.ToString();
    }
}
