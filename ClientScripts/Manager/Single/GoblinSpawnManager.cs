using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSpawnManager : MonoBehaviour
{
    public class TypePercent
    {
        public float normal;
        public float elite;
        public float boss;
    }

    public static GoblinSpawnManager instance;
    [SerializeField] private GameObject goblinPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform[] spawnPos;
    public bool stageChanged = false;
    private bool canSpawn = true;
    private TypePercent ratio;

    private int nameInd = 0;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ratio = new TypePercent();
        ratio = GSSManager.instance.goblinSpawnRatio[GameManager.instance.stageLevel >= 10 ? 9 : GameManager.instance.stageLevel];
    }
    void Update()
    {
        if (stageChanged)
        {
            stageChanged = false;
            ratio = GSSManager.instance.goblinSpawnRatio[GameManager.instance.stageLevel];
        }
        if (canSpawn && !GameManager.instance.player.dead)
        {
            canSpawn = false;
            int _pos = Random.Range(0, 6);
            int _type = Random.Range(0, 100);
            GameObject _go = Instantiate(goblinPrefab, spawnPos[_pos]);
            _go.name = goblinPrefab.name + nameInd.ToString();
            nameInd++;
            GoblinController _gob = _go.GetComponent<GoblinController>();
            //_gob.stat = new CreatureController.Stat();
            if (_type <= ratio.normal)
                _gob.SetCharacter(0);
            else if (_type <= ratio.normal + ratio.elite)
                _gob.SetCharacter(1);
            else
                _gob.SetCharacter(2);
            Invoke("ResetFlag", spawnTime);
        }
    }
    private void ResetFlag()
    {
        canSpawn = true;
    }
}
