using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Idle")]
    public float IdleTime;

    [Header("Movement")]
    public float MoveSpeed;
    public float MoveRange;
    public EnemyBehaviour.MovementType MovementType;

    [Header("Attacks")]
    public float AttackSpeed;
    public int MaxConsecutiveAttacks;
    public EnemyBaseAttack EnemyAttack;

    
}
