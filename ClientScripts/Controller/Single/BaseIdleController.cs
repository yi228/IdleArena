using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIdleController : MonoBehaviour
{
    public class Stat
    {
        public float maxHp;
        public float hp;
        public float speed;
        public float attack;
        public float attackCool;
        public float exp;
        public float maxExp;
    }

    public Stat stat;
    public bool dead = false;

    protected enum Dir
    {
        //UP,
        //DOWN,
        LEFT,
        RIGHT
    }
    protected Dir moveDir;

    protected enum CreatureState
    {
        IDLE,
        MOVE,
        ATTACK
    }
    protected CreatureState state;

    protected virtual void UpdateState() { }
    protected virtual void SetDir() { }
    protected virtual void Chase()
    {
        state = CreatureState.MOVE;
    }
    protected virtual void Attack()
    {
        state = CreatureState.ATTACK;
    }
    public virtual void OnDamaged(float _damage)
    {
        stat.hp -= _damage;
        if (stat.hp <= 0)
            OnDead();
    }
    protected virtual void OnDead()
    {
        Debug.Log(name + " dead");
        dead = true;
    }
}
