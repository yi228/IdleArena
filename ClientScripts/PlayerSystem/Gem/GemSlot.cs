using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemSlot : MonoBehaviour
{
    public GemController gem;
    public int index;
    [SerializeField] private Image slotGemImage;
    [SerializeField] private GameObject selectMark;
    [SerializeField] private TextMeshProUGUI probText;
    private bool selected = false;
    private Button slotButton;

    void Start()
    {
        slotButton = GetComponent<Button>();
        AddButtonListener();
        probText.text = gem.effectProb.ToString();
    }
    private void AddButtonListener()
    {
        slotButton.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        if (selected)
        {
            selected = false;
            selectMark.SetActive(false);
            GemSystem.instance.selectedSlot = null;
        }
        else
        {
            selected = true;
            if(GemSystem.instance.selectedSlot != null)
                GemSystem.instance.selectedSlot.CancelClick();
            selectMark.SetActive(true);
            GemSystem.instance.selectedSlot = this;
        }
    }
    public void CancelClick()
    {
        selected = false;
        selectMark.SetActive(false);
    }
    public void ApplyGem()
    {
        slotGemImage.sprite = gem.gemImage;
    }
}
