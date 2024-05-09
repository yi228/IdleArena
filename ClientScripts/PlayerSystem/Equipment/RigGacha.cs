using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RigGacha : MonoBehaviour
{
    [SerializeField] private Button oneButton;
    [SerializeField] private Button fiveButton;
    [SerializeField] private Image gachaRigImage;
    [SerializeField] private TextMeshProUGUI gachaRigText;
    [SerializeField] private TextMeshProUGUI ticketText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject gachaSlot;

    private bool canGacha = true;

    void Start()
    {
        oneButton.onClick.AddListener(GachaOne);
        fiveButton.onClick.AddListener(GachaFive);
    }
    void Update()
    {
        if (GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.EquipmentTicket) >= 1)
            oneButton.interactable = true;
        else
            oneButton.interactable = false;
        if (GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.EquipmentTicket) >= 5)
            fiveButton.interactable = true;
        else
            fiveButton.interactable = false;
    }
    private void GachaOne()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        StartCoroutine(CoGachaRig(true));
    }
    private void GachaFive()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        StartCoroutine(CoGachaRig(false));
    }
    private IEnumerator CoGachaRig(bool _one)
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA");
        if (_one ? GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.EquipmentTicket) >= 1 :
            GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.EquipmentTicket) >= 5 && canGacha)
        {
            ResetResult();

            if (_one)
                GoodsSystem.instance.UseGoods(GoodsSystem.GoodsType.EquipmentTicket, 1);
            else
                GoodsSystem.instance.UseGoods(GoodsSystem.GoodsType.EquipmentTicket, 5);
            canGacha = false;

            SetTicketText();

            for (int i = 0; i < 30; i++)
            {
                int _rand = Random.Range(1, EquipmentSystem.instance.rigList.Count);
                gachaRigImage.sprite = EquipmentSystem.instance.itemSprites[_rand];
                yield return new WaitForSeconds(0.1f);
            }

            if (_one)
            {
                int _ind = DoGacha();
                SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA_COMP");
                gachaRigImage.sprite = EquipmentSystem.instance.itemSprites[_ind];
                gachaRigText.text = EquipmentSystem.instance.rigList[_ind].name;
                EquipmentSystem.instance.ownRigNum[_ind]++;
                Debug.Log("Gacha result: " + EquipmentSystem.instance.rigList[_ind].name);
            }
            else
            {
                List<int> _indList = new List<int>();
                for (int i = 0; i < 5; i++)
                    _indList.Add(DoGacha());
                _indList.Sort();

                SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA_COMP");
                gachaRigImage.sprite = EquipmentSystem.instance.itemSprites[_indList[0]];
                gachaRigText.text = EquipmentSystem.instance.rigList[_indList[0]].name;
                EquipmentSystem.instance.ownRigNum[_indList[0]]++;

                for (int i = 1; i < _indList.Count; i++)
                {
                    GameObject _go = Instantiate(gachaSlot, resultPanel.transform);
                    _go.GetComponent<RigGachaSlot>().SetSlot(_indList[i]);
                    EquipmentSystem.instance.ownRigNum[_indList[i]]++;
                }
            }

            yield return new WaitForSeconds(0.1f);
            canGacha = true;
            EquipmentSystem.instance.UpdateRigSlot();
        }
    }
    private int DoGacha()
    {
        int _ind = Random.Range(0, EquipmentSystem.instance.rigList.Count);
        return _ind;
    }
    public void SetTicketText()
    {
        ticketText.text = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.EquipmentTicket).ToString();
    }
    public void ResetResult()
    {
        foreach (RigGachaSlot _go in resultPanel.GetComponentsInChildren<RigGachaSlot>())
            Destroy(_go.gameObject);

        gachaRigImage.sprite = UIManager.instance.nullSprite;
        gachaRigText.text = "";
    }
}

