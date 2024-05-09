using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaSlot : MonoBehaviour
{
    [SerializeField] private Image heroImage;
    [SerializeField] private Sprite[] frameSprites;

    public void SetSlot(int _ind)
    {
        heroImage.sprite = HeroSystem.instance.heroList[_ind].image;

        int _frameInd;
        if (_ind <= 3)
            _frameInd = 0;
        else if(_ind <= 5)
            _frameInd = 1;
        else
            _frameInd = 2;
        GetComponent<Image>().sprite = frameSprites[_frameInd];
    }
}
