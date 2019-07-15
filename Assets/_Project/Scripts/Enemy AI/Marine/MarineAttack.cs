using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

//[CreateAssetMenu(fileName ="MarineAttackData", menuName = "Data/MarineAttackData")]
public class MarineAttack : EnemyBaseAttack
{
    [Header("References")]
    public GameObject MarineProjectileObject;
    [Header("Data")]
    public float Speed;

    private Vector3 _target;
    

    public override void StartAttack(Vector3 origin, Vector3 target)
    {
        FireAttack(origin, target);
        base.StartAttack(origin, target);
    }

    public void FireAttack(Vector3 origin, Vector3 target)
    {
        MarineProjectile temp = Instantiate<GameObject>(MarineProjectileObject).GetComponent<MarineProjectile>();
        temp.Prepare(origin, target, Damage);
    }
  
}
