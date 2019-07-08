using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAttack : ScriptableObject
{
    public float Damage;
    public float Range;
    public Vector3 AttackStartPos;

    public delegate void EnemyAttackEvent();

    public EnemyAttackEvent AttackStarted;
    public EnemyAttackEvent AttackFinished;

    public virtual void StartAttack(Vector3 origin, Vector3 target)
    {
        AttackStarted?.Invoke();
    }

    public virtual void FinishAttack()
    {
        AttackFinished?.Invoke();
    }
    

}
