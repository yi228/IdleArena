using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rig
{
    public enum Type
    {
        Armor,
        Weapon
    }
    public enum EffectType
    {
        AttackBuff,
        SkillCool,
        AttackDist,
        Defense,
        Speed,
        Gold
    }

    public int index;
    public Type type;
    public string name;
    public EffectType effectType;
    public int effectProb;
    public string description;
}
