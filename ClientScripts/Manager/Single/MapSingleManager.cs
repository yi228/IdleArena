using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSingleManager : MonoBehaviour
{
    public static MapSingleManager instance;

    [SerializeField] private GameObject[] maps;

    void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        #endregion
    }
    public void ChangeStage(int stageLevel)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == stageLevel % 3)
                maps[i].SetActive(true);
            else
                maps[i].SetActive(false);
        }
    }
}
