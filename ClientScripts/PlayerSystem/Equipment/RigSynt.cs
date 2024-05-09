using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RigSynt : MonoBehaviour
{
    private Rig sacrificeRig;
    private Rig resultRig;

    [Header("Image")]
    [SerializeField] private Image sacImage;
    [SerializeField] private Image resultImage;
    [Header("Button")]
    [SerializeField] private Button syntButton;

    void Start()
    {
        syntButton.onClick.AddListener(SynthesisRig);
    }
    private void SynthesisRig()
    {
        EquipmentSystem.instance.ownRigNum[sacrificeRig.index] -= 3;
        StartCoroutine(CoSynthesisRig());
    }
    private IEnumerator CoSynthesisRig()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA");

        for (int i = 0; i < 30; i++)
        {
            int _rand = Random.Range(1, EquipmentSystem.instance.rigList.Count);
            resultImage.sprite = EquipmentSystem.instance.itemSprites[_rand];
            yield return new WaitForSeconds(0.1f);
        }

        resultRig = EquipmentSystem.instance.rigList[Random.Range(0, EquipmentSystem.instance.rigList.Count)];
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_GACHA_COMP");

        resultImage.sprite = EquipmentSystem.instance.itemSprites[resultRig.index];
        resultImage.GetComponentInChildren<TextMeshProUGUI>().text = resultRig.name;

        EquipmentSystem.instance.ownRigNum[resultRig.index]++;
        EquipmentSystem.instance.UpdateRigSlot();

        Debug.Log("Synthesis result: " + resultRig.name);
    }
    public void SetSacrifice(Rig _rig)
    {
        ResetRig();

        sacrificeRig = _rig;
        sacImage.sprite = EquipmentSystem.instance.itemSprites[sacrificeRig.index];
        sacImage.GetComponentInChildren<TextMeshProUGUI>().text = sacrificeRig.name;
    }
    public void ResetRig()
    {
        sacrificeRig = null;
        sacImage.sprite = UIManager.instance.nullSprite;
        sacImage.GetComponentInChildren<TextMeshProUGUI>().text = "";

        resultRig = null;
        resultImage.sprite = UIManager.instance.nullSprite;
        resultImage.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }
}
