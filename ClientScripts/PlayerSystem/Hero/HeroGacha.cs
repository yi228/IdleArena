using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HeroGacha : MonoBehaviour
{
    [SerializeField] private Button oneButton;
    [SerializeField] private Button fiveButton;
    [SerializeField] private Image gachaHeroImage;
    [SerializeField] private TextMeshProUGUI gachaHeroText;
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
        if (GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.HeroTicket) >= 1)
            oneButton.interactable = true;
        else 
            oneButton.interactable = false;
        if (GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.HeroTicket) >= 5)
            fiveButton.interactable = true;
        else
            fiveButton.interactable = false;
    }
    private void GachaOne()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        StartCoroutine(CoGachaHero(true));
    }
    private void GachaFive()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        StartCoroutine(CoGachaHero(false));
    }
    private IEnumerator CoGachaHero(bool _one)
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA");
        if ( _one ? GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.HeroTicket) >= 1 : 
            GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.HeroTicket) >= 5 && canGacha)
        {
            ResetResult();

            if (_one)
                GoodsSystem.instance.UseGoods(GoodsSystem.GoodsType.HeroTicket, 1);
            else
                GoodsSystem.instance.UseGoods(GoodsSystem.GoodsType.HeroTicket, 5);
            canGacha = false;

            for (int i = 0; i < 30; i++)
            {
                int _rand = Random.Range(1, HeroSystem.instance.heroList.Count);
                gachaHeroImage.sprite = HeroSystem.instance.heroList[_rand].image;
                yield return new WaitForSeconds(0.1f);
            }

            if(_one)
            {
                int _ind = DoGacha();
                SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA_COMP");
                gachaHeroImage.sprite = HeroSystem.instance.heroList[_ind].image;
                gachaHeroText.text = HeroSystem.instance.heroList[_ind].name;
                HeroSystem.instance.heroList[_ind].own = true;
                Debug.Log("Gacha result: " + HeroSystem.instance.heroList[_ind].name);
            }
            else
            {
                List<int> _indList = new List<int>();
                for(int i=0; i<5; i++)
                    _indList.Add(DoGacha());
                _indList.Sort(new Comparison<int>((n1, n2) => n2.CompareTo(n1)));

                SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA_COMP");
                gachaHeroImage.sprite = HeroSystem.instance.heroList[_indList[0]].image;
                gachaHeroText.text = HeroSystem.instance.heroList[_indList[0]].name;
                HeroSystem.instance.heroList[_indList[0]].own = true;
                for (int i=1; i < _indList.Count; i++)
                {
                    GameObject _go = Instantiate(gachaSlot, resultPanel.transform);
                    _go.GetComponent<GachaSlot>().SetSlot(_indList[i]);
                    HeroSystem.instance.heroList[_indList[i]].own = true;
                }
            }

            yield return new WaitForSeconds(0.1f);
            canGacha = true;
            HeroSystem.instance.UpdateOwnStatus();
        }
    }
    //0:기본 1:암살자 2:비스트 3:탐험가 4:근위대 5:로마군 6:검투사 7:사무라이
    private int DoGacha()
    {
        int _num = Random.Range(0, 10);
        int _type, _ind = 0;
        if (_num < 5)
        {
            _type = Random.Range(0, 3);
            switch (_type)
            {
                case 0:
                    _ind = 1;
                    break;
                case 1:
                    _ind = 2;
                    break;
                case 2:
                    _ind = 3;
                    break;
            }
        }
        else if (_num < 8)
        {
            _type = Random.Range(0, 2);
            switch (_type)
            {
                case 0:
                    _ind = 4;
                    break;
                case 1:
                    _ind = 5;
                    break;
            }
        }
        else
        {
            _type = Random.Range(0, 2);
            switch (_type)
            {
                case 0:
                    _ind = 6;
                    break;
                case 1:
                    _ind = 7;
                    break;
            }
        }
        return _ind;
    }
    public void SetTicketText()
    {
        ticketText.text = GoodsSystem.instance.GetGoodsValue(GoodsSystem.GoodsType.HeroTicket).ToString();
    }
    public void ResetResult()
    {
        foreach(GachaSlot _go in resultPanel.GetComponentsInChildren<GachaSlot>())
            Destroy( _go.gameObject );

        gachaHeroImage.sprite = UIManager.instance.nullSprite;
        gachaHeroText.text = "";
    }
}
