using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RigGachaSlot : MonoBehaviour
{
    [SerializeField] private Image rigImage;

    public void SetSlot(int _ind)
    {
        rigImage.sprite = EquipmentSystem.instance.itemSprites[_ind];
    }
}
