using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_MatchingStatus : MonoBehaviour
{
    public TextMeshProUGUI t;

    void Start()
    {
        t = GetComponent<TextMeshProUGUI>();    
    }
}
