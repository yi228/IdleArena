using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public bool matchOver = false;

    public PlayerController Enemy;
    public MyPlayerController MyPlayer;

    public void Clear()
    {
        matchOver = false;
    }
}
 