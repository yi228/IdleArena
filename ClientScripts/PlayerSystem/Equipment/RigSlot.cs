using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RigSlot : MonoBehaviour
{
    protected Rig rig;
    protected int num;

    [SerializeField] private TextMeshProUGUI numText;

    protected void SetUI(Rig _rig, int _num)
    {
        rig = _rig;
        num = _num;

        GetComponent<Image>().sprite = EquipmentSystem.instance.itemSprites[rig.index];
        numText.text = num.ToString();
    }
    public void SetSlot(Rig _rig, int _num)
    {
        SetUI(_rig, _num);

        if (num == 0)
            GetComponent<Button>().interactable = false;

        GetComponent<Button>().onClick.AddListener(() => EquipmentSystem.instance.OpenRigSpec(rig));
    }
    public virtual void SetNum(int _num)
    {
        num = _num;

        if (num <= 0)
            GetComponent<Button>().interactable = false;
        else
            GetComponent<Button>().interactable = true;

        numText.text = num.ToString();
    }
}
