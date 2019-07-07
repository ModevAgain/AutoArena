using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="MarineAttackData", menuName = "Data/MarineAttackData")]
public class MarineAttack : EnemyBaseAttack
{
    [Header("References")]
    public GameObject MarineProjectileObject;
    [Header("Data")]
    public float Speed;

    private Vector3 _target;

    public override void StartAttack(Vector3 origin, Vector3 target)
    {
        MarineProjectile temp = Instantiate<GameObject>(MarineProjectileObject).GetComponent<MarineProjectile>();
        temp.Prepare(origin + AttackStartPos, target);

        base.StartAttack(origin, target);
    }
  
}
