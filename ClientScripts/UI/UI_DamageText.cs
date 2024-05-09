using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DamageText : MonoBehaviour
{
    public float damage;
    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        text.text = damage.ToString();
        StartCoroutine(CoAlphaLerpDown());
    }
    private IEnumerator CoAlphaLerpDown()
    {
        while (text.color.a > 0f)
        {
            var _temp = text.color;
            _temp.a -= (1f * Time.deltaTime);
            text.color = _temp;
            yield return null;
        }
    }
}
