using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TouchEffectCreator : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    private float spawnTime;
    private float defaultTime = 0.05f;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && spawnTime >= defaultTime)
        {
            spawnTime = 0;
            CreateEffect();
        }
        spawnTime += Time.deltaTime;
    }
    private void CreateEffect()
    {
        Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
        Instantiate(effectPrefab, _mousePos, Quaternion.identity);
    }
}
