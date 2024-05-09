using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMultiController : CharacterBaseController
{
    public PlayerController playerController;

    void Start()
    {
        SetLeftRight();
        animManager = GetComponent<AnimationManager>();
        if (GetComponentInParent<PlayerController>() != null)
            playerController = GetComponentInParent<PlayerController>();
    }
}
