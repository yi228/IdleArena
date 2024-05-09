using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class HeroSlot : MonoBehaviour
{
    public Hero hero;
    public int index;

    [SerializeField] private GameObject selectMark;
    private bool selected = false;
    private Button slotButton;

    void Start()
    {
        slotButton = GetComponent<Button>();
        AddButtonListener();
    }
    private void AddButtonListener()
    {
        slotButton.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        if (hero.own)
        {
            if (selected)
            {
                selected = false;
                selectMark.SetActive(false);
                HeroSystem.instance.selectedSlot = null;
            }
            else
            {
                selected = true;
                if (HeroSystem.instance.selectedSlot != null)
                    HeroSystem.instance.selectedSlot.CancelClick();
                selectMark.SetActive(true);
                HeroSystem.instance.selectedSlot = this;
            }
        }
    }
    public void CancelClick()
    {
        selected = false;
        selectMark.SetActive(false);
    }
}
