using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SyntSlot : RigSlot
{
    public void SetSyntSlot(Rig _rig, int _num)
    {
        SetUI(_rig, _num);

        GetComponent<Button>().onClick.AddListener(() => EquipmentSystem.instance.SetSacRig(rig));

        if (_num < 3)
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;
    }
    public override void SetNum(int _num)
    {
        base.SetNum(_num);

        if (_num < 3)
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;
    }
}
