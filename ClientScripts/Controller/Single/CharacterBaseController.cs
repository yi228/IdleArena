using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseController : MonoBehaviour
{
    private GameObject left;
    private GameObject right;
    protected AnimationManager animManager;

    protected void SetLeftRight()
    {
        left = transform.Find("Left").gameObject;
        right = transform.Find("Right").gameObject;
    }
    public void ChangeSide(bool _left)
    {
        if (_left)
        {
            left.SetActive(true);
            right.SetActive(false);
        }
        else
        {
            right.SetActive(true);
            left.SetActive(false);
        }
    }
    public void IdleAnimation()
    {
        if (animManager == null)
            return;

        animManager.SetState(CharacterState.Ready);
    }
    public void MoveAnimation()
    {
        if (animManager == null)
            return;

        animManager.SetState(CharacterState.Run);
    }
    public void AttackAnimation(bool twoHand = false)
    {
        if (animManager == null)
            return;

        animManager.Slash(twoHand);
    }
    public void SkillAnimation()
    {
        if (animManager == null)
            return;

        animManager.Jab();
    }
    public void DamageAnimation()
    {
        if (animManager == null)
            return;

        animManager.Hit();
    }
    public void DeadAnimation()
    {
        if (animManager == null)
            return;

        animManager.Die();
    }
    public void DanceAnimation()
    {
        if (animManager == null)
            return;

        animManager.SetState(CharacterState.Dance);
    }
}
