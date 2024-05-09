using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_ArenaResult : MonoBehaviour
{
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject loseImage;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button exitButton;

    public void Win()
    {
        winImage.SetActive(true);
        SoundManager.instance.PlayClip(GetComponentInParent<AudioSource>(), "UI_STAGE_CLEAR");

        ArenaManager.instance.score += ArenaManager.instance.point;

        scoreText.text = ArenaManager.instance.score.ToString();
    }
    public void Lose()
    {
        loseImage.SetActive(true);
        SoundManager.instance.PlayClip(GetComponentInParent<AudioSource>(), "DIE");

        ArenaManager.instance.score -= ArenaManager.instance.point;
        if (ArenaManager.instance.score < 0)
            ArenaManager.instance.score = 0;

        scoreText.text = ArenaManager.instance.score.ToString();
    }
    void Start()
    {
        exitButton.onClick.AddListener(Exit);
    }
    private void Exit()
    {
        SoundManager.instance.PlayClip(GetComponentInParent<AudioSource>(), "UI_CLICK");
        Managers.Object.Remove(Managers.Object.MyPlayerId);
        Managers.Clear();
        SceneManager.LoadScene("LoadingScene");
    }
}
