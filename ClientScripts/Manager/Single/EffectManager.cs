using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public Dictionary<string, GameObject> effectDict = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject[] effects;

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
        AddEffect();
    }
    private void AddEffect()
    {
        for (int i = 0; i < effects.Length; i++)
            effectDict.Add(effects[i].name, effects[i]);
    }
    public void InstantiateEffect(string _effectKey, Vector3 _pos, string _dir, float _duration)
    {
        GameObject _go = Instantiate(effectDict[_effectKey]);
        _go.transform.position = _pos;
        if(_dir == "L")
            _go.transform.eulerAngles = new Vector3(_go.transform.eulerAngles.x, 180, _go.transform.eulerAngles.z);
        else if(_dir == "R")
            _go.transform.eulerAngles = new Vector3(_go.transform.eulerAngles.x, 0, _go.transform.eulerAngles.z);

        Destroy(_go, _duration);
    }
}
