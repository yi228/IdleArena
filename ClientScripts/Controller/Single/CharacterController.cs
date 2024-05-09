using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CharacterController : CharacterBaseController
{
    public BaseIdleController creatureController;
    public BaseIdleController target;

    void Start()
    {
        SetLeftRight();
        animManager = GetComponent<AnimationManager>();
        if(GetComponentInParent<BaseIdleController>() != null )
            creatureController = GetComponentInParent<BaseIdleController>();
    }
    public void DoAttack()
    {
        if (target == null || creatureController == null)
            return;

        float _appliedDamage = creatureController.stat.attack;
        if (GetComponentInParent<PlayerIdleController>() != null)
        {
            _appliedDamage += GetComponentInParent<PlayerIdleController>().extraAttack + GetComponentInParent<PlayerIdleController>().gemAttack;
            GetComponentInParent<PlayerIdleController>().InstantiateDamageText(_appliedDamage);
            GetComponentInParent<PlayerIdleController>().InstantiateAttackEffect();
            SoundManager.instance.PlayClip(GetComponentInParent<AudioSource>(), "ATTACK");
        }
        target.OnDamaged(_appliedDamage);
    }
}
