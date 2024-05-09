using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerIdleController player;

    public int playerLevel = 0;
    public int stageLevel = 0;
    public int killCount = 0;
    public int statUpPoint = 0;
    public int maxKillCount;
    public int curHeroInd = 0;

    public GameObject gemPrefab;
    public Transform gemStorePos;

    void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion
    }
    void Start()
    {
        maxKillCount = (stageLevel + 1) * 5;
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" && killCount >= maxKillCount)
            StageLevelUp();
        if (SceneManager.GetActiveScene().name == "ArenaScene")
            ;
    }
    public void StageLevelUp()
    {
        stageLevel++;
        maxKillCount = (stageLevel + 1) * 5;
        MapSingleManager.instance.ChangeStage(stageLevel);
        killCount = 0;
        GoblinSpawnManager.instance.stageChanged = true;
        UIManager.instance.ClearPanelOn();
    }
}
