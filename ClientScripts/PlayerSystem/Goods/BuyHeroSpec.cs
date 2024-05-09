using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHeroSpec : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button[] buyButtons;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePanel);
    }
    private void ClosePanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GetComponent<Animator>().SetTrigger("Off");
    }
    public void InitButtons()
    {
        for (int i = 0; i < buyButtons.Length; i++)
            buyButtons[i].GetComponent<HeroBuySlot>().SetButtonInteractable();
    }
}
