using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EarnedItemPanel : MonoBehaviour
{
    public static EarnedItemPanel instance;

    public enum ItemType
    {
        Gold,
        Exp,
        Gem
    }

    [SerializeField] private GameObject slotPrefab;
    public Sprite[] itemImages;

    void Awake()
    {
        instance = this;
    }
    public void InstantiateSlot(ItemType _type, string _text)
    {
        GameObject _go = Instantiate(slotPrefab, transform);
        _go.GetComponent<EarnedItemSlot>().SetSlot(_type, _text);
    }
}
