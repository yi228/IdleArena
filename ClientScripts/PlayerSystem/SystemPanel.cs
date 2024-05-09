using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPanel : MonoBehaviour
{
    protected virtual void ClosePanel()
    {
        SoundManager.instance.PlayClip(UIManager.instance.audioSource, "UI_CLICK");
        GetComponent<Animator>().SetTrigger("Off");
    }
}
