using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EarnedItemSlot : MonoBehaviour
{
    private EarnedItemPanel.ItemType type;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI mainText;

    public void SetSlot(EarnedItemPanel.ItemType _type, string _text)
    {
        type = _type;

        itemIcon.sprite = EarnedItemPanel.instance.itemImages[(int)type];
        mainText.text = _text;
        StartCoroutine(CoLerpAlphaDown());
    }
    private IEnumerator CoLerpAlphaDown()
    {
        yield return new WaitForSeconds(1f);

        while (GetComponent<Image>().color.a > 0f)
        {
            var _panTemp = GetComponent<Image>().color;
            var _icoTemp = itemIcon.color;
            var _texTemp = mainText.color;

            _panTemp.a -= (1f * Time.deltaTime);
            _icoTemp.a -= (1f * Time.deltaTime);
            _texTemp.a -= (1f * Time.deltaTime);

            GetComponent<Image>().color = _panTemp;
            itemIcon.color = _icoTemp;
            mainText.color = _texTemp;

            yield return null;
        }
        Destroy(gameObject);
    }
}
